using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventPropsClasses;
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
    public class EventSQLDB : DBBase, IReadDB, IWriteDB
    {
        #region Constructors

        public EventSQLDB() : base() { }
        public EventSQLDB(string cnString) : base(cnString) { }
        // *** I added this constructor
        public EventSQLDB(DBConnection cn) : base(cn) { }

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
            EventProps props = new EventProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_EventSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@EventID", SqlDbType.Int);
            command.Parameters["@EventID"].Value = (Int32)key;

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
            int rowsAffected = 0;
            EventProps props = (EventProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_EventCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@EventID", SqlDbType.Int);
            command.Parameters.Add("@UserID", SqlDbType.Int);
            command.Parameters.Add("@EventTitle", SqlDbType.NVarChar);
            command.Parameters.Add("@EventDescription", SqlDbType.NVarChar);
            command.Parameters.Add("@EventDate", SqlDbType.Date);
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["@UserID"].Value = props.userID;
            command.Parameters["@EventTitle"].Value = props.title;
            command.Parameters["@EventDescription"].Value = props.description;
            command.Parameters["@EventDate"].Value = props.date;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
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
            EventProps props = (EventProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_EventDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@EventID", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@EventID"].Value = props.ID;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

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
            int rowsAffected = 0;
            EventProps props = (EventProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_EventUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@EventID", SqlDbType.Int);
            command.Parameters.Add("@UserID", SqlDbType.Int);
            command.Parameters.Add("@EventTitle", SqlDbType.NVarChar);
            command.Parameters.Add("@EventDescription", SqlDbType.NVarChar);
            command.Parameters.Add("@EventDate", SqlDbType.Date);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@EventID"].Value = props.ID;
            command.Parameters["@UserID"].Value = props.userID;
            command.Parameters["@EventTitle"].Value = props.title;
            command.Parameters["@EventDescription"].Value = props.description;
            command.Parameters["@EventDate"].Value = props.date;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID++;
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
            command.CommandText = "usp_EventStaticDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@EventID", SqlDbType.Int);
            command.Parameters["@EventID"].Value = key;

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
            DataTable t = new DataTable("EventList");
            DBDataReader reader = null;
            DataRow row;

            t.Columns.Add("ID", System.Type.GetType("System.Int32"));
            t.Columns.Add("UserID", System.Type.GetType("System.Int32"));
            t.Columns.Add("Date", System.Type.GetType("System.DateTime"));
            t.Columns.Add("Title", System.Type.GetType("System.String"));
            t.Columns.Add("Description", System.Type.GetType("System.String"));

            try
            {
                reader = RunProcedure("usp_EventSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        row = t.NewRow();
                        row["ID"] = reader["EventId"];
                        row["UserID"] = reader["UserId"];
                        row["Date"] = reader["EventDate"];
                        row["Title"] = reader["EventTitle"];
                        row["Description"] = reader["EventDescription"];
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
            List<EventProps> list = new List<EventProps>();
            DBDataReader reader = null;
            EventProps props;

            try
            {
                reader = RunProcedure("usp_EventSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new EventProps();
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
