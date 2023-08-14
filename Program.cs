using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CoinDrop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CmdLine cmdLine = new CmdLine(args);
            string value = null;

            if (cmdLine.CmdHash.TryGetValue("-minimized", out value))
                Settings.General.Minimized = true;

            using (Global.MainForm = new frmMain())
            {
                Application.Run(Global.MainForm);
            }
        }
    }
}