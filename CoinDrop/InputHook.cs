// Copyright (c) 2005, Ben Baker
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CoinDrop
{
    class InputHook : IDisposable
    {
        private bool m_mode64 = false;

        public enum KeyState
        {
            KeyDown = 0,
            KeyUp = 1
        }

        [DllImport("InputHook32.dll", EntryPoint = "InitializeHook", CallingConvention = CallingConvention.Cdecl)]
        private static extern void InitializeHook32(StringBuilder name);

        [DllImport("InputHook32.dll", EntryPoint = "ShutdownHook", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShutdownHook32();

        [DllImport("InputHook32.dll", EntryPoint = "SendRawKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendRawKey32(int key, int state);

        [DllImport("InputHook64.dll", EntryPoint = "InitializeHook", CallingConvention = CallingConvention.Cdecl)]
        private static extern void InitializeHook64(StringBuilder name);

        [DllImport("InputHook64.dll", EntryPoint = "ShutdownHook", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShutdownHook64();

        [DllImport("InputHook64.dll", EntryPoint = "SendRawKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SendRawKey64(int key, int state);

        public InputHook(bool mode64)
        {
            m_mode64 = mode64;
        }

        public void Initialize(string name)
        {
            StringBuilder sb = new StringBuilder(name);

            if(m_mode64)
                InitializeHook64(sb);
            else
                InitializeHook32(sb);
        }

        public void SendRawKey(int key, KeyState state)
        {
            if (m_mode64)
                SendRawKey64(key, (int) state);
            else
                SendRawKey32(key, (int)state);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_mode64)
                ShutdownHook64();
            else
                ShutdownHook32();
        }

        #endregion
    }
}
