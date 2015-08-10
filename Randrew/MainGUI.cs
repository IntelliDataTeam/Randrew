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
        enum comboIndex
        {
            Import,
            Check,
            Exit
        }
        private bool oFile = false;

        public MainGUI()
        {
            InitializeComponent();
            Bunny.Text = Secrets.bunnyEmotion();
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
                case (int)comboIndex.Import:
                    string filename;
                    switch (filename = Program.getFile())
                    {
                        case "Cancelled":
                            break;
                        case null:
                            statusText.Text = "File did not open successfully.";
                            break;
                        default:
                            oFile = Program.openFile(filename);
                            statusText.Text = "File: (" + filename + ") is loaded";
                            break;
                    }
                    break;

                case (int)comboIndex.Check:
                    if (oFile)
                    {
                        switch (Program.DataChk())
                        {
                            case 0:
                                statusText.Text = "All is good.";
                                break;
                            case 1:
                                statusText.Text = "There are duplicates.";
                                break;
                            case 2:
                                statusText.Text = "There are error values.";
                                break;
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

        private void statusText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
