using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace EventsUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                MainFrame frame;
                FormLogin logDialog = new FormLogin();
                DialogResult res = logDialog.ShowDialog();
                if (res == DialogResult.OK)
                {
                    UserConfiguration settings = logDialog.Settings;
                    logDialog.Dispose();
                    frame = new MainFrame(settings);
                    Application.Run(frame);
                }
                else
                    logDialog.Dispose();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
