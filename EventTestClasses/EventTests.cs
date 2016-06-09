using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using EventClasses;
using EventPropsClasses;
using EventDBClasses;
using ToolsCSharp;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class EventTests
    {
        //private string folder = "C:\\Courses\\CS234CSharp\\Demos\\FrameworkExampleEvent\\Files\\";
        // *** changed the name AND folder to db connection string
        private string dataSource="Data Source=GIZMO\\SQLEXPRESS;Initial Catalog=Eventcalendar;Integrated Security=True";

        [Test]
        public void TestNewEventConstructor()
        {
            // not in Data Store - no id
            Event e = new Event(dataSource);
            Console.WriteLine(e.ToString());
            Assert.Greater(e.ToString().Length, 1);
        }

        
        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Event e = new Event(1, dataSource);
            Assert.AreEqual(e.ID, 1);
            Assert.AreEqual(e.Title, "First Event");
            Console.WriteLine(e.ToString());
        }
        
        [Test]
        public void TestSaveToDataStore()
        {
            Event e = new Event(dataSource);
            e.UserID = 1;
            e.Title = "Third Event";
            e.Description = "This is the third event in my event list.";
            e.Save();
            Assert.AreEqual(3, e.ID);
        }
        
        [Test]
        public void TestUpdate()
        {
            Event e = new Event(1, dataSource);
            e.UserID = 4;
            e.Title = "Edited Event";
            e.Save();

            e = new Event(1, dataSource);
            Assert.AreEqual(e.ID, 1);
            Assert.AreEqual(e.UserID, 4);
            Assert.AreEqual(e.Title, "Edited Event");
        }
        
        [Test]
        public void TestDelete()
        {
            Event e = new Event(2, dataSource);
            e.Delete();
            e.Save();
            Assert.Throws<Exception>(() => new Event(2, dataSource));
        }

        [Test]
        public void TestStaticDelete()
        {
            Event.Delete(2, dataSource);
            Assert.Throws<Exception>(() => new Event(2, dataSource));
        }

        [Test]
        public void TestStaticGetList()
        {
            List<Event> events = Event.GetList(dataSource);
            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(1, events[0].ID);
            Assert.AreEqual("First Event", events[0].Title);
        }

        // *** I added this
        [Test]
        public void TestGetList()
        {
            Event e = new Event(dataSource);
            List<Event> events = (List<Event>)e.GetList();
            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(1, events[0].ID);
            Assert.AreEqual("First Event", events[0].Title);
        }

        // *** I added this
        [Test]
        public void TestGetTable()
        {
            DataTable events = Event.GetTable(dataSource);
            Assert.AreEqual(events.Rows.Count, 2);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Event e = new Event(dataSource);
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Event e = new Event(dataSource);
            Assert.Throws<Exception>(() => e.Save());
            e.UserID = 1;
            Assert.Throws<Exception>(() => e.Save());
            e.Title = "this is a test";
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestInvalidPropertyUserIDSet()
        {
            Event e = new Event(dataSource);
            Assert.Throws<ArgumentOutOfRangeException>(() => e.UserID = -1);   
        }

        // *** I added this
        [Test]
        public void TestConcurrencyIssue()
        {
            Event e1 = new Event(1, dataSource);
            Event e2 = new Event(1, dataSource);

            e1.Title = "Updated this first";
            e1.Save();

            e2.Title = "Updated this second";
            Assert.Throws<Exception>(() => e2.Save());
        }
        
        #region OtherStuff

        
        /*[SetUp]
        public void WriteListOfProps()
        {
            List<EventProps> events = new List<EventProps>();

            EventProps props = new EventProps();
            props.ID = 1;
            props.userID = 1;
            props.date = DateTime.Now;
            props.title = "First Event";
            props.description = "This is the description of the first event";
            events.Add(props);

            props = new EventProps();
            props.ID = 2;
            props.userID = 1;
            props.date = DateTime.Now;
            props.title = "Second Event";
            props.description = "This is the description of the second event";
            events.Add(props);

            XmlSerializer serializer = new XmlSerializer(events.GetType());
            Stream writer = new FileStream(folder + "EventTextDB.xml", FileMode.Create);
            serializer.Serialize(writer, events);
            writer.Close();
        }*/
        

        // *** I added changed this.  It calls the stored procedure to reset the db
        [SetUp]
        public void TestResetDatabase()
        {
            EventSQLDB db = new EventSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestPropsRetrieve()
        {
            EventSQLDB db = new EventSQLDB(dataSource);
            EventProps props = (EventProps)db.Retrieve(2);
            Assert.AreEqual(props.ID, 2);
            Console.WriteLine(props.GetState());
        }

        #endregion
    }
}
