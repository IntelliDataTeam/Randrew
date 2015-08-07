using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LumenWorks.Framework.IO.Csv;

namespace Randrew
{
    public partial class MainGUI : Form
    {
        private bool oFile = false;
        public MainGUI()
        {
            InitializeComponent();
        }

        private void setDGV(string[] csv)
        {
            dataOutput.AutoGenerateColumns = true;
            dataOutput.DataSource = csv;
        }

        private void MainGUI_Load(object sender, EventArgs e)
        {

        }

        private void menuFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (menuFile.SelectedIndex)
            {
                case 0:
                    statusText.Text = "";
                    string filename;
                    switch (filename = Program.getFile())
                    {
                        case "Cancelled":
                            break;
                        case null:
                            MessageBox.Show("Error: Can't Open the file.", "Error", MessageBoxButtons.OK);
                            break;
                        default:
                            oFile = Program.openFile(filename);
                            break;
                    }
                    break;

                case 1:
                    if (oFile)
                    {
                        if (Program.chkDup())
                        {
                            statusText.Text = statusText.Text + "There are duplicates! " + Environment.NewLine;
                        }
                        else
                        {
                            statusText.Text = "All is good.";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please import a file first.", "Error", MessageBoxButtons.OK);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
