using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using EventClasses;
using EventPropsClassses;
using EventDBClasses;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    

    [TestFixture]
    class CustomerTextDBTests
    {
        private string DataSource = "Data Source=GIZMO\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        [Test]
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no id
            Customer p = new Customer(DataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
        }

        
        [Test]
        public void TestRetrieveCustomerFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(1, DataSource);
            Assert.AreEqual(c.ID, 1);
            Assert.AreEqual(c.Name, "Molunguri, A");
            Console.WriteLine(c.ToString());
        }
        
        [Test]
        public void TestSaveCustomerToDataStore()
        {
            Customer c = new Customer(DataSource);            
            c.Name = "Smith, A";
            c.City = "Eugene";
            c.State = "OR";
            c.Save();
            Assert.AreEqual(700, c.ID);
        }
        
        [Test]
        public void TestCustomerUpdate()   
        {
            Customer c = new Customer(1, DataSource);
            c.Name = "Garnick, J";
            c.City = "Eugene";
            c.Save();

            c = new Customer(1, DataSource);
            Assert.AreEqual(c.ID, 1);
            Assert.AreEqual(c.Name, "Garnick, J");
            Assert.AreEqual(c.City, "Eugene");
        }
        
        [Test]
        public void TestCustomerDelete()
        {
            Customer c = new Customer(2, DataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(2, DataSource));
        }

        [Test]
        public void TestCustomerStaticDelete()
        {
            Customer.Delete(2, DataSource);
            Assert.Throws<Exception>(() => new Customer(2, DataSource));
        }

        [Test]
        public void TestCustomerStaticGetList()
        {
            List<Customer> customers = Customer.GetList(DataSource);
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].ID);           
            Assert.AreEqual("AL", customers[0].State);
        }
        
        [Test]
        public void TestCustomerTextDBRetrieve()
        {
            CustomerSQLDB db = new CustomerSQLDB(DataSource);
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(1, props.id);
            Assert.AreEqual("AL", props.state);
            Assert.AreEqual("Birmingham", props.city);
        }

        [Test]
        public void TestCustomerGetList()
        {
            Customer e = new Customer(DataSource);
            List<Customer> customers = (List<Customer>)e.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].ID);
            Assert.AreEqual("Molunguri, A", customers[0].Name);
        }

        // *** I added this
        [Test]
        public void TestCustomerGetTable()
        {
            DataTable customers = Customer.GetTable(DataSource);
            Assert.AreEqual(customers.Rows.Count, 696);
        }

        [Test]
        public void TestCustomerNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer e = new Customer(DataSource);
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestCustomerSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer e = new Customer(DataSource);
            Assert.Throws<Exception>(() => e.Save());
            e.Name = "Josh Garnick";
            Assert.Throws<Exception>(() => e.Save());
            e.City = "Eugene";
            Assert.Throws<Exception>(() => e.Save());
        }
    
        // *** I added this
        [Test]
        public void TestCustomerConcurrencyIssue()
        {
            Customer e1 = new Customer(1, DataSource);
            Customer e2 = new Customer(1, DataSource);

            e1.Name = "Updated this first";
            e1.Save();

            e2.Name = "Updated this second";
            Assert.Throws<Exception>(() => e2.Save());
        }
        [SetUp]
        public void ResetCustomerData()
        {
            CustomerSQLDB db = new CustomerSQLDB(DataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetCustomerData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
    }
}
