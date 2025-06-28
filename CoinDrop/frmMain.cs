// Copyright (c) 2005, Ben Baker
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CoinDrop
{
    public partial class frmMain : Form
    {
        private System.Windows.Forms.Timer m_insertCreditTimer = null;
        private System.Windows.Forms.Timer m_insertDelayTimer = null;
        private SerialPort m_serialPort = null;
        private bool m_allowInsert = true;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            this.Text = this.Text.Replace("[VERSION]", version.ToString(3));
            this.lblAbout.Text = this.lblAbout.Text.Replace("[VERSION]", version.ToString(3));

            cboCOMPort.Items.AddRange(SerialPort.GetPortNames());
            cboBaudRate.Items.AddRange(Global.BaudRateNames);
            cboDataBits.Items.AddRange(Global.DataBitNames);
            cboParity.Items.AddRange(Global.ParityNames);
            cboStopBits.Items.AddRange(Global.StopBitNames);
            cboHandshake.Items.AddRange(Global.HandshakeNames);
            cboLPTPort.Items.AddRange(Global.LPTNames);

            Settings.Folder.App = Application.StartupPath;
            Settings.Folder.Samples = Path.Combine(Settings.Folder.App, "Samples");

            Settings.FileName.Log = Path.Combine(Settings.Folder.App, "CoinDrop.log");
            Settings.FileName.Ini = Path.Combine(Settings.Folder.App, "CoinDrop.ini");

            LogFile.FileName = Settings.FileName.Log;
            LogFile.ClearLog();
            LogFile.WriteEntry("CoinDrop " + version.ToString(3));

            m_insertCreditTimer = new System.Windows.Forms.Timer();
            m_insertCreditTimer.Tick += new EventHandler(InsertCreditTimer);

            m_insertDelayTimer = new System.Windows.Forms.Timer();
            m_insertDelayTimer.Tick += new EventHandler(InsertDelayTimer);

            ReadConfig();

            try
            {
                InitializeComPort();

                Global.Mode64 = Marshal.SizeOf(typeof(IntPtr)) == 8;
                Global.InputHook = new InputHook(Global.Mode64);
                Global.InputHook.Initialize("mame");

                Global.KeyboardHook = new KeyboardHook();
                Global.MameManager = new MameManager(this);
                Global.SoundPlayer = new System.Media.SoundPlayer();

                nudCreditCount.Value = Settings.General.CreditCount = Settings.General.StartCreditCount;

                SendComm();

                SetInsertCreditTimer();

                this.cboCOMPort.SelectedIndexChanged += new System.EventHandler(this.cboCOM_SelectedIndexChanged);
                this.cboBaudRate.SelectedIndexChanged += new System.EventHandler(this.cboCOM_SelectedIndexChanged);
                this.cboDataBits.SelectedIndexChanged += new System.EventHandler(this.cboCOM_SelectedIndexChanged);
                this.cboParity.SelectedIndexChanged += new System.EventHandler(this.cboCOM_SelectedIndexChanged);
                this.cboStopBits.SelectedIndexChanged += new System.EventHandler(this.cboCOM_SelectedIndexChanged);
                this.cboHandshake.SelectedIndexChanged += new System.EventHandler(this.cboCOM_SelectedIndexChanged);
                this.chkCOMEnabled.CheckedChanged += new System.EventHandler(this.chkCOMEnabled_CheckedChanged);
                Global.KeyboardHook.OnGlobalKeyEvent += new KeyboardHook.GlobalKeyEventHandler(OnGlobalKeyEvent);
                Global.MameManager.OnMameStart += new MameManager.MameStartHandler(OnMameStart);
                Global.MameManager.OnMameStop += new MameManager.MameStopHandler(OnMameStop);

                if (Settings.General.Minimized)
                {
                    HideForm();
                }
                else
                {
                    if (Settings.General.PasswordProtectEnable)
                    {
                        this.Show();
                        if (!IsPasswordCorrect())
                            this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("frmMain_Load", "frmMain", ex.Message, ex.StackTrace);
            }
        }

        private void ReadConfig()
        {
            try
            {
                LogFile.WriteEntry("Reading Config.");

                IniFile iniFile = new IniFile(Settings.FileName.Ini);

                chkRunOnStartup.Checked = Convert.StrToBool(iniFile.ReadValueDefault("General", "RunOnStartup", "False"));
                nudStartCreditCount.Value = Convert.StrToInt(iniFile.ReadValueDefault("General", "StartCreditCount", "0"));
                chkCoinDropSoundEnable.Checked = Convert.StrToBool(iniFile.ReadValueDefault("General", "CoinDropSoundEnable", "True"));
                txtCoinDropSound.Text = iniFile.ReadValueDefault("General", "CoinDropSound", Path.Combine(Settings.Folder.Samples, "1up01.wav"));
                chkSkipDisclaimerEnable.Checked = Convert.StrToBool(iniFile.ReadValueDefault("General", "SkipDisclaimerEnable", "False"));
                nudSkipDisclaimerDelay.Value = Convert.StrToInt(iniFile.ReadValueDefault("General", "SkipDisclaimerDelay", "5"));
                chkPasswordProtectEnable.Checked = Convert.StrToBool(iniFile.ReadValueDefault("General", "PasswordProtectEnable", "False"));
                txtPasswordProtect.Text = iniFile.ReadValueDefault("General", "PasswordProtect", null);
                chkSoundEventsEnable.Checked = Convert.StrToBool(iniFile.ReadValueDefault("General", "SoundEventsEnable", "True"));

                txtFreeCredits.Text = iniFile.ReadValueDefault("SoundEvents", "FreeCredits", Path.Combine(Settings.Folder.Samples, "FreeCredits.wav"));
                txtFiveMinutesRemaining.Text = iniFile.ReadValueDefault("SoundEvents", "FiveMinutesRemaining", Path.Combine(Settings.Folder.Samples, "FiveMinutesRemaining.wav"));
                txtMaximumCreditLimit.Text = iniFile.ReadValueDefault("SoundEvents", "MaximumCreditLimit", Path.Combine(Settings.Folder.Samples, "MaximumCreditLimit.wav"));
                txtGameCreditLimit.Text = iniFile.ReadValueDefault("SoundEvents", "GameCreditLimit", Path.Combine(Settings.Folder.Samples, "GameCreditLimit.wav"));

                chkLimitGameTimeEnable.Checked = Convert.StrToBool(iniFile.ReadValueDefault("Management", "LimitGameTimeEnable", "False"));
                nudLimitGameTime.Value = Convert.StrToInt(iniFile.ReadValueDefault("Management", "LimitGameTime", "30"));
                chkLimitGameCreditsEnable.Checked = Convert.StrToBool(iniFile.ReadValueDefault("Management", "LimitGameCreditsEnable", "False"));
                nudLimitGameCredits.Value = Convert.StrToInt(iniFile.ReadValueDefault("Management", "LimitGameCredits", "5"));
                cboInsertCreditDetect.SelectedIndex = Convert.StrToInt(iniFile.ReadValueDefault("Management", "InsertCreditDetect", "1"));
                chkIgnoreExtraInserts.Checked = Convert.StrToBool(iniFile.ReadValueDefault("Management", "IgnoreExtraInserts", "False"));
                chkLimitMaximumCreditsEnable.Checked = Convert.StrToBool(iniFile.ReadValueDefault("Management", "LimitMaximumCreditsEnable", "False"));
                nudLimitMaximumCredits.Value = Convert.StrToInt(iniFile.ReadValueDefault("Management", "LimitMaximumCredits", "10"));
                chkInsertCreditTimerEnable.Checked = Convert.StrToBool(iniFile.ReadValueDefault("Management", "InsertCreditTimerEnable", "False"));
                nudInsertCreditTimer.Value = Convert.StrToInt(iniFile.ReadValueDefault("Management", "InsertCreditTimer", "5"));
                nudInsertCreditTimerMinutes.Value = Convert.StrToInt(iniFile.ReadValueDefault("Management", "InsertCreditTimerMinutes", "30"));

                Settings.Input.StartKeys[0] = (uint) Convert.StrToInt(iniFile.ReadValueDefault("Input", "P1Start", Settings.Input.StartKeys[0].ToString()));
                Settings.Input.StartKeys[1] = (uint) Convert.StrToInt(iniFile.ReadValueDefault("Input", "P2Start", Settings.Input.StartKeys[1].ToString()));
                Settings.Input.StartKeys[2] = (uint) Convert.StrToInt(iniFile.ReadValueDefault("Input", "P3Start", Settings.Input.StartKeys[2].ToString()));
                Settings.Input.StartKeys[3] = (uint) Convert.StrToInt(iniFile.ReadValueDefault("Input", "P4Start", Settings.Input.StartKeys[3].ToString()));

                Settings.Input.CoinKeys[0] = (uint) Convert.StrToInt(iniFile.ReadValueDefault("Input", "P1Coin", Settings.Input.CoinKeys[0].ToString()));
                Settings.Input.CoinKeys[1] = (uint) Convert.StrToInt(iniFile.ReadValueDefault("Input", "P2Coin", Settings.Input.CoinKeys[1].ToString()));
                Settings.Input.CoinKeys[2] = (uint) Convert.StrToInt(iniFile.ReadValueDefault("Input", "P3Coin", Settings.Input.CoinKeys[2].ToString()));
                Settings.Input.CoinKeys[3] = (uint) Convert.StrToInt(iniFile.ReadValueDefault("Input", "P4Coin", Settings.Input.CoinKeys[3].ToString()));

                chkCOMEnabled.Checked = Convert.StrToBool(iniFile.ReadValueDefault("Comm", "COMEnabled", "False"));
                cboCOMPort.SelectedIndex = Convert.StrToInt(iniFile.ReadValueDefault("Comm", "COMPort", "0"));
                txtStartString.Text = iniFile.ReadValueDefault("Comm", "StartString", "");
                cboBaudRate.SelectedIndex = Convert.StrToInt(iniFile.ReadValueDefault("Comm", "BaudRate", "5"));
                cboDataBits.SelectedIndex = Convert.StrToInt(iniFile.ReadValueDefault("Comm", "DataBits", "3"));
                cboParity.SelectedIndex = Convert.StrToInt(iniFile.ReadValueDefault("Comm", "Parity", "0"));
                cboStopBits.SelectedIndex = Convert.StrToInt(iniFile.ReadValueDefault("Comm", "StopBits", "1"));
                cboHandshake.SelectedIndex = Convert.StrToInt(iniFile.ReadValueDefault("Comm", "Handshake", "0"));
                chkLPTEnabled.Checked = Convert.StrToBool(iniFile.ReadValueDefault("Comm", "LPTEnabled", "False"));
                cboLPTPort.SelectedIndex = Convert.StrToInt(iniFile.ReadValueDefault("Comm", "LPTPort", "0"));

                txtP1Start.Text = ((Keys)Settings.Input.StartKeys[0]).ToString();
                txtP2Start.Text = ((Keys)Settings.Input.StartKeys[1]).ToString();
                txtP3Start.Text = ((Keys)Settings.Input.StartKeys[2]).ToString();
                txtP4Start.Text = ((Keys)Settings.Input.StartKeys[3]).ToString();

                txtP1Coin.Text = ((Keys)Settings.Input.CoinKeys[0]).ToString();
                txtP2Coin.Text = ((Keys)Settings.Input.CoinKeys[1]).ToString();
                txtP3Coin.Text = ((Keys)Settings.Input.CoinKeys[2]).ToString();
                txtP4Coin.Text = ((Keys)Settings.Input.CoinKeys[3]).ToString();
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("ReadConfig", "frmMain", ex.Message, ex.StackTrace);
            }
        }

        private void WriteConfig()
        {
            try
            {
                LogFile.WriteEntry("Writing Config.");

                IniFile iniFile = new IniFile(Settings.FileName.Ini);

                iniFile.WriteValue("General", "RunOnStartup", Settings.General.RunOnStartup.ToString());
                iniFile.WriteValue("General", "StartCreditCount", Settings.General.StartCreditCount.ToString());
                iniFile.WriteValue("General", "CoinDropSoundEnable", Settings.General.CoinDropSoundEnable.ToString());
                iniFile.WriteValue("General", "CoinDropSound", Settings.General.CoinDropSound);
                iniFile.WriteValue("General", "SkipDisclaimerEnable", Settings.General.SkipDisclaimerEnable.ToString());
                iniFile.WriteValue("General", "SkipDisclaimerDelay", Settings.General.SkipDisclaimerDelay.ToString());
                iniFile.WriteValue("General", "PasswordProtectEnable", Settings.General.PasswordProtectEnable.ToString());
                iniFile.WriteValue("General", "PasswordProtect", Settings.General.PasswordProtect);
                iniFile.WriteValue("General", "SoundEventsEnable", Settings.General.SoundEventsEnable.ToString());

                iniFile.WriteValue("SoundEvents", "FreeCredits", Settings.SoundEvents.FreeCredits);
                iniFile.WriteValue("SoundEvents", "FiveMinutesRemaining", Settings.SoundEvents.FiveMinutesRemaining);
                iniFile.WriteValue("SoundEvents", "MaximumCreditLimit", Settings.SoundEvents.MaximumCreditLimit);
                iniFile.WriteValue("SoundEvents", "GameCreditLimit", Settings.SoundEvents.GameCreditLimit);
                
                iniFile.WriteValue("Management", "LimitGameTimeEnable", Settings.Management.LimitGameTimeEnable.ToString());
                iniFile.WriteValue("Management", "LimitGameTime", Settings.Management.LimitGameTime.ToString());
                iniFile.WriteValue("Management", "LimitGameCreditsEnable", Settings.Management.LimitGameCreditsEnable.ToString());
                iniFile.WriteValue("Management", "LimitGameCredits", Settings.Management.LimitGameCredits.ToString());
                iniFile.WriteValue("Management", "InsertCreditDetect", ((int)Settings.Management.InsertCreditDetect).ToString());
                iniFile.WriteValue("Management", "IgnoreExtraInserts", Settings.Management.IgnoreExtraInserts.ToString());
                iniFile.WriteValue("Management", "LimitMaximumCreditsEnable", Settings.Management.LimitMaximumCreditsEnable.ToString());
                iniFile.WriteValue("Management", "LimitMaximumCredits", Settings.Management.LimitMaximumCredits.ToString());
                iniFile.WriteValue("Management", "InsertCreditTimerEnable", Settings.Management.InsertCreditTimerEnable.ToString());
                iniFile.WriteValue("Management", "InsertCreditTimer", Settings.Management.InsertCreditTimer.ToString());
                iniFile.WriteValue("Management", "InsertCreditTimerMinutes", Settings.Management.InsertCreditTimerMinutes.ToString());

                iniFile.WriteValue("Input", "P1Start", Settings.Input.StartKeys[0].ToString());
                iniFile.WriteValue("Input", "P2Start", Settings.Input.StartKeys[1].ToString());
                iniFile.WriteValue("Input", "P3Start", Settings.Input.StartKeys[2].ToString());
                iniFile.WriteValue("Input", "P4Start", Settings.Input.StartKeys[3].ToString());

                iniFile.WriteValue("Input", "P1Coin", Settings.Input.CoinKeys[0].ToString());
                iniFile.WriteValue("Input", "P2Coin", Settings.Input.CoinKeys[1].ToString());
                iniFile.WriteValue("Input", "P3Coin", Settings.Input.CoinKeys[2].ToString());
                iniFile.WriteValue("Input", "P4Coin", Settings.Input.CoinKeys[3].ToString());

                iniFile.WriteValue("Comm", "COMEnabled", chkCOMEnabled.Checked.ToString());
                iniFile.WriteValue("Comm", "COMPort", cboCOMPort.SelectedIndex.ToString());
                iniFile.WriteValue("Comm", "StartString", txtStartString.Text);
                iniFile.WriteValue("Comm", "BaudRate", cboBaudRate.SelectedIndex.ToString());
                iniFile.WriteValue("Comm", "DataBits", cboDataBits.SelectedIndex.ToString());
                iniFile.WriteValue("Comm", "Parity", cboParity.SelectedIndex.ToString());
                iniFile.WriteValue("Comm", "StopBits", cboStopBits.SelectedIndex.ToString());
                iniFile.WriteValue("Comm", "Handshake", cboHandshake.SelectedIndex.ToString());
                iniFile.WriteValue("Comm", "LPTEnabled", chkLPTEnabled.Checked.ToString());
                iniFile.WriteValue("Comm", "LPTPort", cboLPTPort.SelectedIndex.ToString());
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("WriteConfig", "frmMain", ex.Message, ex.StackTrace);
            }
        }

        private void UpdateColumns()
        {
            for (int i = 0; i < lvwLog.Columns.Count; i++)
                lvwLog.Columns[i].Width = -2;
        }

        private void AddLogInfo(string ROMName, string Action, string elapsedTime, string credit)
        {
            lvwLog.Items.Add(new ListViewItem(new string[] { DateTime.Now.ToString(), ROMName, Action, elapsedTime, credit }));
            UpdateColumns();
            lvwLog.Items[lvwLog.Items.Count -1].EnsureVisible();
            LogFile.WriteEntry(String.Format("Game: {0}, Action: {1}, ElapsedTime: {2}, Credit: {3}", ROMName, Action, elapsedTime, credit));
        }

        private void OnMameStart(string ROMName, string elapsedTime)
        {
            AddLogInfo(ROMName, "Game Start", elapsedTime, String.Format("{0} Total", Settings.General.CreditCount));

            IniFile iniFile = new IniFile(Settings.FileName.Ini);

            int insertDelay = Convert.StrToInt(iniFile.ReadValue("InsertDelay", Global.MameManager.ROMName));

            if (insertDelay > 0)
            {
                m_allowInsert = false;
                m_insertDelayTimer.Interval = insertDelay;
                m_insertDelayTimer.Start();
            }
            else
            {
                m_allowInsert = true;
            }
        }

        private void OnMameStop(string ROMName, string elapsedTime, int credits)
        {
            AddLogInfo(ROMName, "Game End", elapsedTime, String.Format("{0} Used, {1} Remaining", credits, Settings.General.CreditCount));
        }

        private void OnGlobalKeyEvent(object sender, GlobalKeyEventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                if (e.Key == (uint)Settings.Input.StartKeys[i])
                {
                    if (Global.MameManager.Running)
                    {
                        if (Settings.General.CreditCount > 0)
                        {
                            e.Handled = true;

                            if (!e.KeyDown)
                            {
                                if (m_allowInsert)
                                {
                                    UseCredit(i);

                                    System.Threading.Thread.Sleep(1000);
                                    Global.InputHook.SendRawKey(KeyCodes.VK2DIK((int)Settings.Input.StartKeys[i]), InputHook.KeyState.KeyDown);
                                    System.Threading.Thread.Sleep(100);
                                    Global.InputHook.SendRawKey(KeyCodes.VK2DIK((int)Settings.Input.StartKeys[i]), InputHook.KeyState.KeyUp);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                        }
                    }
                }

                if (e.Key == (uint)Settings.Input.CoinKeys[i])
                {
                    if (Global.MameManager.Running)
                        e.Handled = true;

                    if (!e.KeyDown)
                        InsertCredit();
               }
            }
        }

        private void SetInsertCreditTimer()
        {
            m_insertCreditTimer.Stop();
            m_insertCreditTimer.Interval = Settings.Management.InsertCreditTimerMinutes * 60000;
            m_insertCreditTimer.Enabled = Settings.Management.InsertCreditTimerEnable;
        }

        void InsertDelayTimer(object sender, EventArgs e)
        {
            m_insertDelayTimer.Stop();
            m_allowInsert = true;
        }

        private void InsertCreditTimer(object sender, EventArgs e)
        {
            if (Settings.Management.InsertCreditTimerEnable)
            {
                if (Settings.General.CreditCount < Settings.Management.InsertCreditTimer)
                {
                    if (Settings.General.SoundEventsEnable)
                        Global.PlaySound(Settings.SoundEvents.FreeCredits, true);

                    for (int i = Settings.General.CreditCount; i < Settings.Management.InsertCreditTimer; i++)
                    {
                        InsertCredit(true);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }

        private void InsertCredit()
        {
            InsertCredit(false);
        }

        private void InsertCredit(bool forceInsert)
        {
            if (!forceInsert)
            {
                if (Settings.Management.InsertCreditDetect == InsertCreditDetectType.Never)
                    return;

                if (Settings.Management.InsertCreditDetect == InsertCreditDetectType.WhileMAMEsRunning)
                {
                    if (!Global.MameManager.Running)
                        return;
                }

                if (Global.MameManager.Running)
                {
                    if (Settings.Management.LimitGameCreditsEnable)
                    {
                        if (Settings.Management.IgnoreExtraInserts)
                        {
                            if (Settings.General.CreditCount >= Settings.Management.LimitGameCredits)
                            {
                                AddLogInfo(Global.MameManager.ROMName, "Game Credit Limit Reached", Global.MameManager.ElaspedTime, String.Format("{0} Max, {1} Remaining", Settings.Management.LimitGameCredits, Settings.General.CreditCount));

                                if (Settings.General.SoundEventsEnable)
                                    Global.PlaySound(Settings.SoundEvents.GameCreditLimit, false);

                                return;
                            }
                        }
                    }
                }

                if (Settings.Management.LimitMaximumCreditsEnable)
                {
                    if (Settings.General.CreditCount >= Settings.Management.LimitMaximumCredits)
                    {
                        AddLogInfo(Global.MameManager.ROMName, "Max Credit Limit Reached", Global.MameManager.ElaspedTime, String.Format("{0} Max, {1} Remaining", Settings.Management.LimitMaximumCredits, Settings.General.CreditCount));

                        if (Settings.General.SoundEventsEnable)
                            Global.PlaySound(Settings.SoundEvents.MaximumCreditLimit, false);

                        return;
                    }
                }
            }

            SetInsertCreditTimer();
            Settings.General.CreditCount++;

            if(Settings.General.CoinDropSoundEnable)
                Global.PlaySound(Settings.General.CoinDropSound, false);

            nudCreditCount.Value = Settings.General.CreditCount;

            SendComm();

            AddLogInfo(Global.MameManager.ROMName, "Credit Insert", Global.MameManager.ElaspedTime, String.Format("{0} Total", Settings.General.CreditCount));
        }

        private void RemoveCredit()
        {
            if (Settings.General.CreditCount > 0)
            {
                Settings.General.CreditCount--;
                nudCreditCount.Value = Settings.General.CreditCount;

                SendComm();

                AddLogInfo(Global.MameManager.ROMName, "Credit Removed", Global.MameManager.ElaspedTime, String.Format("{0} Total", Settings.General.CreditCount));
            }
        }

        private void UseCredit(int player)
        {
            if (Settings.Management.LimitGameCreditsEnable)
            {
                if (Global.MameManager.Credits >= Settings.Management.LimitGameCredits)
                {
                    AddLogInfo(Global.MameManager.ROMName, "Game Credit Limit Reached", Global.MameManager.ElaspedTime, String.Format("{0} Max, {1} Remaining", Settings.Management.LimitGameCredits, Settings.General.CreditCount));

                    if (Settings.General.SoundEventsEnable)
                        Global.PlaySound(Settings.SoundEvents.GameCreditLimit, false);

                    return;
                }
            }

            if (Settings.General.CreditCount > 0)
            {
				System.Threading.Thread.Sleep(1000);
                Global.InputHook.SendRawKey(KeyCodes.VK2DIK((int)Settings.Input.CoinKeys[player]), InputHook.KeyState.KeyDown);
                System.Threading.Thread.Sleep(100);
                Global.InputHook.SendRawKey(KeyCodes.VK2DIK((int)Settings.Input.CoinKeys[player]), InputHook.KeyState.KeyUp);
                System.Threading.Thread.Sleep(100);

                SetInsertCreditTimer();
                Settings.General.CreditCount--;
                Global.MameManager.UseCredit();
                nudCreditCount.Value = Settings.General.CreditCount;

                SendComm();

                AddLogInfo(Global.MameManager.ROMName, "Credit Used", Global.MameManager.ElaspedTime, String.Format("{0} Total", Settings.General.CreditCount));
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            if (Settings.General.Minimized)
                HideForm();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowForm();

            if(Settings.General.PasswordProtectEnable)
                if(!IsPasswordCorrect())
                    HideForm();
        }

        private bool IsPasswordCorrect()
        {
            using (frmPassword passwordForm = new frmPassword(false))
            {
                if (passwordForm.ShowDialog(this) == DialogResult.OK)
                {
                    if (Crypt.MD5Hash(passwordForm.Password) == Settings.General.PasswordProtect)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                HideForm();
        }

        private void ShowForm()
        {
            this.Show();
            this.SetTopLevel(true);
            this.BringToFront();
            this.Focus();
            this.Activate();
            WindowState = FormWindowState.Normal;
        }

        private void HideForm()
        {
            this.Hide();
        }


        public void AddProgramToStartup()
        {
            try
            {
                RegistryKey RegKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                RegKey.SetValue("CoinDrop", String.Format("{0} -minimized", Application.ExecutablePath), RegistryValueKind.String);
                RegKey.Close();
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("AddProgramToStartup", "frmMain", ex.Message, ex.StackTrace);
            }
        }

        public void RemoveProgramFromStartup()
        {
            try
            {
                RegistryKey RegKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                RegKey.DeleteValue("CoinDrop", false);
                RegKey.Close();
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("RemoveProgramFromStartup", "frmMain", ex.Message, ex.StackTrace);
            }
        }

        private void InitializeComPort()
        {
            try
            {
                if (!chkCOMEnabled.Checked)
                    return;

                if (m_serialPort == null)
                    m_serialPort = new SerialPort();

                if (m_serialPort.IsOpen)
                    m_serialPort.Close();

                m_serialPort.PortName = cboCOMPort.Text;
                m_serialPort.BaudRate = Convert.StrToInt(cboBaudRate.Text);
                m_serialPort.Parity = (Parity)cboParity.SelectedIndex;
                m_serialPort.DataBits = Convert.StrToInt(cboDataBits.Text);
                m_serialPort.StopBits = (StopBits)cboStopBits.SelectedIndex;
                m_serialPort.Handshake = (Handshake)cboHandshake.SelectedIndex;

                m_serialPort.ReadTimeout = 500;
                m_serialPort.WriteTimeout = 500;

                m_serialPort.Open();
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("InitializeComPort", "frmMain", ex.Message, ex.StackTrace);
            }
        }

        private void SendComm()
        {
            try
            {
                if (chkCOMEnabled.Checked)
                {
                    if (!m_serialPort.IsOpen)
                        m_serialPort.Open();

                    if (m_serialPort.IsOpen)
                        m_serialPort.Write(String.Format("{0}{1}", txtStartString.Text, Settings.General.CreditCount.ToString("D2")));
                }

                if (chkLPTEnabled.Checked)
                    LPT.Out32((int)((LPT.LPTAddress)cboLPTPort.SelectedIndex), Settings.General.CreditCount);
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("SendComm", "frmMain", ex.Message, ex.StackTrace);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Global.KeyboardHook != null)
            {
                Global.KeyboardHook.Dispose();
                Global.KeyboardHook = null;
            }
            if (Global.InputHook != null)
            {
                Global.InputHook.Dispose();
                Global.InputHook = null;
            }

            WriteConfig();
        }

        private void nudCreditCount_ValueChanged(object sender, EventArgs e)
        {
            if ((int)nudCreditCount.Value < Settings.General.CreditCount)
                RemoveCredit();
            else if ((int)nudCreditCount.Value > Settings.General.CreditCount)
                InsertCredit(true);

            //Settings.General.CreditCount = nudCreditCount.Value;
        }

        private void chkRunOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            Settings.General.RunOnStartup = chkRunOnStartup.Checked;

            if (Settings.General.RunOnStartup)
                AddProgramToStartup();
            else
                RemoveProgramFromStartup();
        }

        private void nudStartCreditCount_ValueChanged(object sender, EventArgs e)
        {
            Settings.General.StartCreditCount = (int) nudStartCreditCount.Value;
        }

        private void chkCoinDropSoundEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.General.CoinDropSoundEnable = chkCoinDropSoundEnable.Checked;
        }

        private void txtCoinDropSound_TextChanged(object sender, EventArgs e)
        {
            Settings.General.CoinDropSound = txtCoinDropSound.Text;
        }

        private void chkSkipDisclaimerEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.General.SkipDisclaimerEnable = chkSkipDisclaimerEnable.Checked;
        }

        private void nudSkipDisclaimerDelay_ValueChanged(object sender, EventArgs e)
        {
            Settings.General.SkipDisclaimerDelay = (int)nudSkipDisclaimerDelay.Value;
        }

        private void chkPasswordProtectEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.General.PasswordProtectEnable = chkPasswordProtectEnable.Checked;
        }

        private void txtPasswordProtect_TextChanged(object sender, EventArgs e)
        {
            Settings.General.PasswordProtect = txtPasswordProtect.Text;
        }

        private void chkLimitGameTimeEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Management.LimitGameTimeEnable = chkLimitGameTimeEnable.Checked;
        }

        private void nudLimitGameTime_ValueChanged(object sender, EventArgs e)
        {
            Settings.Management.LimitGameTime = (int)nudLimitGameTime.Value;
        }

        private void cboInsertCreditDetect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Management.InsertCreditDetect = (InsertCreditDetectType)cboInsertCreditDetect.SelectedIndex;
        }

        private void chkLimitGameCreditsEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Management.LimitGameCreditsEnable = chkLimitGameCreditsEnable.Checked;
        }

        private void nudLimitGameCredits_ValueChanged(object sender, EventArgs e)
        {
            Settings.Management.LimitGameCredits = (int)nudLimitGameCredits.Value;
        }

        private void chkIgnoreExtraInserts_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Management.IgnoreExtraInserts = chkIgnoreExtraInserts.Checked;
        }

        private void chkLimitMaximumCreditsEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Management.LimitMaximumCreditsEnable = chkLimitMaximumCreditsEnable.Checked;
        }

        private void nudLimitMaximumCredits_ValueChanged(object sender, EventArgs e)
        {
            Settings.Management.LimitMaximumCredits = (int)nudLimitMaximumCredits.Value;
        }

        private void chkInsertCreditTimerEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Management.InsertCreditTimerEnable = chkInsertCreditTimerEnable.Checked;
            SetInsertCreditTimer();
        }

        private void nudInsertCreditTimer_ValueChanged(object sender, EventArgs e)
        {
            Settings.Management.InsertCreditTimer = (int)nudInsertCreditTimer.Value;
        }

        private void nudInsertCreditTimerMinutes_ValueChanged(object sender, EventArgs e)
        {
            if (nudInsertCreditTimerMinutes.Value < 1)
                nudInsertCreditTimerMinutes.Value = 1;
            Settings.Management.InsertCreditTimerMinutes = (int)nudInsertCreditTimerMinutes.Value;
            SetInsertCreditTimer();
        }


        private void butCoinDropSound_Click(object sender, EventArgs e)
        {
            string wavFileName = null;

            if (FileIO.TryGetWav(this, Settings.Folder.Samples, out wavFileName))
            {
                txtCoinDropSound.Text = wavFileName;
                Settings.General.CoinDropSound = wavFileName;
            }
        }

        private void butPasswordProtect_Click(object sender, EventArgs e)
        {
            using (frmPassword passwordForm = new frmPassword(true))
            {
                if (passwordForm.ShowDialog(this) == DialogResult.OK)
                {
                    if (passwordForm.Password == passwordForm.PasswordConfirm)
                    {
                        txtPasswordProtect.Text = Crypt.MD5Hash(passwordForm.Password);
                    }
                    else
                        MessageBox.Show(this, "Passwords Don't Match", "Error");
                }
            }
        }

        private void chkSoundEventsEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.General.SoundEventsEnable = chkSoundEventsEnable.Checked;
        }

        private void butPlayFreeCredits_Click(object sender, EventArgs e)
        {
            Global.PlaySound(Settings.SoundEvents.FreeCredits, false);
        }

        private void butPlayFiveMinutesRemaining_Click(object sender, EventArgs e)
        {
            Global.PlaySound(Settings.SoundEvents.FiveMinutesRemaining, false);
        }

        private void butPlayMaximumCreditLimit_Click(object sender, EventArgs e)
        {
            Global.PlaySound(Settings.SoundEvents.MaximumCreditLimit, false);
        }

        private void butPlayGameCreditLimit_Click(object sender, EventArgs e)
        {
            Global.PlaySound(Settings.SoundEvents.GameCreditLimit, false);
        }

        private void txtFreeCredits_TextChanged(object sender, EventArgs e)
        {
            Settings.SoundEvents.FreeCredits = txtFreeCredits.Text;
        }

        private void txtFiveMinutesRemaining_TextChanged(object sender, EventArgs e)
        {
            Settings.SoundEvents.FiveMinutesRemaining = txtFiveMinutesRemaining.Text;
        }

        private void txtMaximumCreditLimit_TextChanged(object sender, EventArgs e)
        {
            Settings.SoundEvents.MaximumCreditLimit = txtMaximumCreditLimit.Text;
        }

        private void txtGameCreditLimit_TextChanged(object sender, EventArgs e)
        {
            Settings.SoundEvents.GameCreditLimit = txtGameCreditLimit.Text;
        }

        private void butFreeCredits_Click(object sender, EventArgs e)
        {
            string wavFileName = null;

            if (FileIO.TryGetWav(this, Settings.Folder.Samples, out wavFileName))
            {
                txtFreeCredits.Text = wavFileName;
                Settings.SoundEvents.FreeCredits = wavFileName;
            }
        }

        private void butFiveMinutesRemaining_Click(object sender, EventArgs e)
        {
            string wavFileName = null;

            if (FileIO.TryGetWav(this, Settings.Folder.Samples, out wavFileName))
            {
                txtFiveMinutesRemaining.Text = wavFileName;
                Settings.SoundEvents.FiveMinutesRemaining = wavFileName;
            }
        }

        private void butMaximumCreditLimit_Click(object sender, EventArgs e)
        {
            string wavFileName = null;

            if (FileIO.TryGetWav(this, Settings.Folder.Samples, out wavFileName))
            {
                txtMaximumCreditLimit.Text = wavFileName;
                Settings.SoundEvents.MaximumCreditLimit = wavFileName;
            }
        }

        private void butGameCreditLimit_Click(object sender, EventArgs e)
        {
            string wavFileName = null;

            if (FileIO.TryGetWav(this, Settings.Folder.Samples, out wavFileName))
            {
                txtGameCreditLimit.Text = wavFileName;
                Settings.SoundEvents.GameCreditLimit = wavFileName;
            }
        }

        private void butInput_Click(object sender, EventArgs e)
        {
            Button butInput = (Button)sender;

            using (frmInput inputForm = new frmInput())
            {
                if (inputForm.ShowDialog(this) == DialogResult.OK)
                {
                    switch (butInput.Name)
                    {
                        case "butP1Start":
                            Settings.Input.StartKeys[0] = (uint)inputForm.KeyCode;
                            txtP1Start.Text = ((Keys)inputForm.KeyCode).ToString();
                            break;
                        case "butP2Start":
                            Settings.Input.StartKeys[1] = (uint)inputForm.KeyCode;
                            txtP2Start.Text = ((Keys)inputForm.KeyCode).ToString();
                            break;
                        case "butP3Start":
                            Settings.Input.StartKeys[2] = (uint)inputForm.KeyCode;
                            txtP3Start.Text = ((Keys)inputForm.KeyCode).ToString();
                            break;
                        case "butP4Start":
                            Settings.Input.StartKeys[3] = (uint)inputForm.KeyCode;
                            txtP4Start.Text = ((Keys)inputForm.KeyCode).ToString();
                            break;
                        case "butP1Coin":
                            Settings.Input.CoinKeys[0] = (uint)inputForm.KeyCode;
                            txtP1Coin.Text = ((Keys)inputForm.KeyCode).ToString();
                            break;
                        case "butP2Coin":
                            Settings.Input.CoinKeys[1] = (uint)inputForm.KeyCode;
                            txtP2Coin.Text = ((Keys)inputForm.KeyCode).ToString();
                            break;
                        case "butP3Coin":
                            Settings.Input.CoinKeys[2] = (uint)inputForm.KeyCode;
                            txtP3Coin.Text = ((Keys)inputForm.KeyCode).ToString();
                            break;
                        case "butP4Coin":
                            Settings.Input.CoinKeys[3] = (uint)inputForm.KeyCode;
                            txtP4Coin.Text = ((Keys)inputForm.KeyCode).ToString();
                            break;
                    }
                }
            }
        }

        private void butPlayCoinDropSound_Click(object sender, EventArgs e)
        {
            Global.PlaySound(Settings.General.CoinDropSound, false);
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            lvwLog.Items.Clear();
            LogFile.ClearLog();
        }

        private void butConfigure_Click(object sender, EventArgs e)
        {
            ComPort.ConfigPort(this.Handle, cboCOMPort.SelectedIndex + 1);
        }

        private void cboCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeComPort();
        }

        private void chkCOMEnabled_CheckedChanged(object sender, EventArgs e)
        {
            InitializeComPort();
        }
    }
}