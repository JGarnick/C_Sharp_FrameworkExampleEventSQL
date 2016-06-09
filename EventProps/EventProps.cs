using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ToolsCSharp;

using DBDataReader = System.Data.SqlClient.SqlDataReader;

namespace EventPropsClasses
{
    [Serializable()]
    public class EventProps : IBaseProps
    {

        #region instance variables
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public int userID = Int32.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public DateTime date = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public string title = "";

        /// <summary>
        /// 
        /// </summary>
        public string description = "";

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor. This object should only be instantiated by Customer, not used directly.
        /// </summary>
        public EventProps()
        {
        }

        #endregion

        #region BaseProps Members
        /// <summary>
        /// Serializes this props object to XML, and writes the key-value pairs to a string.
        /// </summary>
        /// <returns>String containing key-value pairs</returns>	
        public string GetState()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.GetStringBuilder().ToString();
        }

        // *** I added this method for performance reasons.
        // I don't always want to generate xml in the db class so the 
        // props class can read in from xml
        public void SetState(DBDataReader dr)
        {
            this.ID = (Int32)dr["EventID"];
            this.userID= (Int32)dr["UserID"];
            this.title = (string)dr["EventTitle"];
            this.description = (string)dr["EventDescription"];
            this.date = (DateTime)dr["EventDate"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            EventProps p = (EventProps)serializer.Deserialize(reader);
            this.ID = p.ID;
            this.userID = p.userID;
            this.date = p.date;
            this.title = p.title;
            this.description = p.description;
            this.ConcurrencyID = p.ConcurrencyID;
        }
        #endregion

        #region ICloneable Members
        /// <summary>
        /// Clones this object.
        /// </summary>
        /// <returns>A clone of this object.</returns>
        public Object Clone()
        {
            EventProps p = new EventProps();
            p.ID = this.ID;
            p.userID = this.userID;
            p.date = this.date;
            p.title = this.title;
            p.description = this.description;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }
        #endregion

    }
}
