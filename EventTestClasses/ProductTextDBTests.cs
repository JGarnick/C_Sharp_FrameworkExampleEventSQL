using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;


using EventPropsClassses;
using EventDBClasses;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;
using EventClasses;

namespace EventTestClasses
{
    

    [TestFixture]
    class ProductTextDBTests
    {
        //private string folder = "C:\\Users\\Christina\\Desktop\\Advanced C#\\Lab 3\\FrameworkExampleEvent\\Files\\";
        private string DataSource = "Data Source=GIZMO\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";
         [Test]
        public void TestNewProductConstructor()
        {
            // not in Data Store - no id
            Product p = new Product(DataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
        }

        
        [Test]
        public void TestProductRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product p = new Product(1, DataSource);
            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.Code, "A4CS");
            Console.WriteLine(p.ToString());
        }
        
        [Test]
        public void TestProductSaveToDataStore()
        {
            Product p = new Product(DataSource);            
            p.Code = "3333";
            p.UnitPrice = 11.11m;
            p.OnHandQuantity = 1000000;
            p.Save();
            Assert.AreEqual(17, p.ID);
        }
        
        [Test]
        public void TestProductUpdate()   // I don't understand the output message
        {
            Product p = new Product(1, DataSource);
            p.Description = "This is the new description";
            p.Code = "1234";
            p.Save();

            p = new Product(1, DataSource);
            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.Description, "This is the new description");
            Assert.AreEqual(p.Code, "1234");
        }
        
        [Test]
        public void TestProductDelete()
        {
            Product p = new Product(2, DataSource);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(2, DataSource));
        }

        [Test]
        public void TestProductStaticDelete()
        {
            Product.Delete(2, DataSource);
            Assert.Throws<Exception>(() => new Product(2, DataSource));
        }

        [Test]
        public void TestProductStaticGetList()
        {
            List<Product> products = Product.GetList(DataSource);
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(1, products[0].ID);           
            Assert.AreEqual("A4CS", products[0].Code);
        }
        
        [Test]
        public void TestProductTextDBRetrieve()
        {
            ProductSQLDB db = new ProductSQLDB(DataSource);
            ProductProps props = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(1, props.id);
            Assert.AreEqual("A4CS", props.code);
            Assert.AreEqual(4637, props.onHandQty);
        }
        [Test]
        public void TestProductGetList()
        {
            Product e = new Product(DataSource);
            List<Product> products = (List<Product>)e.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(1, products[0].ID);
            Assert.AreEqual("A4CS", products[0].Code);
        }

        // *** I added this
        [Test]
        public void TestProductGetTable()
        {
            DataTable products = Product.GetTable(DataSource);
            Assert.AreEqual(products.Rows.Count, 16);
        }

        [Test]
        public void TestProductNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product e = new Product(DataSource);
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestProductSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product e = new Product(DataSource);
            Assert.Throws<Exception>(() => e.Save());
            e.Code = "1234";
            Assert.Throws<Exception>(() => e.Save());
            e.OnHandQuantity = 50;
            Assert.Throws<Exception>(() => e.Save());
        }
        

        // *** I added this
        [Test]
        public void TestProductConcurrencyIssue()
        {
            Product e1 = new Product(1, DataSource);
            Product e2 = new Product(1, DataSource);

            e1.Description = "Updated this first";
            e1.Save();

            e2.Description = "Updated this second";
            Assert.Throws<Exception>(() => e2.Save());
        }
        
        [SetUp]
        public void ResetProductData()
        {
            ProductSQLDB db = new ProductSQLDB(DataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
    }
}
