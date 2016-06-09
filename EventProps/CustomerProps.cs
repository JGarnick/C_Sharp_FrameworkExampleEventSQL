using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ToolsCSharp;

using DBDataReader = System.Data.SqlClient.SqlDataReader;

namespace EventPropsClassses
{
    [Serializable()]
    public class CustomerProps:IBaseProps
    {
        public int id = Int32.MinValue;
        public string name = "unknown";
        public string address = "unknown";
        public string city = "unknown";
        public string state = "unknown";
        public string zipCode = "unknown";
        public int concurrencyID = 0;

        public CustomerProps()
        {

        }
        
        public string GetState()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.GetStringBuilder().ToString();
        }

        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            CustomerProps c = (CustomerProps)serializer.Deserialize(reader);
            this.id = c.id;
            this.name = c.name;
            this.address = c.address;
            this.city = c.city;
            this.state = c.state;
            this.zipCode = c.zipCode;
            this.concurrencyID = c.concurrencyID;
        }

        public void SetState(DBDataReader dr)
        {
            this.id = (Int32)dr["CustomerID"];
            this.name = (string)dr["Name"];
            this.address = (string)dr["Address"];
            this.city = (string)dr["City"];
            this.state = (string)dr["State"];
            this.zipCode = (string)dr["ZipCode"];
            this.concurrencyID = (Int32)dr["ConcurrencyID"];
        }

        public object Clone()
        {
            CustomerProps c = new CustomerProps();
            c.id = this.id;
            c.name = this.name;
            c.state = this.state;
            c.city = this.city;
            c.address = this.address;
            c.zipCode = this.zipCode;
            c.concurrencyID = this.concurrencyID;
            return c;
        }
    }
}
