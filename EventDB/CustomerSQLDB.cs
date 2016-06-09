using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventPropsClassses;
using ToolsCSharp;

using System.Data;
using System.Data.SqlClient;

// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = ToolsCSharp.BaseSQLDB;
using DBConnection = System.Data.SqlClient.SqlConnection;
using DBCommand = System.Data.SqlClient.SqlCommand;
using DBParameter = System.Data.SqlClient.SqlParameter;
using DBDataReader = System.Data.SqlClient.SqlDataReader;
using DBDataAdapter = System.Data.SqlClient.SqlDataAdapter;

namespace EventDBClasses
{
    public class CustomerSQLDB : DBBase, IReadDB, IWriteDB
    {
        #region Constructors

        public CustomerSQLDB() : base() { }
        public CustomerSQLDB(string cnString) : base(cnString) { }
        // *** I added this constructor
        public CustomerSQLDB(DBConnection cn) : base(cn) { }

        #endregion

        // *** I deleted IndexOf and NextID.  
        // I'll use the database to find the record and
        // determine the value of a new EventId

        // *** The body of all of these methods are different
        // They use ADO.NET objects and call methods in the SQL base class
        #region IReadDB Members
        /// <summary>
        /// </summary>
        /// 
        public IBaseProps Retrieve(Object key)
        {
            DBDataReader data = null;
            CustomerProps props = new CustomerProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_CustomerSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters["@CustomerID"].Value = (Int32)key;

            try
            {
                data = RunProcedure(command);
                // *** I added this version of SetState to the props class
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        } //end of Retrieve()
        #endregion

        #region IWriteDB Members
        /// <summary>
        /// </summary>
        public IBaseProps Create(IBaseProps p)
        {
            //return p;
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters.Add("@Name", SqlDbType.VarChar);
            command.Parameters.Add("@Address", SqlDbType.VarChar);
            command.Parameters.Add("@City", SqlDbType.VarChar);
            command.Parameters.Add("@State", SqlDbType.Char);
            command.Parameters.Add("@ZipCode", SqlDbType.Char);
            command.Parameters[0].Direction = ParameterDirection.Output;
            //Should not add value for productID because of autonumber, right?
            command.Parameters["@Name"].Value = props.name;
            command.Parameters["@Address"].Value = props.address;
            command.Parameters["@City"].Value = props.city;
            command.Parameters["@State"].Value = props.state;
            command.Parameters["@ZipCode"].Value = props.zipCode;
            

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.id = (int)command.Parameters[0].Value;
                    props.concurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.ToString());
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        /// <summary>
        /// </summary>
        public bool Delete(IBaseProps p)
        {
            //return true;
            CustomerProps props = (CustomerProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@CustomerID"].Value = props.id;
            command.Parameters["@ConcurrencyID"].Value = props.concurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }

            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        } // end of Delete()

        /// <summary>
        /// </summary>
        public bool Update(IBaseProps p)
        {
            //return true;
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters.Add("@Name", SqlDbType.VarChar);
            command.Parameters.Add("@Address", SqlDbType.VarChar);
            command.Parameters.Add("@City", SqlDbType.VarChar);
            command.Parameters.Add("@State", SqlDbType.Char);
            command.Parameters.Add("@ZipCode", SqlDbType.Char);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@CustomerID"].Value = props.id;
            command.Parameters["@Name"].Value = props.name;
            command.Parameters["@Address"].Value = props.address;
            command.Parameters["@City"].Value = props.city;
            command.Parameters["@State"].Value = props.state;
            command.Parameters["@ZipCode"].Value = props.zipCode;
            command.Parameters["@ConcurrencyID"].Value = props.concurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.concurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        } // end of Update()
        #endregion

        // *** this replaces the version of the delete called from the 
        // static method in the Business Class.  It ignores the concurrency id
        public void Delete(int key)
        {

            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerStaticDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters["@CustomerID"].Value = key;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected != 1)
                {
                    string message = "Record was not deleted. Perhaps the key you specified does not exist.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }


        // *** I added this
        public DataTable RetrieveTable()
        {
            DataTable t = new DataTable("CustomerList");
            //return t;

            DBDataReader reader = null;
            DataRow row;

            t.Columns.Add("ID", System.Type.GetType("System.Int32"));
            t.Columns.Add("Name", System.Type.GetType("System.String"));
            t.Columns.Add("Address", System.Type.GetType("System.String"));
            t.Columns.Add("City", System.Type.GetType("System.String"));
            t.Columns.Add("State", System.Type.GetType("System.String"));
            t.Columns.Add("ZipCode", System.Type.GetType("System.String")); //String or Int?
            t.Columns.Add("ConcurrencyID", System.Type.GetType("System.Int32"));

            try
            {
                reader = RunProcedure("usp_CustomerSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        row = t.NewRow();
                        row["ID"] = reader["CustomerID"];
                        row["Name"] = reader["Name"];
                        row["Address"] = reader["Address"];
                        row["City"] = reader["City"];
                        row["State"] = reader["State"];
                        row["ZipCode"] = reader["ZipCode"];
                        row["ConcurrencyID"] = reader["ConcurrencyID"];
                        t.Rows.Add(row);
                    }
                }
                t.AcceptChanges();
                return t;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }

        public object RetrieveAll(Type type)
        {
            List<CustomerProps> list = new List<CustomerProps>();
            //return list;

            DBDataReader reader = null;
            CustomerProps props;

            try
            {
                reader = RunProcedure("usp_CustomerSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new CustomerProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }
    }
}
