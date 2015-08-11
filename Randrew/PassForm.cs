using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Randrew
{
    public partial class PassForm : Form
    {
        private string user;
        private string pass;
        public PassForm()
        {
            InitializeComponent();
        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private void submit_Click(object sender, EventArgs e)
        {
            user = username.Text;
            pass = password.Text;
        }

        public string[] getCredential()
        {
            string[] credential = new string[2];
            credential[0] = user;
            credential[1] = pass;
            return credential;
        }
    }
}
