using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolsCSharp;
using EventPropsClassses;
using ProductDB = EventDBClasses.ProductSQLDB;
using System.Data;

namespace EventClasses
{
    public class Product : BaseBusiness
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
            mRules.RuleBroken("Code", true);
            mRules.RuleBroken("UnitPrice", true);
            mRules.RuleBroken("OnHandQuantity", true);
        }

        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects.
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new ProductDB();
                mdbWriteable = new ProductDB();
            }

            else
            {
                mdbReadable = new ProductDB(this.mConnectionString);
                mdbWriteable = new ProductDB(this.mConnectionString);
            }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Product()
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
        public Product(string cnString)
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
        public Product(int key, string cnString)
            : base(key, cnString)
        {
        }

        public Product(int key)
            : base(key)
        {
        }

        public Product(ProductProps props)
            : base(props)
        {
        }

        public Product(ProductProps props, string cnString)
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
                return ((ProductProps)mProps).id;
            }
        }

        /// <summary>
        /// Read/Write property. 
        /// </summary>
        public int OnHandQuantity
        {
            get
            {
                return ((ProductProps)mProps).onHandQty;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).onHandQty))
                {
                    if (value >= 0)
                    {
                        mRules.RuleBroken("OnHandQuantity", false);
                        ((ProductProps)mProps).onHandQty = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("OnHandQuantity must be a positive number.");
                    }
                }
            }
        }

        public decimal UnitPrice
        {
            get
            {
                return ((ProductProps)mProps).unitPrice;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).unitPrice))
                {
                    
                    if (value >= 0)
                    {
                        mRules.RuleBroken("UnitPrice", false);
                        ((ProductProps)mProps).unitPrice = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("UnitPrice must be a positive number.");
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
        public string Code
        {
            get
            {
                return ((ProductProps)mProps).code;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).code))
                {
                    if (value.Length >= 1 && value.Length <= 4)
                    {
                        mRules.RuleBroken("Code", false);
                        ((ProductProps)mProps).code = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Code must be between 1 and 4 characters");
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
                return ((ProductProps)mProps).description;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).description))
                {
                    if (value.Length >= 1 && value.Length <= 50)
                    {
                        //mRules.RuleBroken("Description", false);
                        ((ProductProps)mProps).description = value;
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
        /*public DateTime Date
        {
            get
            {
                return ((ProductProps)mProps).date;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).date))
                {
                    ((ProductProps)mProps).date = value;
                    mIsDirty = true;
                }
            }
        }*/
        #endregion

        #region others
        /// <summary>
        /// Retrieves a list of Products.
        /// </summary>
        public static List<Product> GetList(string cnString)
        {
            ProductDB db = new ProductDB(cnString);
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();

            // *** methods in the textdb and sqldb classes don't match
            // Ideally, I should go back and fix the IReadDB interface!
            props = (List<ProductProps>)db.RetrieveAll(props.GetType());
            foreach (ProductProps prop in props)
            {
                // *** creates the business object from the props objet
                Product e = new Product(prop, cnString);
                products.Add(e);
            }

            return products;
        }

        public override object GetList()
        {
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();


            props = (List<ProductProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (ProductProps prop in props)
            {
                Product e = new Product(prop, this.mConnectionString);
                products.Add(e);
            }

            return products;
        }

        // *** this is new
        public static DataTable GetTable(string cnString)
        {
            ProductDB db = new ProductDB(cnString);
            return db.RetrieveTable();
        }

        public static DataTable GetTable()
        {
            ProductDB db = new ProductDB();
            return db.RetrieveTable();
        }

        /// <summary>
        /// Deletes the customer identified by the id.
        /// </summary>
        public static void Delete(int id)
        {
            ProductDB db = new ProductDB();
            db.Delete(id);
        }

        public static void Delete(int id, string cnString)
        {
            ProductDB db = new ProductDB(cnString);
            db.Delete(id);
        }
        #endregion
    }
}
