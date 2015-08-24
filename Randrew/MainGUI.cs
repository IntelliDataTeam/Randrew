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
using System.Diagnostics;

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
        BackgroundWorker Minion;

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
            Minion = new BackgroundWorker();
            Minion.WorkerReportsProgress = true;
            Minion.WorkerSupportsCancellation = true;
            Minion.DoWork += new DoWorkEventHandler(Minion_DoWork);
            Minion.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Minion_RunWorkerCompleted);
            
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
                        Stopwatch stopwatch = new Stopwatch();
                        menuFile.Enabled = false;

                        stopwatch.Start();

                        bool[] errors = Program.DataChk();

                        stopwatch.Stop();

                        string err_output = null;
                        TimeSpan ts = stopwatch.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                        if (errors[1])
                        {
                            err_output += "There are duplicate values.";
                            err_output += Environment.NewLine;
                        }
                        if (errors[2])
                        {
                            err_output += "There are error codes from excel.";
                            err_output += Environment.NewLine;
                        }
                        if (errors[3])
                        {
                            err_output += "Missing crossing referrences.";
                            err_output += Environment.NewLine;
                        }
                        if (errors[4])
                        {
                            err_output += "PN column is missing.";
                            err_output += Environment.NewLine;
                        }
                        if (errors[5])
                        {
                            err_output += "There are new values.";
                            err_output += Environment.NewLine;
                        }
                        if (!errors.Contains(true))
                        {
                            err_output = "Everything is good.";
                            err_output += Environment.NewLine;
                        }

                        statusText.Text = err_output + elapsedTime;
                        /*switch (Program.DataChk())
                        {
                            case 0:
                                statusText.Text = "All is good.";
                                break;
                            case 1:
                                statusText.Text = "There are duplicates.";
                                setDGV(Program.getOutput());
                                break;
                            case 2:
                                statusText.Text = "There are error values.";
                                setDGV(Program.getOutput());
                                break;
                            case 4:
                                statusText.Text = "There is no PN Column.";
                                break;
                            case 5:
                                statusText.Text = "There are new values.";
                                break;
                        }*/
                        menuFile.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Please import a file first.", "Error", MessageBoxButtons.OK);
                    }
                    break;
                case (int)comboIndex.Update:
                    setDGV(Program.getReq());
                    bool submit = false;
                    submit = Program.setCredential(false);
                    /*if (!Minion.IsBusy)
                    {
                        Minion.RunWorkerAsync(Secrets.bunnyEmotion(12));
                        menuFile.Enabled = false;
                        statusText.Text = "Working...";
                    }*/
                    while (!Program.UpdateSource() && submit)
                    {
                        statusText.Text = "Incorrect Username/Password. Try Again.";
                        submit = Program.setCredential(true);
                    }
                    if (submit)
                        statusText.Text = "Successfully connected to the database and Updated the Source Files.";
                    else
                        statusText.Text = "Update Source Files Cancelled.";
                    break;
                case (int)comboIndex.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        public void Waiting()
        {
            Bunny.Text = Secrets.bunnyEmotion(12);
        }

        public void Resuming()
        {
            Bunny.Text = Secrets.bunnyEmotion();
        }

        private void Minion_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker minion = sender as BackgroundWorker;
            //minion.ReportProgress(1);
            Minion.ReportProgress(1);
        }

        private void Minion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                statusText.Text = "Process was cancelled.";
            }
            else if (!(e.Error == null))
            {
                statusText.Text = "Error: " + e.Error.Message;
            }
            else
            {
                //Resuming();
                Waiting();
            }
        }

        private void Minion_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            Waiting();
        }

        private void statusText_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
