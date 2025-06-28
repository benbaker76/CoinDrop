using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace CoinDrop
{
    class LogFile
    {
        public static string FileName = null;

        static LogFile()
        {
        }

        public static void ClearLog()
        {
            try
            {
                using (System.IO.StreamWriter sw = File.CreateText(FileName))
                    sw.Flush();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        public static void WriteEntry(string Method, string Class, string Error)
        {
            WriteEntry(String.Format("ERROR @ {0} ({1})", Method, Class));
            WriteEntry(Error);
        }

        public static void WriteEntry(string Method, string Class, string Error, string StackTrace)
        {
            WriteEntry(String.Format("ERROR @ {0} ({1})", Method, Class));
            WriteEntry(Error);
            WriteEntry(StackTrace);
        }

        public static void WriteEntry(string Error, string StackTrace)
        {
            WriteEntry(Error);
            WriteEntry(StackTrace);
        }

        public static void WriteEntry(string Entry)
        {
            try
            {
                using (System.IO.StreamWriter sw = File.AppendText(FileName))
                {
                    sw.WriteLine(DateTime.Now + ": " + Entry);
                    sw.Flush();
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
    }
}
