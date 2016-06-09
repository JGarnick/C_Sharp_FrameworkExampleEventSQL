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
    public class ProductTests
    {
        private string folder = "C:\\Users\\Christina\\Desktop\\Advanced C#\\Lab 3\\FrameworkExampleEvent\\Files\\";

        [Test]
        public void TestNewProductConstructor()
        {
            // not in Data Store - no id
            Product e = new Product(folder);
            Console.WriteLine(e.ToString());
            Assert.Greater(e.ToString().Length, 1);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product e = new Product(1, folder);
            Assert.AreEqual(e.ID, 1);
            Assert.AreEqual(e.Code, "P100");
            Console.WriteLine(e.ToString());
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Product e = new Product(folder);
            e.OnHandQuantity = 30;
            e.UnitPrice = 300m;
            e.Code = "P300";
            e.Description = "This is the third event in my event list.";
            e.Save();
            Assert.AreEqual(3, e.ID);
            Product e2 = new Product(3, folder);
            Assert.AreEqual(30, e2.OnHandQuantity);
        }

        [Test]
        public void TestUpdate()
        {
            Product e = new Product(1, folder);
            e.Code = "P001";
            e.Description = "Edited Product";
            e.Save();

            e = new Product(1, folder);
            Assert.AreEqual(e.ID, 1);
            Assert.AreEqual(e.Code, "P001");
            Assert.AreEqual(e.Description, "Edited Product");
        }

        [Test]
        public void TestDelete()
        {
            Product e = new Product(2, folder);
            e.Delete();
            e.Save();
            Assert.Throws<Exception>(() => new Product(2, folder));
        }

        [Test]
        public void TestStaticDelete()
        {
            Product.Delete(2, folder);
            Assert.Throws<Exception>(() => new Product(2, folder));
        }

        [Test]
        public void TestGetList()
        {
            List<Product> products = Product.GetList(folder);
            Assert.AreEqual(products.Count, 2);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product e = new Product(folder);
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product e = new Product(folder);
            Assert.Throws<Exception>(() => e.Save());
            e.UnitPrice = 10m;
            Assert.Throws<Exception>(() => e.Save());
            e.OnHandQuantity = 1;
            Assert.Throws<Exception>(() => e.Save());
        }

        /*[Test]
        /*public void TestInvalidPropertyUserIDSet()
        {
            Product e = new Product(folder);
            Assert.Throws<ArgumentOutOfRangeException>(() => e.ID = -1);
        }*/

        #region OtherStuff

        [SetUp]
        public void WriteListOfProps()
        {
            List<ProductProps> products = new List<ProductProps>();

            ProductProps props = new ProductProps();
            props.id = 1;
            props.code = "P100";
            props.onHandQty = 10;
            props.unitPrice = 100m;
            props.description = "This is the description of the first event";
            products.Add(props);

            props = new ProductProps();
            props.id = 2;
            props.code = "P200";
            props.onHandQty = 20;
            props.unitPrice = 200m;
            props.description = "This is the description of the second event";
            products.Add(props);

            XmlSerializer serializer = new XmlSerializer(products.GetType());
            Stream writer = new FileStream(folder + "products.xml", FileMode.Create);
            serializer.Serialize(writer, products);
            writer.Close();
        }
        #endregion
    }
}
