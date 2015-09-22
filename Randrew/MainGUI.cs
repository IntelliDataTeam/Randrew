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
        

        public MainGUI()
        {
            InitializeComponent();
            Bunny.Text = Secrets.bunnyEmotion();

            
        }

        private void setDGV(DataTable output)
        {
            dataOutput.AutoGenerateColumns = true;
            dataOutput.DataSource = output;
            List<string> errCoor = Program.GetErrCoor();
            int r = 0;
            int c = 0;
            foreach (string err in errCoor)
            {
                r = Int32.Parse(err.Substring(0,err.IndexOf(',')));
                c = Int32.Parse(err.Substring(err.IndexOf(',')+1));
                dataOutput.Rows[r].Cells[c].Style.BackColor = Color.Tomato;
            }
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
                        Stopwatch stopwatch = new Stopwatch();
                        menuFile.Enabled = false;

                        stopwatch.Start();

                        bool[] errors = Program.DataChk();

                        stopwatch.Stop();

                        string err_output = null;
                        TimeSpan ts = stopwatch.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                        if (errors[0])
                        {
                            err_output += "There is no ProdCat column.";
                            err_output += Environment.NewLine;
                        }
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
                            err_output += "Missing an important column.";
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
                        if (errors[6])
                        {
                            err_output += "There are too many errors!";
                            err_output += Environment.NewLine;
                        }
                        if (errors[7])
                        {
                            err_output += "Failed customized rules.";
                            err_output += Environment.NewLine;
                        }
                        if (!errors.Contains(true))
                        {
                            err_output = "Everything is good.";
                            err_output += Environment.NewLine;
                        }
                        setDGV(Program.GetErr());
                        statusText.Text = err_output + elapsedTime;
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

        private void statusText_TextChanged(object sender, EventArgs e)
        {

        }

    }

}
