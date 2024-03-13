using MVCDHProject.Models;
using System.Data;
public class CustomerXmlDAL:ICustomerDAL
{
    DataSet ds;
    public CustomerXmlDAL()
    {
        ds = new DataSet();
        ds.ReadXml("Customer.xml");

        //Adding primary key on Custid of DataTable
        ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["Custid"] };
    }
    public List<Customer> Customers_Select()
    {
        List<Customer> customers=new List<Customer>();
        foreach(DataRow dr in ds.Tables[0].Rows)
        {
            Customer obj = new Customer
            {
                Custid = Convert.ToInt32(dr["Custid"]),
                Name = Convert.ToString(dr["Name"]),
                Balance = Convert.ToDecimal(dr["Balance"]),
                City = Convert.ToString(dr["City"]),
                Status = Convert.ToBoolean(dr["Status"])
            };
            customers.Add(obj);
        }
        return customers;   
    }
    public Customer Customer_Select(int Custid)
    {
        //Finding a DataRow based on its Primary Key value
        DataRow dr = ds.Tables[0].Rows.Find(Custid);
        Customer customer = new Customer
        {
            Custid = Convert.ToInt32(dr["Custid"]),
            Name = (string)dr["Name"],
            Balance = Convert.ToDecimal(dr["Balance"]),
            City = (string)dr["City"],
            Status = Convert.ToBoolean(dr["Status"])
        };
        return customer;
    }
    public void Customer_Insert(Customer customer)
    {
        //Creating a new DataRow based on the Data Table Structure
        DataRow dr = ds.Tables[0].NewRow();

        //Assigning values to each Column of the DataRow
        dr["Custid"] = customer.Custid;
        dr["Name"]= customer.Name;
        dr["Balance"] = customer.Balance;
        dr["City"]=customer.City;
        dr["Status"] = customer.Status;

        //Adding the new DataRow to DataTable
        ds.Tables[0].Rows.Add(dr);

        //Saving data back to XML file
        ds.WriteXml("Customer.xml");
    }
    public void Customer_Update(Customer customer)
    {
        //Finding a DataRow based on its primary key value
        DataRow dr = ds.Tables[0].Rows.Find(customer.Custid);

        //Finding the index of DataRow by calling IndexOf method
        int Index = ds.Tables[0].Rows.IndexOf(dr);
        //Overriding the old values in DataRow with new values based on the index
        ds.Tables[0].Rows[Index]["Name"]=customer.Name;
        ds.Tables[0].Rows[Index]["Balance"] = customer.Balance;
        ds.Tables[0].Rows[Index]["City"] = customer.City;

        //Saving data back to xml
        ds.WriteXml("Customer.xml");
    }
    public void Customer_Delete(int Custid)
    {
        //Finding a DataRow based on its Primary key
        DataRow dr = ds.Tables[0].Rows.Find(Custid);

        //Finding the Index of DataRow by callling IndexOf method
        int Index = ds.Tables[0].Rows.IndexOf(dr);

        //Deleting the DataRow from DataTable by using index
        ds.Tables[0].Rows[Index].Delete();

        //Saving data into xml file
        ds.WriteXml("Customer.xml");
    }

}

