using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

namespace CoinDrop
{
    public class KeyboardHook : IDisposable
    {
        #region pinvoke details

        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;

        private const int HC_ACTION = 0;
        private const int LLKHF_ALTDOWN = 0x20;

        private const int VK_SHIFT = 0x10;
        private const int VK_CONTROL = 0x11;
        private const int VK_CAPITAL = 0x14;
        private const int VK_NUMLOCK = 0x90;

        private const int KEYEVENTF_KEYUP = 0x0002;

        private enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        public struct KBDLLHOOKSTRUCT
        {
            public UInt32 vkCode;
            public UInt32 scanCode;
            public UInt32 flags;
            public UInt32 time;
            public IntPtr extraInfo;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(HookType code, HookProc func, IntPtr instance, int threadID);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, uint fuState);

        #endregion

        HookType m_hookType = HookType.WH_KEYBOARD_LL;
        IntPtr m_hookHandle = IntPtr.Zero;
        HookProc m_hookFunction = null;

        // hook method called by system
        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        // events
        public delegate void GlobalKeyEventHandler(object sender, GlobalKeyEventArgs e);
        public event GlobalKeyEventHandler OnGlobalKeyEvent;

        public KeyboardHook()
        {
            m_hookFunction = new HookProc(HookCallback);
            Install();
        }

        ~KeyboardHook()
        {
            //UnInstall();
        } //IMessageFilter 

        // hook function called by system
        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            bool Handled = false;

            if (code < 0 || code != HC_ACTION || OnGlobalKeyEvent == null)
                return CallNextHookEx(m_hookHandle, code, wParam, lParam);

            KBDLLHOOKSTRUCT KeyboardData = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            // KeyDown event
            if ((KeyboardData.flags & 0x80) == 0)
            {
                bool isAltDown = ((KeyboardData.flags & LLKHF_ALTDOWN) != 0);
                bool isControlDown = (GetAsyncKeyState(VK_CONTROL) != 0);
                bool isShiftDown = (GetAsyncKeyState(VK_SHIFT) != 0);
                bool isCapsLockDown = (GetAsyncKeyState(VK_CAPITAL) != 0);

                byte[] keyState = new byte[256];
                GetKeyboardState(keyState);
                byte[] inBuffer = new byte[2];
                char ch = '\0';

                if (ToAscii(KeyboardData.vkCode, KeyboardData.scanCode, keyState, inBuffer, KeyboardData.flags) == 1)
                {
                    ch = (char)inBuffer[0];

                    if ((isCapsLockDown ^ isShiftDown) && Char.IsLetter(ch))
                        ch = Char.ToUpper(ch);
                }

                GlobalKeyEventArgs e = new GlobalKeyEventArgs(KeyboardData.vkCode, isAltDown, isControlDown, isShiftDown, isCapsLockDown, ch, true);

                OnGlobalKeyEvent(this, e);

                Handled = Handled || e.Handled;

                //if (e.ReMap)
                //    Global.SendKeys.SendKeyDown((short)e.Key, 1, false);

                //System.Diagnostics.Debug.WriteLine("KEYDOWN: " + KeyboardData.vkCode.ToString() + " (" + Handled.ToString() + ")");
            }

            // KeyUp event
            if ((KeyboardData.flags & 0x80) != 0)
            {
                bool isAltDown = ((KeyboardData.flags & LLKHF_ALTDOWN) != 0);
                bool isControlDown = (GetAsyncKeyState(VK_CONTROL) != 0);
                bool isShiftDown = (GetAsyncKeyState(VK_SHIFT) != 0);
                bool isCapsLockDown = (GetAsyncKeyState(VK_CAPITAL) != 0);

                byte[] keyState = new byte[256];
                GetKeyboardState(keyState);
                byte[] inBuffer = new byte[2];
                char ch = '\0';

                if (ToAscii(KeyboardData.vkCode, KeyboardData.scanCode, keyState, inBuffer, KeyboardData.flags) == 1)
                {
                    ch = (char)inBuffer[0];

                    if ((isCapsLockDown ^ isShiftDown) && Char.IsLetter(ch))
                        ch = Char.ToUpper(ch);
                }

                GlobalKeyEventArgs e = new GlobalKeyEventArgs(KeyboardData.vkCode, isAltDown, isControlDown, isShiftDown, isCapsLockDown, ch, false);

                OnGlobalKeyEvent(this, e);

                Handled = Handled || e.Handled;

                //if (e.ReMap)
                //    Global.SendKeys.SendKeyUp((short)e.Key, true);

                //System.Diagnostics.Debug.WriteLine("  KEYUP: " + KeyboardData.vkCode.ToString() + " (" + Handled.ToString() + ")");
            }

            return (Handled ? 1 : CallNextHookEx(m_hookHandle, code, wParam, lParam));
        }

        private void Install()
        {
            // make sure not already installed
            if (m_hookHandle != IntPtr.Zero)
                return;

           // install system-wide hook
            //m_hookHandle = SetWindowsHookEx(m_hookType, m_hookFunction, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
			m_hookHandle = SetWindowsHookEx(m_hookType, m_hookFunction, System.Diagnostics.Process.GetCurrentProcess().MainModule.BaseAddress, 0);
        }

        private void UnInstall()
        {
            if (m_hookHandle != IntPtr.Zero)
            {
                // uninstall system-wide hook
                UnhookWindowsHookEx(m_hookHandle);
                m_hookHandle = IntPtr.Zero;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            UnInstall();
        }

        #endregion
    }

    public class GlobalKeyEventArgs : EventArgs
    {
        public uint Key;
        public bool Alt;
        public bool Control;
        public bool Shift;
        public bool CapsLock;
        public char Char;
        public bool KeyDown;
        public bool Handled = false;
        public bool ReMap = false;

        public GlobalKeyEventArgs(uint keyCode, bool alt, bool control, bool shift, bool capslock, char ch, bool keydown)
        {
            this.Key = keyCode;
            this.Alt = alt;
            this.Control = control;
            this.Shift = shift;
            this.CapsLock = capslock;
            this.Char = ch;
            this.KeyDown = keydown;
        }
    }
}