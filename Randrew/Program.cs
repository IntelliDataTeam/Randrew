using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Data;
using MySql.Data.MySqlClient;

namespace Randrew
{
    static class Program
    {
        /** <Global Variables> **/
        enum Err
        {
            Good,
            Duplicates,
            Errors,
            Missings,
            PNless
        }
        private static CsvReader csv;
        private static string[] headers;
        private static List<string> distincts = new List<string>();
        private static DataTable output = new DataTable();
        private static string username;
        private static string password;
        /** </Global Variables> **/

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainGUI());
        }

        private static void initializeCsv(string filename)
        {
            csv = new CsvReader(new StreamReader(filename), true);
            headers = csv.GetFieldHeaders();
            for (int x=0;x<headers.Length;x++) {
                if (headers[x] != "PN")
                    distincts.Add(headers[x]);
            }
              
        }

        public static DataTable parsedFile()
        {
            string filename = "\\\\INTELLIDATA-NAS\\IntelliDataNetworkDrive\\z_Quang\\Projects\\Randrew\\Configs\\columns.txt";
            DataTable req = new DataTable("Requirements");
            List<string[]> rList = new List<string[]>();
            int rLen = 0;
            Console.WriteLine(filename);
            try
            {
                using (StreamReader sr = new StreamReader(filename)) {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        /*req.Columns.Add(line.Substring(0, line.IndexOf(':')));
                        string[] row = line.Substring(line.IndexOf(':') + 1).Split(',');
                        for (int x = 0; x < rows.Length; x++)
                            req.Rows.Add(rows[x]);*/
                        string cHeader = line.Substring(0, line.IndexOf(':')); 
                        req.Columns.Add(cHeader, typeof(string));
                        string[] row = line.Substring(line.IndexOf(':') + 1).Split(',');
                        if (row.Length > rLen)
                            rLen = row.Length;
                        rList.Add(row);
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
                            catch (IndexOutOfRangeException ex)
                            {
                                rArr[x] = "";
                            }
                        }
                        req.Rows.Add(rArr);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
            }
            return req;
        }

        public static string getFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select CSV File";
            ofd.InitialDirectory = "c:\\Documents";
            ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
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

        public static bool openFile(string filename)
        {
            try { initializeCsv(filename); }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private static void setOutput(string[] rec)
        {
            for (int x = 0; x < headers.Length; x++)
            {
                output.Columns.Add(headers[x],typeof(string));
            }
            output.Rows.Add(rec);
        }

        public static DataTable getOutput()
        {
            return output;
        }

        public static void setCredential()
        {
            PassForm pf = new PassForm();
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK)
            {
                string[] credential = new string[2];
                credential = pf.getCredential();
                username = credential[0];
                password = credential[1];
            }
        }

        public static bool checkCredential()
        {
            if (username == null || password == null || !updateSource())
                return false;
            return true;
        }

        public static bool updateSource()
        {
            string cs = @"server=10.176.3.13;userid=" + username + ";password=" + password + ";database=dev";
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(cs);
                /*MySqlCommand command = conn.CreateCommand();
                MySqlDataReader reader;
                command.CommandText = "SELECT DISTINCT "*/
                conn.Open();
            }
            catch (MySqlException ex)
            {
                return false;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return true;
        }

        // Run checks while reading the csv file once.
        public static int DataChk()
        {
            int pn = csv.GetFieldIndex("PN");
            if (pn == -1)
            {
                return (int)Err.PNless;
            }
            List<HashSet<string>> hashList = new List<HashSet<string>>();
            HashSet<string> hashItem = new HashSet<string>();
            string[] temp = new string[csv.FieldCount];
            hashList.Add(hashItem);
            while (csv.ReadNextRecord())
            {
                try
                {
                    // Check for duplicates in column "PN"
                    if (hashList.TrueForAll(hSet => !hSet.Contains(csv[pn])))
                    {
                        hashItem.Add(csv[pn]);
                    }
                    else
                    {
                        Console.Write(csv.CurrentRecordIndex + ": " + csv[pn]);
                        Console.WriteLine();
                        csv.CopyCurrentRecordTo(temp);
                        setOutput(temp);
                        return (int)Err.Duplicates;
                    }
                    csv.CopyCurrentRecordTo(temp);
                    // Check for '#' in the row
                    if (string.Join(",",temp).Contains('#'))
                    {
                        Console.Write(csv.CurrentRecordIndex + ": " + csv[pn]);
                        Console.WriteLine();
                        return (int)Err.Errors;
                    }
                }
                // If hashset is out of memory, store it in the list and create a new one
                catch (OutOfMemoryException ex)
                {
                    hashItem = new HashSet<string>() { csv[0] };
                    hashList.Add(hashItem);
                }

            }

            return (int)Err.Good;
        }
    }
}
