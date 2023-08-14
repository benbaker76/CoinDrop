using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CoinDrop
{
    public partial class frmPassword : Form
    {
        private bool m_newPassword = false;
        private string m_password = null;
        private string m_passwordConfirm = null;

        public frmPassword(bool newPassword)
        {
            InitializeComponent();

            m_newPassword = newPassword;

            if (!m_newPassword)
            {
                lblConfirm.Visible = false;
                txtConfirm.Visible = false;
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtEnter_TextChanged(object sender, EventArgs e)
        {
            m_password = txtEnter.Text;
        }

        private void txtConfirm_TextChanged(object sender, EventArgs e)
        {
            m_passwordConfirm = txtConfirm.Text;
        }

        public string Password
        {
            get { return m_password; }
        }

        public string PasswordConfirm
        {
            get { return m_passwordConfirm; }
        }

        private void txtEnter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!m_newPassword)
            {
                if (e.KeyChar == (char)13)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}