using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EventClasses;

namespace EventsUI
{
    public partial class EventGridForm : Form
    {
        private string connectionString;
        private List<Event> list;
        public EventGridForm(string cnString)
        {
            InitializeComponent();
            connectionString = cnString;
            list = Event.GetList(connectionString);
            grid.DataSource = list;
        }
    }
}
