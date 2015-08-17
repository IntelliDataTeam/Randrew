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
using System.Security.Cryptography;
using System.Threading;

namespace Randrew
{
    public partial class MainGUI : Form
    {
        enum comboIndex
        {
            Import,
            Check,
            Update,
            Exit
        }
        private bool oFile = false;
        //BackgroundWorker Minion = new BackgroundWorker();

        public MainGUI()
        {
            InitializeComponent();
            Bunny.Text = Secrets.bunnyEmotion();
        }
        
        private void setDGV(DataTable output)
        {
            dataOutput.AutoGenerateColumns = true;
            dataOutput.DataSource = output;

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
                        /*Minion.DoWork += new DoWorkEventHandler(Minion_DoWork);
                        Minion.ProgressChanged += new ProgressChangedEventHandler(Minion_ProgressChanged);
                        Minion.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Minion_RunWorkerCompleted);
                        Minion.WorkerReportsProgress = true;
                        Minion.RunWorkerAsync();*/

                        menuFile.Enabled = false;
                        switch (Program.DataChk())
                        {
                            case 0:
                                statusText.Text = "All is good.";
                                menuFile.Enabled = true;
                                break;
                            case 1:
                                statusText.Text = "There are duplicates.";
                                setDGV(Program.getOutput());
                                menuFile.Enabled = true;
                                break;
                            case 2:
                                statusText.Text = "There are error values.";
                                setDGV(Program.getOutput());
                                menuFile.Enabled = true;
                                break;
                            case 4:
                                statusText.Text = "There is no PN Column.";
                                menuFile.Enabled = true;
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please import a file first.", "Error", MessageBoxButtons.OK);
                    }
                    break;
                case (int)comboIndex.Update:
                    /*while (!Program.checkCredential())
                    {
                        statusText.Text = "Wrong Username/Password combination. Please try again.";
                        Program.setCredential();
                    }
                    statusText.Text = "Successfully connected to the database.";*/
                    setDGV(Program.parsedFile());
                    break;
                default:
                    break;
            }
        }

        private void Minion_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(100);
        }

        private void Minion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Random random = new Random();
            int ranGen = random.Next(0, 3);
            Bunny.Text = Secrets.bunnyEmotion(ranGen + 12);
        }

        private void Minion_ProgressChanged(object sender, ProgressChangedEventArgs e) {

        }

        private void statusText_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
