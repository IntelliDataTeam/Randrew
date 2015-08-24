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
            PNless,
            NewValues
        }
        private static CsvReader csv;
        private static string[] headers;
        private static List<string> distincts = new List<string>();
        private static DataTable output = new DataTable();
        private static string username;
        private static string password;
        private static List<string[]> d_columns = new List<string[]>();
        private static List<string> d_headers = new List<string>();
        private static List<string[]> uniques = new List<string[]>();
        private static List<string> families = new List<string>();
        private static DataTable req = new DataTable();
        /** </Global Variables> **/

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            parsedFile();
            LoadData("CCA");

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

        /* Is currently working, but still need to cut down on overheads. */
        public static void parsedFile()
        {
            string filename = "\\\\INTELLIDATA-NAS\\IntelliDataNetworkDrive\\z_Quang\\Projects\\Randru\\Configs\\columns_test.txt";
            //DataTable req = new DataTable("Requirements");
            List<string[]> rList = new List<string[]>();
            int rLen = 0;
            Console.WriteLine(filename);
            try
            {
                using (StreamReader sr = new StreamReader(filename)) {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string cHeader = line.Substring(0, line.IndexOf(':'));
                        d_headers.Add(cHeader);
                        req.Columns.Add(cHeader, typeof(string));
                        string[] row = line.Substring(line.IndexOf(':') + 1).Split(',');
                        if (row.Length > rLen)
                            rLen = row.Length;
                        rList.Add(row);
                        d_columns.Add(row);
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
                                rArr[x] = "";
                            }
                        }
                        req.Rows.Add(rArr);
                    }
                }
            }
            catch (Exception e) {
                MessageBox.Show("Error: " + e.Message,"Error",MessageBoxButtons.OK);
                Environment.Exit(0);
            }
        }

        public static DataTable getReq()
        {
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

        public static bool setCredential(bool err)
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

        public static void LoadData(string family) {
            // Current plan is for this to only keep in memory only one family at a time and replace it if a new family appeared.
            string filename = @"\\INTELLIDATA-NAS\IntelliDataNetworkDrive\z_Quang\Projects\Randru\Configs\" + family + ".csv";
            // Can do a File.Exists as part of the data checking process (There shouldn't be any new family).
            using (StreamReader sr = new StreamReader(filename))
            {
                //Console.WriteLine(d_headers[1]);
                int i = d_headers.IndexOf(family);
                string[] arr = new string[i];
                string[] temp = d_columns[i];
                for (int x = 0; x < temp.Length; x++)
                {
                    families.Add(temp[x]);
                }
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    arr = line.Split(',');
                    uniques.Add(arr);
                }
            }
        }

        public static bool UpdateSource()
        {
            //string cs = @"server=localhost;userid=" + username + ";password=" + password + ";database=test";
            string cs = @"server=10.176.3.13;userid=" + username + ";password=" + password + ";database=dev";
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            MySqlCommand cmd = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                for (int x = 0; x < d_headers.Count(); x++)
                {
                    string[] disCol = d_columns[x];
                    object[] oString = new object[disCol.Length];
                    string qString = string.Join(",", disCol);

                    //string stm = "SELECT DISTINCT " + qString + " FROM cdb WHERE Family='" + d_headers[x] + "';";
                    string stm = "SELECT DISTINCT " + qString + " FROM capacitors WHERE prodcat='" + d_headers[x] + "';";
                    Console.WriteLine(stm);
                    cmd = new MySqlCommand(stm, conn);
                    
                    reader = cmd.ExecuteReader();
                    using (StreamWriter w = new StreamWriter(@"\\INTELLIDATA-NAS\IntelliDataNetworkDrive\z_Quang\Projects\Randru\Configs\" + d_headers[x] + ".csv", false))
                    {
                        while (reader.Read())
                        {
                            reader.GetValues(oString);
                            w.WriteLine(string.Join(",", oString));
                        }
                    }
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
            /**** Find how to get the max size of the csv file */

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

                    // !Currently is not able to check for unique value. Need to be fix.
                    for (int x = 0; x < csv.FieldCount; x++)
                    {
                        if (families.Contains(headers[x]))
                        {
                            bool nVal = false;
                            int col = families.IndexOf(headers[x]);
                            for (int y = 0; y < uniques.Count(); y++)
                            {
                                string[] arr = uniques[y];
                                if (csv[x] == arr[col])
                                {
                                    
                                    nVal = true;
                                    break;
                                }
                            }
                            if (!nVal)
                            {
                                Console.Write("New value for " + headers[x] + " for " + csv.CurrentRecordIndex + ": " + csv[pn]);
                                return (int)Err.NewValues;
                            }
                        }
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
