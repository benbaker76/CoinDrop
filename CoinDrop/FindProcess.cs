// Copyright (c) 2005, Ben Baker
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//using System.Management;

namespace CoinDrop
{
    partial class Win32
    {
        //inner enum used only internally
        [Flags]
        public enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            Inherit = 0x80000000,
            All = 0x0000001F
        }
        //inner struct used only internally
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PROCESSENTRY32
        {
            const int MAX_PATH = 260;
            internal UInt32 dwSize;
            internal UInt32 cntUsage;
            internal UInt32 th32ProcessID;
            internal IntPtr th32DefaultHeapID;
            internal UInt32 th32ModuleID;
            internal UInt32 cntThreads;
            internal UInt32 th32ParentProcessID;
            internal Int32 pcPriClassBase;
            internal UInt32 dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            internal string szExeFile;
        }

        [DllImport("user32.dll")]
        public static extern short GetKeyState(Keys nVirtKey);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_RESTORE = 0xf120;
        public const int SC_HOTKEY = 0xf150;
        public const int SW_SHOW = 5;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern IntPtr CreateToolhelp32Snapshot([In]UInt32 dwFlags, [In]UInt32 th32ProcessID);

        [DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool Process32First([In]IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool Process32Next([In]IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("user32.dll", SetLastError=true)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        public delegate bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowsCallback callPtr, IntPtr lParam);
    }

    public class FindProcess
    {
        private static IntPtr m_hWnd;
        private static string m_WindowTitle = null;
        private static string m_WindowClass = null;
        private static IntPtr hWndFromPid = IntPtr.Zero;
        private static int PidFound = 0;

        /* public static string GetCommandLineFromHwnd(IntPtr hWnd)
        {
            int pid = 0;

            try
            {
                Win32.GetWindowThreadProcessId(hWnd, out pid);

                if (pid == 0)
                    return null;

                return GetCommandLineFromPid(pid);
            }
            catch(Exception ex)
            {
                LogFile.WriteEntry("GetCommandLineFromHwnd", "FindProcess", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetCommandLineFromPid(int pid)
        {
            try
            {
                SelectQuery selectQuery = new SelectQuery(String.Format("select * from Win32_Process where ProcessId={0}", pid));

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery))
                {
                    foreach (ManagementObject process in searcher.Get())
                        return (string)process.Properties["CommandLine"].Value;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("GetCommandLineFromPid", "FindProcess", ex.Message, ex.StackTrace);
            }

            return null;
        } */

        public static IntPtr GetHwndFromExe(string exe)
        {
            if(String.IsNullOrEmpty(exe))
                return IntPtr.Zero;

            try
            {
                PidFound = GetPidFromExe(exe);

                if (PidFound == 0)
                    return IntPtr.Zero;

                hWndFromPid = IntPtr.Zero;

                Win32.EnumWindows(new Win32.EnumWindowsCallback(EnumWindowsCallbackPid), IntPtr.Zero);
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("GetHwndFromExe", "FindProcess", ex.Message, ex.StackTrace);
            }

            return hWndFromPid;
        }

        public static int GetPidFromExe(string exe)
        {
            if (String.IsNullOrEmpty(exe))
                return 0;

            int pid = 0;

            try
            {
                Win32.PROCESSENTRY32 procEntry = new Win32.PROCESSENTRY32();
                procEntry.dwSize = (UInt32)Marshal.SizeOf(typeof(Win32.PROCESSENTRY32));
                IntPtr handleToSnapshot = Win32.CreateToolhelp32Snapshot((uint)Win32.SnapshotFlags.Process, 0);
                if (Win32.Process32First(handleToSnapshot, ref procEntry))
                {
                    do
                    {
                        if (exe.ToLower() == procEntry.szExeFile.ToLower())
                        {
                            pid = (int) procEntry.th32ProcessID;

                            break;
                        }
                    } while (Win32.Process32Next(handleToSnapshot, ref procEntry));
                }
                else
                {
                    LogFile.WriteEntry("GetPidFromExe", "FindProcess", string.Format("Failed with win32 error code {0}", Marshal.GetLastWin32Error()));
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("GetPidFromExe", "FindProcess", ex.Message, ex.StackTrace);
            }
            return pid;
        }

        // The EnumWindowsProc callback 
        public static bool EnumWindowsCallbackPid(IntPtr hWnd, IntPtr lParam)
        { 
            int pid = 0;

            Win32.GetWindowThreadProcessId(hWnd, out pid);

            if (pid == PidFound)
            {
                hWndFromPid = hWnd;
                return false;
            }
            else
                return true;
        }

        public static string GetWindowTitle(IntPtr hWnd)
        {
            StringBuilder s = new StringBuilder(1024);
            Win32.GetWindowText(hWnd, s, 1023);

            return s.ToString();
        }

        public static IntPtr FindWindow(string WindowTitle, string WindowClass)
        {
            if (String.IsNullOrEmpty(WindowTitle) && String.IsNullOrEmpty(WindowClass))
                return IntPtr.Zero;

            IntPtr hWnd = IntPtr.Zero;

            try
            {
                hWnd = Win32.FindWindow(WindowClass, WindowTitle);

                if (hWnd == IntPtr.Zero)
                {
                    m_hWnd = IntPtr.Zero;
                    m_WindowTitle = WindowTitle;
                    m_WindowClass = WindowClass;

                    Win32.EnumWindows(new Win32.EnumWindowsCallback(EnumWindowCallback), IntPtr.Zero);

                    if (m_hWnd != IntPtr.Zero)
                        return m_hWnd;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("FindWindow", "FindProcess", ex.Message, ex.StackTrace);
            }

            return hWnd;
        }

        /* public static void PressMouse(int x, int y)
        {
            Win32.mouse_event(Win32.MOUSEEVENTF_LEFTDOWN | Win32.MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        } */

        public static bool AppActivate(string Name)
        {
            bool ret = false;

            ret = AppActivate(Name, null);

            if (!ret)
                ret = AppActivate(null, Name);

            return ret;
        }

        public static bool AppActivate(string WindowTitle, string WindowClass)
        {
            IntPtr hWnd = FindWindow(WindowTitle, WindowClass);

            if (hWnd == IntPtr.Zero)
                return false;

            return AppActivate(hWnd);
        }

        public static bool AppActivate(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return false;

            Win32.SendMessage(new HandleRef(Global.MainForm, hWnd), Win32.WM_SYSCOMMAND, (IntPtr)Win32.SC_HOTKEY, hWnd);
            Win32.SendMessage(new HandleRef(Global.MainForm, hWnd), Win32.WM_SYSCOMMAND, (IntPtr)Win32.SC_RESTORE, hWnd);

            Win32.ShowWindow(hWnd, Win32.SW_SHOW);
            Win32.SetForegroundWindow(hWnd);
            Win32.SetFocus(hWnd);
            Win32.SetActiveWindow(hWnd);
            //Win32.WaitForInputIdle()

            return true;
        }

        private static bool EnumWindowCallback(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder sb = new StringBuilder(1024);

            if (!String.IsNullOrEmpty(m_WindowTitle))
            {
                Win32.GetWindowText(hWnd, sb, sb.Capacity);

                if (sb.ToString().StartsWith(m_WindowTitle))
                {
                    m_hWnd = hWnd;

                    return false;
                }
            } else if (!String.IsNullOrEmpty(m_WindowClass))
            {
                Win32.GetClassName(hWnd, sb, sb.Capacity);

                if (sb.ToString().StartsWith(m_WindowClass))
                {
                    m_hWnd = hWnd;

                    return false;
                }
            }

            return true;
        }
    }
}
