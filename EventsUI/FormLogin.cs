using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;



namespace EventsUI
{
    public partial class FormLogin : Form
    {
        private UserConfiguration settings;

        public UserConfiguration Settings
        {
            get
            {
                return settings;
            }

        }

        public FormLogin()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            string userID = userTB.Text.Trim();
            string password = pwTB.Text.Trim();

            try
            {

                settings = new UserConfiguration(userID, password);
                string localCnString = settings.ConnectionString;

                SqlConnection cn = new SqlConnection(localCnString);
                cn.Open();
                cn.Close();
                cn.Dispose();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid user id or password.  Please re-enter.\n" + ex.Message);
                this.DialogResult = DialogResult.None;
                this.userTB.Focus();
            }
        }
    }
}
