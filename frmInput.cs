using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CoinDrop
{
    public partial class frmInput : Form
    {
        public int KeyCode;
        private int m_keyCode;
        private bool keystrokeProcessed;

        [DllImport("user32.dll")]
        private static extern short GetKeyState(Keys nVirtKey);

        public frmInput()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputForm_KeyDown);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            KeyCode = (int) Keys.None;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!keystrokeProcessed)
                m_keyCode = (int) e.KeyCode;

            e.Handled = true;

            KeyCode = m_keyCode;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            keystrokeProcessed = true;
            m_keyCode = (int) keyData;

            switch (keyData)
            {
                case Keys.Enter:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    InputForm_KeyDown(this, new KeyEventArgs(keyData));
                    break;
                default:
                    if ((GetKeyState(Keys.LShiftKey) & 0xF0000000) != 0)
                    {
                        m_keyCode = (int)Keys.LShiftKey;
                        keystrokeProcessed = true;
                    }
                    if ((GetKeyState(Keys.RShiftKey) & 0xF0000000) != 0)
                    {
                        m_keyCode = (int)Keys.RShiftKey;
                        keystrokeProcessed = true;
                    }
                    if ((GetKeyState(Keys.LControlKey) & 0xF0000000) != 0)
                    {
                        m_keyCode = (int)Keys.LControlKey;
                        keystrokeProcessed = true;
                    }
                    if ((GetKeyState(Keys.RControlKey) & 0xF0000000) != 0)
                    {
                        m_keyCode = (int)Keys.RControlKey;
                        keystrokeProcessed = true;
                    }
                    if ((GetKeyState(Keys.LMenu) & 0xF0000000) != 0)
                    {
                        m_keyCode = (int)Keys.LMenu;
                        keystrokeProcessed = true;
                    }
                    if ((GetKeyState(Keys.RMenu) & 0xF0000000) != 0)
                    {
                        m_keyCode = (int) Keys.RMenu;
                        keystrokeProcessed = true;
                    }
                    break;
            }

            this.Invalidate();
            this.Update();

            return base.ProcessDialogKey(keyData);
        }

        public static bool TryGetKey(ref int KeyCode)
        {
            try
            {
                using (frmInput input = new frmInput())
                {
                    if(input.ShowDialog(Global.MainForm) == DialogResult.OK)
                    {
                        KeyCode = input.KeyCode;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("TryGetKey", "frmInput", ex.Message, ex.StackTrace);
            }

            return false;
        }
    }
}