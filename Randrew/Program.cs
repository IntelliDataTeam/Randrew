﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Randrew
{
    static class Program
    {
        enum Err
        {
            Good,
            Duplicates,
            Errors,
            Missings
        }
        private static CsvReader csv;
        private static string[] headers;
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

        public static int DataChk()
        {
            int pn = csv.GetFieldIndex("PN");
            if (pn == -1)
            {

            }
            List<HashSet<string>> hashList = new List<HashSet<string>>();
            HashSet<string> hashItem = new HashSet<string>();
            string[] temp = new string[csv.FieldCount];
            hashList.Add(hashItem);
            while (csv.ReadNextRecord())
            {
                try
                {
                    if (hashList.TrueForAll(hSet => !hSet.Contains(csv[0])))
                    {
                        hashItem.Add(csv[0]);
                    }
                    else
                    {
                        Console.Write(csv.CurrentRecordIndex + ": " + csv[0]);
                        Console.WriteLine();
                        return (int)Err.Duplicates;
                    }
                    csv.CopyCurrentRecordTo(temp);
                    if (string.Join(",",temp).Contains('#'))
                    {
                        Console.Write(csv.CurrentRecordIndex + ": " + csv[0]);
                        Console.WriteLine();
                        return (int)Err.Errors;
                    }
                }
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
