using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolsCSharp;
using EventPropsClassses;
using CustomerDB = EventDBClasses.CustomerSQLDB;
using System.Data;

namespace EventClasses
{
    public class Customer : BaseBusiness
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
            mRules.RuleBroken("Name", true);
            mRules.RuleBroken("City", true);
            mRules.RuleBroken("State", true);
        }

        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects.
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new CustomerDB();
                mdbWriteable = new CustomerDB();
            }

            else
            {
                mdbReadable = new CustomerDB(this.mConnectionString);
                mdbWriteable = new CustomerDB(this.mConnectionString);
            }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Customer()
            : base()
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
        public Customer(string cnString)
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
        public Customer(int key, string cnString)
            : base(key, cnString)
        {
        }

        public Customer(int key)
            : base(key)
        {
        }

        public Customer(CustomerProps props)
            : base(props)
        {
        }

        public Customer(CustomerProps props, string cnString)
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
                return ((CustomerProps)mProps).id;
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        public string Name
        {
            get
            {
                return ((CustomerProps)mProps).name;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).name))
                {
                    if (value != "")
                    {
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).name = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("You must enter a name.");
                    }
                }
            }
        }

        public string City
        {
            get
            {
                return ((CustomerProps)mProps).city;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).city))
                {

                    if (value != "")
                    {
                        mRules.RuleBroken("City", false);
                        ((CustomerProps)mProps).city = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("You must enter a city.");
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
        public string State
        {
            get
            {
                return ((CustomerProps)mProps).state;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).state))
                {
                    if (value != "")
                    {
                        mRules.RuleBroken("State", false);
                        ((CustomerProps)mProps).state = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("You must enter a state.");
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
        public string Address
        {
            get
            {
                return ((CustomerProps)mProps).address;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).address))
                {
                    if (value != "")
                    {
                        
                        ((CustomerProps)mProps).address = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("You must enter an address.");
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
        public string ZipCode
        {
            get
            {
                return ((CustomerProps)mProps).zipCode;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).zipCode))
                {
                    if (value != "")
                    {

                        ((CustomerProps)mProps).zipCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("You must enter an ZipCode.");
                    }
                }
            }
        }
        #endregion

        #region others
        /// <summary>
        /// Retrieves a list of Customers.
        /// </summary>
        public static List<Customer> GetList(string cnString)
        {
            CustomerDB db = new CustomerDB(cnString);
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();

            // *** methods in the textdb and sqldb classes don't match
            // Ideally, I should go back and fix the IReadDB interface!
            props = (List<CustomerProps>)db.RetrieveAll(props.GetType());
            foreach (CustomerProps prop in props)
            {
                // *** creates the business object from the props objet
                Customer c = new Customer(prop, cnString);
                customers.Add(c);
            }

            return customers;
        }

        public override object GetList()
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();


            props = (List<CustomerProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop, this.mConnectionString);
                customers.Add(c);
            }

            return customers;
        }

        // *** this is new
        public static DataTable GetTable(string cnString)
        {
            CustomerDB db = new CustomerDB(cnString);
            return db.RetrieveTable();
        }

        public static DataTable GetTable()
        {
            CustomerDB db = new CustomerDB();
            return db.RetrieveTable();
        }

        /// <summary>
        /// Deletes the customer identified by the id.
        /// </summary>
        public static void Delete(int id)
        {
            CustomerDB db = new CustomerDB();
            db.Delete(id);
        }

        public static void Delete(int id, string cnString)
        {
            CustomerDB db = new CustomerDB(cnString);
            db.Delete(id);
        }
        #endregion
    }
}
