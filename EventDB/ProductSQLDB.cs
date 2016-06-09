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
    public class ProductSQLDB : DBBase, IReadDB, IWriteDB
    {
        #region Constructors

        public ProductSQLDB() : base() { }
        public ProductSQLDB(string cnString) : base(cnString) { }
        // *** I added this constructor
        public ProductSQLDB(DBConnection cn) : base(cn) { }

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
            ProductProps props = new ProductProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_ProductSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = (Int32)key;

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
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ProductCode", SqlDbType.Char);
            command.Parameters.Add("@Description", SqlDbType.VarChar);
            command.Parameters.Add("@UnitPrice", SqlDbType.Money);
            command.Parameters.Add("@OnHandQuantity", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters[0].Direction = ParameterDirection.Output;
                                                    //Should not add value for productID because of autonumber, right?
            command.Parameters["@ProductCode"].Value = props.code;
            command.Parameters["@Description"].Value = props.description;
            command.Parameters["@UnitPrice"].Value = props.unitPrice;
            command.Parameters["@OnHandQuantity"].Value = props.onHandQty;
            command.Parameters["@ConcurrencyID"].Value = props.concurrencyID; //This is hardcoded in the stored procedure

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
            ProductProps props = (ProductProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = props.id;
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
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters.Add("@ProductCode", SqlDbType.Char);
            command.Parameters.Add("@Description", SqlDbType.VarChar);
            command.Parameters.Add("@UnitPrice", SqlDbType.Money);
            command.Parameters.Add("@OnHandQuantity", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = props.id;
            command.Parameters["@ProductCode"].Value = props.code;
            command.Parameters["@Description"].Value = props.description;
            command.Parameters["@UnitPrice"].Value = props.unitPrice;
            command.Parameters["@OnHandQuantity"].Value = props.onHandQty;
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
            command.CommandText = "usp_ProductStaticDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ProductID", SqlDbType.Int);
            command.Parameters["@ProductID"].Value = key;

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
            DataTable t = new DataTable("ProductList");
            //return t;
            
            DBDataReader reader = null;
            DataRow row;

            t.Columns.Add("ID", System.Type.GetType("System.Int32"));
            t.Columns.Add("ProductCode", System.Type.GetType("System.String"));
            t.Columns.Add("Description", System.Type.GetType("System.String"));
            t.Columns.Add("UnitPrice", System.Type.GetType("System.Decimal"));
            t.Columns.Add("OnHandQuantity", System.Type.GetType("System.Int32"));
            t.Columns.Add("ConcurrencyID", System.Type.GetType("System.Int32"));

            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        row = t.NewRow();
                        row["ID"] = reader["ProductID"];
                        row["ProductCode"] = reader["ProductCode"];
                        row["Description"] = reader["Description"];
                        row["UnitPrice"] = reader["UnitPrice"];
                        row["OnHandQuantity"] = reader["OnHandQuantity"];
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
            List<ProductProps> list = new List<ProductProps>();
            //return list;
            
            DBDataReader reader = null;
            ProductProps props;

            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new ProductProps();
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
