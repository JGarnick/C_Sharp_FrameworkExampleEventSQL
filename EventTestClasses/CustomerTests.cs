using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;



using EventPropsClassses;
using EventClasses;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerTests
    {
        private string folder = "C:\\Users\\Christina\\Desktop\\Advanced C#\\Lab 3\\FrameworkExampleEvent\\Files\\";

        [Test]
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no id
            Customer e = new Customer(folder);
            Console.WriteLine(e.ToString());
            Assert.Greater(e.ToString().Length, 1);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer e = new Customer(1, folder);
            Assert.AreEqual(e.ID, 1);
            Assert.AreEqual(e.Name, "John Smith");
            Console.WriteLine(e.ToString());
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer e = new Customer(folder);
            e.City = "Vancouver";
            e.State = "Washington";
            e.Name = "Ricky Bobby";
            e.Address = "8888 Fast Dr.";
            e.ZipCode = "12345";
            e.Save();
            Assert.AreEqual(3, e.ID);
            Customer e2 = new Customer(3, folder);
            Assert.AreEqual("Ricky Bobby", e2.Name);
        }

        [Test]
        public void TestUpdate()
        {
            Customer e = new Customer(1, folder);
            e.Name = "Ricky Bobby";
            e.City = "Vancouver";
            e.Save();

            e = new Customer(1, folder);
            Assert.AreEqual(e.ID, 1);
            Assert.AreEqual(e.Name, "Ricky Bobby");
            Assert.AreEqual(e.City, "Vancouver");
        }

        [Test]
        public void TestDelete()
        {
            Customer e = new Customer(2, folder);
            e.Delete();
            e.Save();
            Assert.Throws<Exception>(() => new Customer(2, folder));
        }

        [Test]
        public void TestStaticDelete()
        {
            Customer.Delete(2, folder);
            Assert.Throws<Exception>(() => new Customer(2, folder));
            
        }

        [Test]
        public void TestGetList()
        {
            List<Customer> customers = Customer.GetList(folder);
            Assert.AreEqual(customers.Count, 2);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer e = new Customer(folder);
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer e = new Customer(folder);
            Assert.Throws<Exception>(() => e.Save());
            e.Name = "Rick Astley";
            Assert.Throws<Exception>(() => e.Save());
            e.City = "Medford";
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestDidDataChange()
        {
            // not in Data Store - userid, title and description must be provided
            Customer e = new Customer(1, folder);

            e.Name = "John Smith";

            Assert.IsFalse(e.IsDirty);

            e.Name = "Rick Astley";

            Assert.IsTrue(e.IsDirty);            
        }

        /*[Test]
        /*public void TestInvalidPropertyUserIDSet()
        {
            Customer e = new Customer(folder);
            Assert.Throws<ArgumentOutOfRangeException>(() => e.ID = -1);
        }*/

        #region OtherStuff

        [SetUp]
        public void WriteListOfProps()
        {
            List<CustomerProps> customers = new List<CustomerProps>();

            CustomerProps props = new CustomerProps();
            props.id = 1;
            props.name = "John Smith";
            props.city = "Eugene";
            props.zipCode = "97402";
            props.state = "Oregon";
            props.address = "1234 Test Street";
            customers.Add(props);

            props = new CustomerProps();
            props.id = 2;
            props.name = "Aaron Beckett";
            props.city = "Tuscon";
            props.zipCode = "97403";
            props.state = "Arizona";
            props.address = "4321 Street Test";
            customers.Add(props);

            XmlSerializer serializer = new XmlSerializer(customers.GetType());
            Stream writer = new FileStream(folder + "customers.xml", FileMode.Create);
            serializer.Serialize(writer, customers);
            writer.Close();
        }
        #endregion
    }
}
