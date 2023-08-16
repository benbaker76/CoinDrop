// Copyright (c) 2005, Ben Baker
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CoinDrop
{
    partial class Win32
    {
        public const int WM_QUIT = 0x12;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
    class MameManager : IDisposable
    {
        private System.Timers.Timer m_timer = null;
        private Stopwatch m_stopWatch = null;
        private bool m_fiveMinuteDone = false;

        private string m_ROMName = null;
        private bool m_running = false;
        private int m_credits = 0;
        private IntPtr m_hWnd = IntPtr.Zero;

        public delegate void MameStartHandler(string ROMName, string elapsedTime);
        public delegate void MameStopHandler(string ROMName, string elapsedTime, int credits);

        public event MameStartHandler OnMameStart = null;
        public event MameStopHandler OnMameStop = null;

        public MameManager(System.Windows.Forms.Control m_parent)
        {
            m_stopWatch = new Stopwatch();
            m_timer = new System.Timers.Timer();

            m_timer.SynchronizingObject = m_parent;
            m_timer.AutoReset = true;
            m_timer.Interval = 100;
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            m_timer.Start();
        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (FindMame(out m_hWnd))
            {
                string ROMName = null;

                if (m_running)
                {               
                    if(TryGetMameROMName(out ROMName))
                    {
                        if (m_ROMName == ROMName)
                        {
                            if (Settings.Management.LimitGameTimeEnable)
                            {
                                if (!m_fiveMinuteDone && m_stopWatch.Elapsed.Minutes == Settings.Management.LimitGameTime - 5)
                                {
                                    m_fiveMinuteDone = true;

                                    if (Settings.General.SoundEventsEnable)
                                        Global.PlaySound(Settings.SoundEvents.FiveMinutesRemaining, false);
                                }

                                if (m_stopWatch.Elapsed.Minutes == Settings.Management.LimitGameTime)
                                {
                                    Win32.PostMessage(m_hWnd, Win32.WM_QUIT, IntPtr.Zero, IntPtr.Zero);
                                }
                            }

                            if (Settings.General.SkipDisclaimerEnable)
                            {
                                if (m_stopWatch.Elapsed.Seconds == Settings.General.SkipDisclaimerDelay)
                                {
                                    SkipDisclaimer();
                                }
                            }

                            return;
                        }
                        else
                            MameStart(ROMName);
                    }
                    else
                    {
                        MameStop();
                    }
                }
                else
                {
                    if (TryGetMameROMName(out ROMName))
                        MameStart(ROMName);
                }
            }
            else
            {
                if (m_running)
                    MameStop();
            }
        }

        private void MameStart(string ROMName)
        {
            m_running = true;
            m_credits = 0;
            m_ROMName = ROMName;
            m_stopWatch.Start();
            m_fiveMinuteDone = false;

            if (OnMameStart != null)
                OnMameStart(m_ROMName, GetElapsedTime());
        }

        private void MameStop()
        {
            m_running = false;

            if (OnMameStop != null)
                OnMameStop(m_ROMName, GetElapsedTime(), m_credits);

            m_ROMName = null;
            m_stopWatch.Reset();
        }

        private string GetElapsedTime()
        {
            if (!m_stopWatch.IsRunning)
                return null;

            return String.Format("{0:00}:{1:00}:{2:00}", m_stopWatch.Elapsed.Hours, m_stopWatch.Elapsed.Minutes, m_stopWatch.Elapsed.Seconds);

        }

        public bool FindMame(out IntPtr hWnd)
        {
            hWnd = FindProcess.FindWindow(null, "MAME");

            return (hWnd != IntPtr.Zero);
        }

        public bool TryGetMameROMName(out string ROMName)
        {
            ROMName = null;
            string WindowTitle = FindProcess.GetWindowTitle(m_hWnd);

            if (WindowTitle != null)
            {
                if (WindowTitle.IndexOf('[') < WindowTitle.IndexOf(']'))
                {
                    ROMName = WindowTitle.Substring(WindowTitle.IndexOf('[') + 1, WindowTitle.IndexOf(']') - WindowTitle.IndexOf('[') - 1);
                    return true;
                }
            }

            return false;
        }

        private void SkipDisclaimer()
        {
            for (int i = 0; i < 5; i++)
            {
                Global.InputHook.SendRawKey(KeyCodes.VK2DIK((int)Keys.O), InputHook.KeyState.KeyDown);
                System.Threading.Thread.Sleep(100);
                Global.InputHook.SendRawKey(KeyCodes.VK2DIK((int)Keys.O), InputHook.KeyState.KeyUp);
                System.Threading.Thread.Sleep(100);
                Global.InputHook.SendRawKey(KeyCodes.VK2DIK((int)Keys.K), InputHook.KeyState.KeyDown);
                System.Threading.Thread.Sleep(100);
                Global.InputHook.SendRawKey(KeyCodes.VK2DIK((int)Keys.K), InputHook.KeyState.KeyUp);
                System.Threading.Thread.Sleep(100);
            }
        }

        public void UseCredit()
        {
            if (m_running)
                m_credits++;
        }

        public bool Running
        {
            get { return m_running; }
        }

        public IntPtr Hwnd
        {
            get { return m_hWnd; }
        }

        public string ROMName
        {
            get { return m_ROMName; }
        }

        public string ElaspedTime
        {
            get { return GetElapsedTime(); }
        }

        public int Credits
        {
            get { return m_credits; }
        }

        public void Dispose()
        {
            if (m_timer != null)
            {
                m_timer.Stop();
                m_timer.Dispose();
                m_timer = null;
            }
            if (m_stopWatch != null)
            {
                m_stopWatch.Stop();
            }
        }
    }
}
