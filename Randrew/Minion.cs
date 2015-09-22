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
    class Minion
    {
        private BackgroundWorker minion;

        public Minion(MainGUI mGui) {
            minion = new BackgroundWorker();
            minion.DoWork += new DoWorkEventHandler(minion_DoWork);
            minion.ProgressChanged += new ProgressChangedEventHandler(minion_ProgressChanged);
            minion.RunWorkerCompleted += new RunWorkerCompletedEventHandler(minion_RunWorkerCompleted);
            minion.WorkerReportsProgress = true;
            minion.WorkerSupportsCancellation = true;
        }

        private void minion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void minion_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void minion_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
    }
}
