using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace EventsUI
{
    public class UserConfiguration
    {
        private string userID;
        private string password;
        private string connectionString;
        private string authentication;

        //private string reportDirectory;

        /// <summary>
        /// Constructor for Login (2 parameters)
        /// </summary>
        public UserConfiguration(string uID, string pw)
        {
            userID = uID;
            password = pw;
            connectionString = "";
            authentication = "";
            //reportDirectory = "";

            // get all the config information that you're going to need once and just return the values when
            // the getter properties are called

            try
            {
                AppSettingsReader asr = new AppSettingsReader();

                // use sql or nt authentication
                authentication = (asr.GetValue("AuthMode", authentication.GetType())).ToString();

                // connection string
                if (authentication.ToLower().Equals("nt"))
                    connectionString = (asr.GetValue("dbNTConnectionString", connectionString.GetType())).ToString();
                else
                    connectionString = (asr.GetValue("dbConnectionString", connectionString.GetType())).ToString() +
                    ";user ID=" + userID + ";password=" + password;

                // report directory
                //this.reportDirectory = (asr.GetValue("crystalReportPath", authentication.GetType())).ToString();
            }

            catch (Exception e)
            {
                // log the exception
                throw;
            }
        }


        #region Property Procedures

        /// <summary>
        /// property for UserID
        /// </summary>
        public string UserID
        {
            get { return userID; }
        }

        /// <summary>
        /// property for Password
        /// </summary>
        public string Password
        {
            get { return password; }
        }

        /// <summary>
        /// property for ConnectionString
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
        }

        public string Authentication
        {
            get
            {
                return this.authentication;
            }
        }

        /*public string ReportDirectory
        {
        get
        {
        return this.reportDirectory;
        }
        }

        */
        #endregion
    }
  
}
