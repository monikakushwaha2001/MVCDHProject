using Microsoft.AspNetCore.Mvc;
using MVCDHProject.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MailKit.Net.Smtp;
namespace MVCDHProject.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser>userManager;
		private readonly SignInManager<IdentityUser> signInManager;
		public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser>signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		#region Register
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(UserModel userModel)
		{
			if (ModelState.IsValid)
			{
				//IdentityUser represents a new user with given set of attributes.
				IdentityUser identityUser = new IdentityUser
				{
					UserName = userModel.Name,
					Email = userModel.Email,
					PhoneNumber = userModel.Mobile
				};
				//Creates a new user and returns a result which tells about success or failure.
				var result = await userManager.CreateAsync(identityUser, userModel.Password);
				if (result.Succeeded)
				{
					//Implementation logic for sending a mail to confirm the email

					var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
					var confirmationUrlLink = Url.Action("ConfirmEmail", "Account", new { UserId = identityUser.Id, Token = token }, Request.Scheme);
					SendMail(identityUser, confirmationUrlLink, "Email Confirmation Link");
					TempData["Message"] = "A confirm email link has been sent to your registered mail, click on it to confirm.";
					return View("DisplayMessage");
				}
				else
				{
					foreach(var Error in result.Errors)
					{
						//Displaying error details to the user
						ModelState.AddModelError("", Error.Description);
					}
				}
			}
			return View(userModel);
		}
		#endregion
		#region Login & Logout
		public IActionResult Login()
		{
			return View();	
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel loginModel)
		{
			if (ModelState.IsValid)
			{
				var result = await signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password, loginModel.RememberMe, false);
				if(result.Succeeded)
				{
					if (string.IsNullOrEmpty(loginModel.ReturnUrl))
						return RedirectToAction("Index", "Home");
					else
						return LocalRedirect(loginModel.ReturnUrl);
				}
				else
				{
					ModelState.AddModelError("", "Invalid login credentials");
				}
			}
			return View(loginModel);
		}
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}
        #endregion
        #region SendMail Method
		public void SendMail(IdentityUser identityUser,string requestLink,string subject)
		{
			StringBuilder mailBody=new StringBuilder();
			mailBody.Append("Hello" + identityUser.UserName + "<br /><br />");
			if(subject == "Email Confirmation Link")
			{
				mailBody.Append("Click on the link below to confirm your email:");
			}
			else if(subject == "Change Password Link")
			{
				mailBody.Append("Click on the link below to reset your password:");
			}
			mailBody.Append("<br />");
			mailBody.Append(requestLink);
			mailBody.Append("<br /><br />");
			mailBody.Append("Regards");
			mailBody.Append("<br /><br />");
			mailBody.Append("Customer Support");

			BodyBuilder bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = mailBody.ToString();

			MailboxAddress fromAddress = new MailboxAddress("Customer Support", "monikakushwaha2001@gmail.com");
			MailboxAddress toAddress = new MailboxAddress(identityUser.UserName,identityUser.Email);

			MimeMessage mailMessage = new MimeMessage();
			mailMessage.From.Add(fromAddress);
			mailMessage.To.Add(toAddress);
			mailMessage.Subject = subject;
			mailMessage.Body = bodyBuilder.ToMessageBody();

			SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 465, true);
            smtpClient.Authenticate("monikakushwaha2001@gmail.com", "<Generate an App Password and use it here>");
            smtpClient.Send(mailMessage);

        }
        #endregion
    }
}
