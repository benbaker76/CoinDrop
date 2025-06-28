namespace CoinDrop
{
    partial class frmPassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.txtEnter = new System.Windows.Forms.TextBox();
            this.lblEnter = new System.Windows.Forms.Label();
            this.grpPassword = new System.Windows.Forms.GroupBox();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.grpPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(12, 99);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(107, 34);
            this.butOK.TabIndex = 1;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(173, 99);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(107, 34);
            this.butCancel.TabIndex = 2;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // txtEnter
            // 
            this.txtEnter.Location = new System.Drawing.Point(70, 23);
            this.txtEnter.Name = "txtEnter";
            this.txtEnter.Size = new System.Drawing.Size(174, 20);
            this.txtEnter.TabIndex = 1;
            this.txtEnter.UseSystemPasswordChar = true;
            this.txtEnter.TextChanged += new System.EventHandler(this.txtEnter_TextChanged);
            this.txtEnter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEnter_KeyPress);
            // 
            // lblEnter
            // 
            this.lblEnter.AutoSize = true;
            this.lblEnter.Location = new System.Drawing.Point(16, 26);
            this.lblEnter.Name = "lblEnter";
            this.lblEnter.Size = new System.Drawing.Size(35, 13);
            this.lblEnter.TabIndex = 0;
            this.lblEnter.Text = "Enter:";
            // 
            // grpPassword
            // 
            this.grpPassword.Controls.Add(this.txtConfirm);
            this.grpPassword.Controls.Add(this.lblConfirm);
            this.grpPassword.Controls.Add(this.txtEnter);
            this.grpPassword.Controls.Add(this.lblEnter);
            this.grpPassword.Location = new System.Drawing.Point(12, 6);
            this.grpPassword.Name = "grpPassword";
            this.grpPassword.Size = new System.Drawing.Size(265, 87);
            this.grpPassword.TabIndex = 0;
            this.grpPassword.TabStop = false;
            this.grpPassword.Text = "Enter Password";
            // 
            // txtConfirm
            // 
            this.txtConfirm.Location = new System.Drawing.Point(70, 49);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(174, 20);
            this.txtConfirm.TabIndex = 3;
            this.txtConfirm.UseSystemPasswordChar = true;
            this.txtConfirm.TextChanged += new System.EventHandler(this.txtConfirm_TextChanged);
            // 
            // lblConfirm
            // 
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new System.Drawing.Point(16, 52);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(45, 13);
            this.lblConfirm.TabIndex = 2;
            this.lblConfirm.Text = "Confirm:";
            // 
            // frmPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 145);
            this.Controls.Add(this.grpPassword);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPassword";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password";
            this.grpPassword.ResumeLayout(false);
            this.grpPassword.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.TextBox txtEnter;
        private System.Windows.Forms.Label lblEnter;
        private System.Windows.Forms.GroupBox grpPassword;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Label lblConfirm;
    }
}