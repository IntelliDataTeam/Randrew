using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LumenWorks.Framework.IO.Csv;
using System.Security.Cryptography;
using System.Threading;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Randrew
{
    public partial class MainGUI : Form
    {
        /** <Global Variables> **/
        #region Global_Variables
        enum Err
        {
            ProdCatLess,
            Duplicates,
            Errors,
            Missings,
            PNless,
            NewValues,
            TooMany,
            Customized
        }
        enum comboIndex
        {
            Import,
            Check,
            Scan,
            Update,
            Customed,
            Reset,
            Exit
        }
        private CsvReader csv;
        private string[] headers;
        private string username;
        private string password;
        private List<string[]> d_columns;
        private List<string> d_families;
        private List<string> log;
        private DataTable req;
        private DataTable errTable;
        private bool[] errors = { false, false, false, false, false, false, false, false };
        private List<List<string>> ruleList;
        private Stopwatch stopwatch;
        private bool oFile = false;
        private bool inProgress;
        private int w_bunny = 0;
        private string fname;
        private BackgroundWorker minion = new BackgroundWorker();
        #endregion
        /** </Global Variables> **/
        
        public MainGUI()
        {
            InitializeComponent();
            minion.DoWork += new DoWorkEventHandler(minion_DoWork);
            minion.ProgressChanged += new ProgressChangedEventHandler(minion_ProgressChanged);
            minion.RunWorkerCompleted += new RunWorkerCompletedEventHandler(minion_RunWorkerCompleted);
            Bunny.Text = Secrets.bunnyEmotion();
       
        }

        /********************* <BackgroundWorker Functions> *********************/
        #region backgroundworker
        void minion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                statusText.Text = e.Error.Message;
            }
            else if (e.Cancelled)
                statusText.Text = "Cancelled.";
            else
            {
                Resuming();
                stopwatch.Stop();
                string err_output = null;
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                if (!errors.Contains(true))
                {
                    err_output = "Everything is good.";
                    err_output += Environment.NewLine;
                }
                else
                {
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
                    writeToLog();
                    setDGV(errTable, (List<string>)e.Result);
                }
                statusText.Text = err_output + elapsedTime;
                menuFile.Enabled = true;
            }
        }

        void minion_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (inProgress)
            {
                return;
            }
            else
            {
                inProgress = true;
                /*if (e.ProgressPercentage < 0)
                {
                    statusText.Text = "Updating " + d_columns[(-1 * e.ProgressPercentage) - 1];
                }
                else
                {*/
                    Waiting();
                    statusText.Text = "Current Progress: " + e.ProgressPercentage + " lines";
                //}
                inProgress = false;
            }
        }

        void minion_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Hello World: " + e.Argument);
            if ((int)e.Argument == 1)
                e.Result = DataChk();
            else if ((int)e.Argument == 2)
                e.Result = DataScn();
            // Future idea: Bring 'Update Source' function into here so that GUI control and data processes will not interfere with each others.
        }
        #endregion
        /********************* </BackgroundWorker Functions> *********************/


        /***************************** <GUI Controls> ****************************/
        #region GUI_Controls
        private void MainGUI_Load(object sender, EventArgs e)
        {
            parsedFile();
            int x = 0;
            foreach (string fam in d_families)
            {
                UpdateList.Items.Add(fam);
                UpdateList.SetItemChecked(x, true);
                x++;
            }
            minion.WorkerReportsProgress = true;
            minion.WorkerSupportsCancellation = true;
            CheckingLabel.Text = "Please Import First.";
            CheckList.Hide();
        }

        private void menuFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (menuFile.SelectedIndex)
            {
                case (int)comboIndex.Import:
                    string filename;
                    switch (filename = getFile("csv"))
                    {
                        case "Cancelled":
                            break;
                        case null:
                            statusText.Text = "File did not open successfully.";
                            break;
                        default:
                            oFile = openFile(filename);
                            fname = filename;
                            statusText.Text = "File: (" + filename + ") is loaded";
                            CheckingLabel.Text = "Customize Checking";
                            CheckList.Show();
                            break;
                    }
                    break;

                case (int)comboIndex.Check:
                    if (oFile)
                    {
                        menuFile.Enabled = false;
                        dataOutput.DataSource = null;

                        stopwatch = new Stopwatch();
                        stopwatch.Start();

                        // currently not responsive for ccs due to large config file.
                        minion.RunWorkerAsync(menuFile.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("Please import a file first.", "Error", MessageBoxButtons.OK);
                    }
                    break;

                case (int)comboIndex.Scan:
                    if (oFile)
                    {
                        bool sub = false;
                        sub = setCredential(false);
                        while (!sub)
                        {
                            statusText.Text = "Incorrect Username/Password. Try Again.";
                            sub = setCredential(true);
                        }
                        if (sub)
                        {
                            statusText.Text = "Successfully connected to the database.";
                            menuFile.Enabled = false;
                            dataOutput.DataSource = null;

                            stopwatch = new Stopwatch();
                            stopwatch.Start();

                            // currently not responsive for ccs due to large config file.
                            minion.RunWorkerAsync(menuFile.SelectedIndex);
                        }
                        else
                            statusText.Text = "Connection Cancelled.";
                    }
                    else
                    {
                        MessageBox.Show("Please import a file first.", "Error", MessageBoxButtons.OK);
                    }
                    break;

                case (int)comboIndex.Update:
                    setDGV(req);
                    bool submit = false;
                    submit = setCredential(false);
                    while (!UpdateSource() && submit)
                    {
                        statusText.Text = "Incorrect Username/Password. Try Again.";
                        submit = setCredential(true);
                    }
                    if (submit)
                        statusText.Text = "Successfully connected to the database and Updated the Source Files.";
                    else
                        statusText.Text = "Update Source Files Cancelled.";
                    break;

                case (int)comboIndex.Customed:
                    string fn = getFile("config");
                    if (fn != "Cancelled" || fn != null)
                    {
                        Properties.Settings.Default.distincts = fn;
                        Properties.Settings.Default.Save();
                        parsedFile();
                        UpdateList.Items.Clear();
                        foreach (string fam in d_families)
                        {
                            UpdateList.Items.Add(fam);
                        }
                    }
                    break;

                case (int)comboIndex.Reset:
                    Properties.Settings.Default.Reset();
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
            if (w_bunny == 2)
            {
                Bunny.Text = Secrets.bunnyEmotion(14);
                w_bunny = 0;
            }
            else
            {
                Bunny.Text = Secrets.bunnyEmotion(12+w_bunny);
                w_bunny++;
            }
        }

        public void Resuming()
        {
            Bunny.Text = Secrets.bunnyEmotion();
        }

        private void statusText_TextChanged(object sender, EventArgs e)
        {

        }

        private void setDGV(DataTable output)
        {
            dataOutput.Rows.Clear();
            dataOutput.Columns.Clear();

            dataOutput.AutoGenerateColumns = true;
            dataOutput.DataSource = output;
            dataOutput.Refresh();
        }

        private void setDGV(DataTable output, List<string> errCoor)
        {
            dataOutput.AutoGenerateColumns = true;
            dataOutput.DataSource = output;
            dataOutput.Refresh();
            int r = 0;
            int c = 0;
            foreach (string err in errCoor)
            {
                r = Int32.Parse(err.Substring(0, err.IndexOf(',')));
                c = Int32.Parse(err.Substring(err.IndexOf(',') + 1));
                dataOutput.Rows[r].Cells[c].Style.BackColor = Color.Tomato;
            }
        }

        private void writeToLog()
        {
            string fi = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\randru_log.txt";
            using (StreamWriter w = new StreamWriter(fi))
            {
                for (int x = 0; x < log.Count(); x++)
                    w.WriteLine(log[x]);
                w.Close();
            }
            Process.Start(fi);
        }

        
        #endregion
        /**************************** </GUI Controls> ****************************/


        /************************* <Internal Functions> *************************/
        #region Internal_Functions
        private void initializeCsv(string filename)
        {
            errTable = new DataTable();
            csv = new CsvReader(new StreamReader(filename), true);
            headers = csv.GetFieldHeaders();

            errTable.Columns.Add("Row Number");
            CheckList.Items.Clear();
            for (int x = 0; x < headers.Length; x++)
            {
                errTable.Columns.Add(headers[x]);
                CheckList.Items.Add(headers[x]);
                CheckList.SetItemChecked(x, true);
            }
        }

        /* Is currently working, but still need to cut down on overheads. */
        private void parsedFile()
        {
            d_columns = new List<string[]>();
            d_families = new List<string>();
            req = new DataTable();
            ruleList = new List<List<string>>();
            string filename = Properties.Settings.Default.distincts;
            List<string[]> rList = new List<string[]>();
            int rLen = 0;
            Console.WriteLine(filename);
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string cHeader = line.Substring(0, line.IndexOf(':'));
                        d_families.Add(cHeader);
                        req.Columns.Add(cHeader, typeof(string));
                        string[] row = line.Substring(line.IndexOf(':') + 1).Split(',');
                        if (row.Length > rLen)
                            rLen = row.Length;

                        List<string> rules = new List<string>();
                        /* To check for special conditions for the column, use a for loop and Contains to find the character '('. */
                        for (int x = 0; x < row.Length; x++)
                        {
                            if (row[x].Contains('('))
                            {
                                string c_name = row[x].Substring(0, row[x].IndexOf('('));
                                rules.Add(c_name + "#" + row[x].Substring(row[x].IndexOf('(') + 1, row[x].IndexOf(')') - row[x].IndexOf('(') - 1));
                                row[x] = c_name;
                            }
                            
                        }
                        ruleList.Add(rules);
                        rList.Add(row);
                        d_columns.Add(row);
                        /* End of testing */
                    }
                    string[] rArr = new string[req.Columns.Count];
                    for (int y = 0; y < rLen; y++)
                    {
                        for (int x = 0; x < req.Columns.Count; x++)
                        {
                            try
                            {
                                string[] tmp = rList[x];
                                rArr[x] = tmp[y];
                            }
                            catch (IndexOutOfRangeException ex) // Currently throw a harmless 'IndexOutOfRangeException' in the console.
                            {
                                Console.WriteLine("Ignore this error: " + ex);
                                rArr[x] = "";
                            }
                        }
                        req.Rows.Add(rArr);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "Error", MessageBoxButtons.OK);
                Properties.Settings.Default.Reset();
                Environment.Exit(0);
            }
        }

        private bool Compare<T>(string op, T x, T y) where T : IComparable
        {
            switch (op)
            {
                case "==": return x.CompareTo(y) == 0;
                case "!=": return x.CompareTo(y) != 0;
                case ">": return x.CompareTo(y) > 0;
                case ">=": return x.CompareTo(y) >= 0;
                case "<": return x.CompareTo(y) < 0;
                case "<=": return x.CompareTo(y) <= 0;
            }
            return false;
        }

        private string getFile(string option)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (option == "csv")
            {
                ofd.Title = "Select CSV File";
                ofd.InitialDirectory = "c:\\Documents";
                ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            }
            else
            {
                ofd.Title = "Select The Config File";
                ofd.InitialDirectory = "c:\\Documents";
                ofd.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
            }
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                return ofd.FileName;
            }
            else if (result == DialogResult.Cancel)
            {
                return "Cancelled";
            }
            else
            {
                return null;
            }
        }

        private bool openFile(string filename)
        {
            try { initializeCsv(filename); }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private bool setCredential(bool err)
        {
            PassForm pf = new PassForm();
            if (err)
                pf.Show_Info();
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK)
            {
                string[] credential = new string[2];
                credential = pf.getCredential();
                username = credential[0];
                password = credential[1];
                return true;
            }
            return false;
        }

        private List<List<string>> LoadData(string family)
        {
            List<List<string>> uniques = new List<List<string>>();
            // Current plan is for this to only keep in memory only one family at a time and replace it if a new family appeared.
            string filename = @"\\INTELLIDATA-NAS\IntelliDataNetworkDrive\z_Quang\Projects\Randru\Configs\" + family + ".csv";
            // Can do a File.Exists as part of the data checking process (There shouldn't be any new family).
            using (StreamReader sr = new StreamReader(filename))
            {
                int i = d_families.IndexOf(family);
                Console.WriteLine(i);
                string[] row = d_columns[i];
                for (int x = 0; x < row.Length; x++)
                {
                    //Console.Write(row[x] + ",");
                    List<string> temp = new List<string>();
                    temp.Add(row[x]);
                    uniques.Add(temp);
                }
                string line;

                // Read from Unique Value File (familyname.csv)
                line = sr.ReadLine();   // Skip column headers since we already have those.
                string[] hTemp;
                hTemp = line.Split(',');

                // Check to see if the config file and the distinct file are out of sync (out of date).
                if (row.SequenceEqual(hTemp))
                {

                    while ((line = sr.ReadLine()) != null)
                    {
                        // Read in the entire line and split it up by ','.
                        string[] arr;
                        // Wrapped all of the output in double quotes already.
                        arr = line.Trim('"').Split(new String[] { "\",\"" }, StringSplitOptions.None);

                        for (int x = 0; x < row.Length; x++)
                        {
                            if (!uniques[x].Contains(arr[x]))
                                uniques[x].Add(arr[x]);
                        }
                    }
                    sr.Close();
                }
                else
                {
                    MessageBox.Show("Error: Config and Distinct File are out of date. Please 'Update Source'.", "Error", MessageBoxButtons.OK);
                    Environment.Exit(0);
                }
            }
            return uniques;
        }

        private bool UpdateSource()
        {
            string cs = @"server=10.176.3.13;userid=" + username + ";password=" + password + ";database=dev";
            string device = "capacitors";
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            MySqlCommand cmd = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                //for (int x = 0; x < d_families.Count(); x++)
                foreach (int indexChecked in UpdateList.CheckedIndices)
                {
                    //string[] disCol = d_columns[x];
                    string[] disCol = d_columns[indexChecked];
                    object[] oString = new object[disCol.Length];
                    string qString = string.Join(",", disCol);

                    // Attempt to allow "smart" querying.
                    switch (UpdateList.Items[indexChecked].ToString().Substring(0, 1))
                    {
                        case "C":
                            device = "capacitors";
                            break;
                        case "I":
                            device = "inductors";
                            break;
                        case "V":
                            device = "varistors";
                            break;
                        case "R":
                            device = "resistors";
                            break;
                        default:
                            device = "capacitors";
                            break;
                    }

                    Console.WriteLine(UpdateList.Items[indexChecked].ToString());

                    string stm = "SELECT DISTINCT " + qString + " FROM " + device + " WHERE prodcat='" + UpdateList.Items[indexChecked].ToString() + "';";
                    Console.WriteLine(stm);
                    using (cmd = new MySqlCommand(stm, conn))
                    {
                        cmd.CommandTimeout = 0;
                        reader = cmd.ExecuteReader();
                    }
                    

                    // Need to change how the output data are going to be structure to reduce processing time.
                    // Could write to temp files that each hold a column and then combined together to create the final file.
                    using (StreamWriter w = new StreamWriter(@"\\INTELLIDATA-NAS\IntelliDataNetworkDrive\z_Quang\Projects\Randru\Configs\" + UpdateList.Items[indexChecked].ToString() + ".csv", false))
                    {
                        w.WriteLine(string.Join(",", disCol));
                        while (reader.Read())
                        {
                            reader.GetValues(oString);
                            for (int n = 0; n < oString.Length; n++)
                            {
                                if (oString[n].GetType() == typeof(DBNull))
                                {
                                    oString[n] = "NULL";
                                }
                                else if (oString[n].ToString() == "")
                                    oString[n] = "NULL";
                                oString[n] = String.Concat("\"", oString[n], "\"");
                            }
                            w.WriteLine(string.Join(",", oString));
                        }
                        w.Close();
                    }


                    Console.WriteLine("Done with " + UpdateList.Items[indexChecked].ToString());
                    reader.Close();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
                if (reader != null)
                    reader.Close();
                if (conn != null)
                    conn.Close();
                return false;
            }
            if (reader != null)
                reader.Close();
            if (conn != null)
                conn.Close();
            return true;
        }

        private List<string> DataScn()
        {
            List<string> errCoor = new List<string>();
            int pn = csv.GetFieldIndex("PN");
            string[] arr = new string[2];
            int r = 0;
            object[] output = new object[2];

            string cs = @"server=10.176.3.13;userid=" + username + ";password=" + password + ";database=dev";
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            MySqlCommand cmd = null;

            try
            {
                conn = new MySqlConnection(cs);

                using (StreamWriter w = new StreamWriter(@"\\INTELLIDATA-NAS\IntelliDataNetworkDrive\z_Quang\Projects\Randru\Configs\output.csv", false))
                {
                    while (csv.ReadNextRecord())
                    {
                        conn.Open();
                        //string stm = "SELECT COUNT(*) FROM capacitors WHERE pn='" + csv[0] + "' OR alias1='" + csv[0] + "' OR alias2='" + csv[0] + "' OR alias3='" + csv[0] + "';";
                        string stm = "SELECT COUNT(*) FROM capacitors WHERE pn='" + csv[0] + "';";
                        using (cmd = new MySqlCommand(stm, conn))
                        {
                            cmd.CommandTimeout = 0;
                            reader = cmd.ExecuteReader();
                        }
                        while (reader.Read()){
                            reader.GetValues(output);
                            Console.WriteLine("Output = " + output[0]);
                        }
                        if (String.Compare(output[0].ToString(),"0") != 0)
                        {
                            output[1] = "TRUE";
                        }
                        else
                        {
                            output[1] = "FALSE";
                        }
                        Console.WriteLine(output[1].ToString());

                        output[0] = csv[0];
                        w.WriteLine(string.Join(",", output));
                        
                        conn.Close();
                        reader.Close();
                    }
                    w.Close();
                }
                
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
                if (reader != null)
                    reader.Close();
                if (conn != null)
                    conn.Close();
            }

          

            return errCoor;
        }

        // Run checks while reading the csv file once.
        private List<string> DataChk()
        {
            for (int x = 0; x < errors.Length; x++)
                errors[x] = false;
            log = new List<string>();
            
            List<string> errCoor = new List<string>();
            List<List<string>> uniques = new List<List<string>>();
            int pn = csv.GetFieldIndex("PN");
            int prodcat = csv.GetFieldIndex("ProdCat");
            int r = 0;
            int famRules = 0;
            int oy, oz;
            string[] arr = new string[headers.Length];
            string[] fArr = new string[headers.Length + 1];
            //int[] r_headers = new int[rules.Count()];
            bool add = false;
            List<string> rules = new List<string>();

            if (pn == -1)
            {
                errors[(int)Err.PNless] = true;
                log.Add("No 'PN' Column.");
            }

            if (prodcat == -1)
            {
                errors[(int)Err.ProdCatLess] = true;
                log.Add("No 'Prodcat' Column.");
                return errCoor;
            }

            List<HashSet<string>> hashList = new List<HashSet<string>>();
            HashSet<string> hashItem = new HashSet<string>();
            hashList.Add(hashItem);
            /**** Find how to get the max size of the csv file */
            if (csv.CurrentRecordIndex > 0)     // Reset CSVReader csv so that we can use it again.
                initializeCsv(fname);
            while (csv.ReadNextRecord())
            {
                if (errTable.Rows.Count > 50000)
                {
                    Array.Clear(errors, 0, errors.Length);
                    errors[(int)Err.TooMany] = true;
                    log.Add("More than 50000 errors.");
                    return errCoor;
                }
                
                // Load the relevant family rules;
                if (csv.CurrentRecordIndex == 0)
                {
                    famRules = d_families.IndexOf(csv[prodcat].ToUpper());
                    // Got to find out how to send custom error messages.
                    try
                    {
                        rules = ruleList[famRules];
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        throw new ArgumentOutOfRangeException("ERROR: " + csv[prodcat].ToUpper() + " does not have distinct file.");
                    }
                    uniques = LoadData(csv[prodcat].ToUpper());
                    for (int x = 0; x < rules.Count(); x++)
                        Console.WriteLine(rules[x]);
                }

                add = false;

                try
                {
                    // Check for duplicates in column "PN"
                    if (hashList.TrueForAll(hSet => !hSet.Contains(csv[pn])))
                    {
                        hashItem.Add(csv[pn]);
                    }
                    else
                    {
                        errors[(int)Err.Duplicates] = true;
                        add = true;
                        errCoor.Add(r + "," + (pn + 1));
                        log.Add("There are duplicates PN in row " + (csv.CurrentRecordIndex+1) + ".");
                    }


                }
                // If hashset is out of memory, store it in the list and create a new one
                catch (OutOfMemoryException ex)
                {
                    hashItem = new HashSet<string>() { csv[0] };
                    hashList.Add(hashItem);
                }
                
                // Check for '#' in the row
                for (int x = 0; x < csv.FieldCount; x++)
                {
                    if (csv[x].Contains('#'))
                    {
                        errors[(int)Err.Errors] = true;
                        add = true;
                        errCoor.Add(r + "," + (x + 1));
                        log.Add("There is an excel error code in row " + (csv.CurrentRecordIndex+1) + " and column '" + headers[x] + "'.");
                    }
                }

                // Check for new values
                for (int x = 0; x < csv.FieldCount; x++)
                {
                    for (int y = 0; y < uniques.Count(); y++)
                    {
                        // NEWIDEA: implemented optional skipping of checking certain column.
                        if (uniques[y].First() == headers[x] && CheckList.GetItemCheckState(CheckList.FindStringExact(headers[x])) == CheckState.Checked)
                        {

                            if (!uniques[y].Contains(csv[x]))
                            {
                                if (csv[x] == "" && uniques[y].Contains("NULL"))
                                {
                                    break;
                                }
                                //Console.WriteLine(uniques[y].First() + ": " + csv[x]);
                                errors[(int)Err.NewValues] = true;
                                add = true;
                                errCoor.Add(r + "," + (x + 1));
                                log.Add("There is a new value in row " + (csv.CurrentRecordIndex+1) + " and column '" + headers[x] + "'.");
                            }
                            break;
                        }
                    }
                }

                // Custom rules
                // Need a way to find the family headers without having to run everything again
                // Need to know which csv column are being compare with condition
                if (ruleList[famRules].Count > 0)
                {
                    
                    
                    for (int x = 0; x < rules.Count(); x++)
                    {
                        string y = rules[x].Substring(rules[x].IndexOf(' ')+1);
                        string op = rules[x].Substring(rules[x].IndexOf('#') + 1, rules[x].IndexOf(' ') - rules[x].IndexOf('#') - 1);
                        int h = csv.GetFieldIndex(rules[x].Substring(0, rules[x].IndexOf('#')));
                        string z = csv[h];

                        if (int.TryParse(y, out oy) && int.TryParse(z, out oz))
                        {
                            if (!Compare(op, oz, oy))
                            {
                                errors[(int)Err.Customized] = true;
                                errCoor.Add(r + "," + (h + 1));
                                add = true;
                            }
                        }
                        else
                        {
                            if (!Compare(op, z, y))
                            {
                                errors[(int)Err.Customized] = true;
                                errCoor.Add(r + "," + (h + 1));
                                log.Add("There is violator of custom rule in row " + (csv.CurrentRecordIndex+1) + " and column '" + headers[h] + "'.");
                                add = true;
                            }
                        }
                    }
                }

                if (add)
                {
                    csv.CopyCurrentRecordTo(arr);
                    fArr[0] = (csv.CurrentRecordIndex + 1).ToString();
                    for (int j = 0; j < arr.Length; j++)
                    {
                        fArr[j + 1] = arr[j];
                    }
                    errTable.Rows.Add(fArr);
                    r++;
                }
                if (((int)csv.CurrentRecordIndex % 100) == 0)
                {
                    minion.ReportProgress((int)csv.CurrentRecordIndex);
                }
            }
            return errCoor;
        }
        #endregion

        /************************ </Internal Functions> *************************/

        

    }

}
