using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolsCSharp;
using EventPropsClasses;

// *** I had to change this
using EventDB = EventDBClasses.EventSQLDB;

// *** I added this
using System.Data;


namespace EventClasses
{
    public class Event : BaseBusiness
    {
        #region SetUpStuff
        /// <summary>
        /// 
        /// </summary>		
        protected override void SetDefaultProperties()
        {
        }

        /// <summary>
        /// Sets required fields for a record.
        /// </summary>
        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("UserID", true);
            mRules.RuleBroken("Title", true);
            mRules.RuleBroken("Description", true);
        }

        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects.
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new EventProps();
            mOldProps = new EventProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new EventDB();
                mdbWriteable = new EventDB();
            }

            else
            {
                mdbReadable = new EventDB(this.mConnectionString);
                mdbWriteable = new EventDB(this.mConnectionString);
            }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Event() : base()
        {
        }

        /// <summary>
        /// One arg constructor.
        /// Calls methods SetUp(), SetRequiredRules(), 
        /// SetDefaultProperties() and BaseBusiness one arg constructor.
        /// </summary>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Event(string cnString)
            : base(cnString)
        {
        }

        /// <summary>
        /// Two arg constructor.
        /// Calls methods SetUp() and Load().
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Event(int key, string cnString)
            : base(key, cnString)
        {
        }

        public Event(int key)
            : base(key)
        {
        }

        // *** I added these 2 so that I could create a 
        // business object from a properties object
        // I added the new constructors to the base class
        public Event(EventProps props)
            : base(props)
        {
        }

        public Event(EventProps props, string cnString)
            : base(props, cnString)
        {
        }
        #endregion

        #region properties
        /// <summary>
        /// Read-only ID property.
        /// </summary>
        public int ID
        {
            get
            {
                return ((EventProps)mProps).ID;
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        public int UserID
        {
            get
            {
                return ((EventProps)mProps).userID;
            }

            set
            {
                if (!(value == ((EventProps)mProps).userID))
                {
                    if (value > 0)
                    {
                        mRules.RuleBroken("UserID", false);
                        ((EventProps)mProps).userID = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("UserID must be a positive number.");
                    }
                }
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 
        /// </exception>
        public string Title
        {
            get
            {
                return ((EventProps)mProps).title;
            }

            set
            {
                if (!(value == ((EventProps)mProps).title))
                {
                    if (value.Length >= 1 && value.Length <= 50)
                    {
                        mRules.RuleBroken("Title", false);
                        ((EventProps)mProps).title = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Title must be between 1 and 50 characters");
                    }
                }
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 
        /// </exception>
        public string Description
        {
            get
            {
                return ((EventProps)mProps).description;
            }

            set
            {
                if (!(value == ((EventProps)mProps).description))
                {
                    if (value.Length >= 1 && value.Length <= 2000)
                    {
                        mRules.RuleBroken("Description", false);
                        ((EventProps)mProps).description = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Description must be between 1 and 2000 characters");
                    }
                }
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if the value is null or less than 1.
        /// </exception>
        public DateTime Date
        {
            get
            {
                return ((EventProps)mProps).date;
            }

            set
            {
                if (!(value == ((EventProps)mProps).date))
                {
                    ((EventProps)mProps).date = value;
                    mIsDirty = true;
                }
            }
        }
        #endregion

        #region others
        /// <summary>
        /// Retrieves a list of Events.
        /// </summary>
        /// 
        // *** I had to change this
        public static List<Event> GetList(string cnString)
        {
            EventDB db = new EventDB(cnString);
            List<Event> events = new List<Event>();
            List<EventProps> props = new List<EventProps>();

            // *** methods in the textdb and sqldb classes don't match
            // Ideally, I should go back and fix the IReadDB interface!
            props = (List<EventProps>)db.RetrieveAll(props.GetType());
            foreach (EventProps prop in props)
            {
                // *** creates the business object from the props objet
                Event e = new Event(prop, cnString);
                events.Add(e);
            }

            return events;
        }

        // *** this o
        public override object GetList()
        {
            List<Event> events = new List<Event>();
            List<EventProps> props = new List<EventProps>();


            props = (List<EventProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (EventProps prop in props)
            {
                Event e = new Event(prop, this.mConnectionString);
                events.Add(e);
            }

            return events;
        }

        // *** this is new
        public static DataTable GetTable(string cnString)
        {
            EventDB db = new EventDB(cnString);
            return db.RetrieveTable();
        }

        public static DataTable GetTable()
        {
            EventDB db = new EventDB();
            return db.RetrieveTable();
        }

        /// <summary>
        /// Deletes the customer identified by the id.
        /// </summary>
        public static void Delete(int id)
        {
            EventDB db = new EventDB();
            db.Delete(id);
        }

        public static void Delete(int id, string cnString)
        {
            EventDB db = new EventDB(cnString);
            db.Delete(id);
        }
        #endregion
    }
}
