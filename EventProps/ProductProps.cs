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
    public class ProductProps : IBaseProps
    {
        public int id = Int32.MinValue;
        public string code = "";
        public string description = "";
        public decimal unitPrice = Decimal.MinValue;
        public int onHandQty = Int32.MinValue;
        public int concurrencyID = 0;

        public ProductProps()
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
            ProductProps p = (ProductProps)serializer.Deserialize(reader);
            this.id = p.id;
            this.code = p.code;
            this.unitPrice = p.unitPrice;
            this.onHandQty = p.onHandQty;
            this.description = p.description;
            this.concurrencyID = p.concurrencyID;
        }
        public void SetState(DBDataReader dr)
        {
            this.id = (Int32)dr["ProductID"];
            this.code = ((string)dr["ProductCode"]).Trim();
            this.unitPrice = (decimal)dr["UnitPrice"];
            this.description = (string)dr["Description"];
            this.onHandQty = (Int32)dr["OnHandQuantity"];
            this.concurrencyID = (Int32)dr["ConcurrencyID"];
        }

        public object Clone()
        {
            ProductProps p = new ProductProps();
            p.id = this.id;
            p.code = this.code;
            p.description = this.description;
            p.unitPrice = this.unitPrice;
            p.onHandQty = this.onHandQty;
            p.concurrencyID = this.concurrencyID;
            return p;
        }
    }
}
