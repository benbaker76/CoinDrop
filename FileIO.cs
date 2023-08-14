using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CoinDrop
{
    class FileIO
    {
        public static bool TryGetWav(IWin32Window owner, string initialDirectory, out string Wav)
        {
            Wav = null;

            try
            {
                OpenFileDialog fd = new OpenFileDialog();

                fd.Title = "Select Mame Exe";
                fd.InitialDirectory = initialDirectory;
                //fd.FileName = ;
                fd.Filter = "Wav Files (*.wav)|*.wav|All Files (*.*)|*.*";
                fd.RestoreDirectory = true;
                fd.CheckFileExists = true;

                if (fd.ShowDialog(owner) == DialogResult.OK)
                {
                    Wav = fd.FileName;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("TryGetWav", "FileIO", ex.Message, ex.StackTrace);
            }

            return false;
        }
    }
}
