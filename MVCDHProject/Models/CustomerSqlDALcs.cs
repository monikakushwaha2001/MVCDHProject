
namespace MVCDHProject.Models
{
    public class CustomerSqlDALcs : ICustomerDAL
    {
        public readonly MVCCoreDbContext dc;
        public CustomerSqlDALcs(MVCCoreDbContext context)
        {
            this.dc = context;
        }
        public List<Customer> Customers_Select()
        {
            var customers = dc.Customers.Where(C => C.Status == true).ToList();
            return customers;
        }
        public Customer Customer_Select(int Custid)
        {
            Customer customer = dc.Customers.Find(Custid);
            if (customer == null)
            {
                throw new Exception("No customer exist's with given Custid.");
            }
            return customer;
        }
        public void Customer_Insert(Customer customer)
        {
            dc.Customers.Add(customer);
            dc.SaveChanges();
        }
        public void Customer_Update(Customer customer)
        {
            customer.Status = true;
            dc.Update(customer);
            dc.SaveChanges();
        }
        public void Customer_Delete(int Custid)
        {
            Customer customer = dc.Customers.Find(Custid);
            //dc.Remove(customer); //Permanent Deletion
            customer.Status = false; //Changing the status
            dc.SaveChanges();
        }

    }
}
