using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            ProdCatLess,
            Duplicates,
            Errors,
            Missings,
            PNless,
            NewValues,
            TooMany,
            Customized
        }
        private static CsvReader csv;
        private static string[] headers;
        private static List<string> distincts = new List<string>();
        private static string username;
        private static string password;
        private static List<string[]> d_columns = new List<string[]>();
        private static List<string> d_families = new List<string>();
        private static List<List<string>> uniques = new List<List<string>>();
        private static DataTable req = new DataTable();
        private static DataTable errTable = new DataTable();
        private static bool[] errors = { false, false, false, false, false, false, false, false };
        private static List<string> errCoor = new List<string>();

        //private static List<string> rules = new List<string>();
        private static List<List<string>> ruleList = new List<List<string>>();
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

            Application.Run(new MainGUI());
        }

        private static void initializeCsv(string filename)
        {
            csv = new CsvReader(new StreamReader(filename), true);
            headers = csv.GetFieldHeaders();
            for (int x = 0; x < headers.Length; x++)
            {
                if (headers[x] != "PN")
                    distincts.Add(headers[x]);
            }
            errTable.Columns.Add("Row Number");
            for (int x = 0; x < headers.Length; x++)
                errTable.Columns.Add(headers[x]);
        }

        /* Is currently working, but still need to cut down on overheads. */
        public static void parsedFile()
        {
            string filename = "\\\\INTELLIDATA-NAS\\IntelliDataNetworkDrive\\z_Quang\\Projects\\Randru\\Configs\\columns.txt";
            //DataTable req = new DataTable("Requirements");
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


                        /* To check for special conditions for the column, use a for loop and Contains to find the character '('. */
                        for (int x = 0; x < row.Length; x++)
                        {
                            List<string> rules = new List<string>();
                            if (row[x].Contains('('))
                            {
                                string c_name = row[x].Substring(0, row[x].IndexOf('('));
                                rules.Add(c_name + "#" + row[x].Substring(row[x].IndexOf('(') + 1, row[x].IndexOf(')') - row[x].IndexOf('(') - 1));
                                row[x] = c_name;
                            }
                            ruleList.Add(rules);
                        }

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
                Environment.Exit(0);
            }
        }

        public static DataTable getReq()
        {
            return req;
        }

        public static DataTable GetErr()
        {
            return errTable;
        }

        public static List<string> GetErrCoor()
        {
            return errCoor;
        }

        public static bool[] GetErrArray()
        {
            return errors;
        }

        public static bool Compare<T>(string op, T x, T y) where T : IComparable
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

        public static void LoadData(string family)
        {
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
                    List<string> temp = new List<string>();
                    temp.Add(row[x]);
                    uniques.Add(temp);
                }
                string line;

                // Read from Unique Value File (familyname.csv)
                while ((line = sr.ReadLine()) != null)
                {
                    // Read in the entire line and split it up by ','.
                    string[] arr = line.Split(',');
                    for (int x = 0; x < row.Length; x++)
                    {
                        if (!uniques[x].Contains(arr[x]))
                            uniques[x].Add(arr[x]);
                    }
                }
            }
        }

        public static bool UpdateSource()
        {
            string cs = @"server=10.176.3.13;userid=" + username + ";password=" + password + ";database=dev";
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            MySqlCommand cmd = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                for (int x = 0; x < d_families.Count(); x++)
                {
                    string[] disCol = d_columns[x];
                    object[] oString = new object[disCol.Length];
                    string qString = string.Join(",", disCol);

                    string stm = "SELECT DISTINCT " + qString + " FROM capacitors WHERE prodcat='" + d_families[x] + "';";
                    Console.WriteLine(stm);
                    cmd = new MySqlCommand(stm, conn);
                    cmd.CommandTimeout = 0;

                    reader = cmd.ExecuteReader();

                    using (StreamWriter w = new StreamWriter(@"\\INTELLIDATA-NAS\IntelliDataNetworkDrive\z_Quang\Projects\Randru\Configs\" + d_families[x] + ".csv", false))
                    {
                        w.WriteLine(string.Join(",", disCol));
                        while (reader.Read())
                        {
                            reader.GetValues(oString);
                            w.WriteLine(string.Join(",", oString));
                        }
                    }
                    Console.WriteLine("Done with " + d_families[x]);
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
        public static bool[] DataChk()
        {
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
                errors[(int)Err.PNless] = true;

            if (prodcat == -1)
            {
                errors[(int)Err.ProdCatLess] = true;
                return errors;
            }

            /*for (int x = 0; x < ruleList[0].Count(); x++)
            {
                Console.WriteLine(rules[x]);

                string y = rules[x].Substring(rules[x].IndexOf(' ')+1);
                string op = rules[x].Substring(rules[x].IndexOf('#') + 1, rules[x].IndexOf(' ') - rules[x].IndexOf('#') - 1);
                int h = csv.GetFieldIndex(rules[x].Substring(0, rules[x].IndexOf('#')));
                int ou,iu;

                if (int.TryParse(y, out ou) && int.TryParse("8", out iu))
                {
                    Console.WriteLine("1 " + op + " " + y + " : " + Compare(op, iu, ou));
                }
                else  
                    Console.WriteLine("1 " + op + " " + y + " : " + Compare(op, "8", y));
            }*/

            List<HashSet<string>> hashList = new List<HashSet<string>>();
            HashSet<string> hashItem = new HashSet<string>();
            hashList.Add(hashItem);
            /**** Find how to get the max size of the csv file */

            while (csv.ReadNextRecord())
            {
                if (errTable.Rows.Count > 50000)
                {
                    Array.Clear(errors, 0, errors.Length);
                    errors[(int)Err.TooMany] = true;
                    return errors;
                }
                
                // Load the relevant family rules;
                if (csv.CurrentRecordIndex == 0)
                {
                    //Console.WriteLine(csv[prodcat]);
                    famRules = d_families.IndexOf(csv[prodcat].ToUpper());
                    rules = ruleList[famRules];
                    LoadData(csv[prodcat].ToUpper());
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
                    }
                }

                // Check for new values
                for (int x = 0; x < csv.FieldCount; x++)
                {
                    for (int y = 0; y < uniques.Count(); y++)
                    {
                        if (uniques[y].First() == headers[x])
                        {

                            if (!uniques[y].Contains(csv[x]))
                            {
                                //Console.WriteLine(uniques[y].First() + ": " + csv[x]);
                                errors[(int)Err.NewValues] = true;
                                add = true;
                                errCoor.Add(r + "," + (x + 1));
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
                        
                        //Console.WriteLine(rules[x] + " : " + h + Compare(op, z, y));
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
                                add = true;
                            }
                        }
                    }
                }

                if (add)
                {
                    csv.CopyCurrentRecordTo(arr);
                    fArr[0] = (csv.CurrentRecordIndex + 2).ToString();
                    for (int j = 0; j < arr.Length; j++)
                    {
                        fArr[j + 1] = arr[j];
                    }
                    errTable.Rows.Add(fArr);
                    r++;
                }
            }

            return errors;
        }

    }
}
