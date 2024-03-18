using System.ComponentModel.DataAnnotations;
namespace MVCDHProject.Models
{
	public class LoginModel
	{
		[Required]
		[Display(Name="User Name")]
		public string Name {  get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name= "Remember Me")]
		public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; } = "";
    }
}
