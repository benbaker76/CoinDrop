// Copyright (c) 2005, Ben Baker
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CoinDrop
{
    class KeyTableNode
    {
        public int DIKKey = 0;
        public int VKKey = 0;
        public int Ascii = 0;

        public KeyTableNode(int dikKey, int vkKey, int ascii)
        {
            DIKKey = dikKey;
            VKKey = vkKey;
            Ascii = ascii;
        }
    }

    class KeyCodes
    {
        private enum VKKeys : int
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,
            VK_BACK = 0x08,
            VK_TAB = 0x09,
            VK_CLEAR = 0x0C,
            VK_RETURN = 0x0D,
            VK_SHIFT = 0x10,
            VK_CONTROL = 0x11,
            VK_MENU = 0x12,
            VK_PAUSE = 0x13,
            VK_CAPITAL = 0x14,
            VK_ESCAPE = 0x1B,
            VK_SPACE = 0x20,
            VK_PRIOR = 0x21,
            VK_NEXT = 0x22,
            VK_END = 0x23,
            VK_HOME = 0x24,
            VK_LEFT = 0x25,
            VK_UP = 0x26,
            VK_RIGHT = 0x27,
            VK_DOWN = 0x28,
            VK_SELECT = 0x29,
            VK_PRINT = 0x2A,
            VK_EXECUTE = 0x2B,
            VK_SNAPSHOT = 0x2C,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_HELP = 0x2F,
            VK_LWIN = 0x5B,
            VK_RWIN = 0x5C,
            VK_APPS = 0x5D,
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 0x64,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_MULTIPLY = 0x6A,
            VK_ADD = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT = 0x6D,
            VK_DECIMAL = 0x6E,
            VK_DIVIDE = 0x6F,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,
            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91,
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3,
            VK_LMENU = 0xA4,
            VK_RMENU = 0xA5,
            VK_PROCESSKEY = 0xE5,
            VK_ATTN = 0xF6,
            VK_CRSEL = 0xF7,
            VK_EXSEL = 0xF8,
            VK_EREOF = 0xF9,
            VK_PLAY = 0xFA,
            VK_ZOOM = 0xFB,
            VK_NONAME = 0xFC,
            VK_PA1 = 0xFD,
            VK_BROWSER_BACK = 0xA6,
            VK_BROWSER_FORWARD = 0xA7,
            VK_BROWSER_REFRESH = 0xA8,
            VK_BROWSER_STOP = 0xA9,
            VK_BROWSER_SEARCH = 0xAA,
            VK_BROWSER_FAVORITES = 0xAB,
            VK_BROWSER_HOME = 0xAC,
            VK_VOLUME_MUTE = 0xAD,
            VK_VOLUME_DOWN = 0xAE,
            VK_VOLUME_UP = 0xAF,
            VK_MEDIA_NEXT_TRACK = 0xB0,
            VK_MEDIA_PREV_TRACK = 0xB1,
            VK_MEDIA_STOP = 0xB2,
            VK_MEDIA_PLAY_PAUSE = 0xB3,
            VK_LAUNCH_MAIL = 0xB4,
            VK_LAUNCH_MEDIA_SELECT = 0xB5,
            VK_LAUNCH_APP1 = 0xB6,
            VK_LAUNCH_APP2 = 0xB7,
            VK_OEM_CLEAR = 0xFE,
            VK_OEM_1 = 0xBA,
            VK_OEM_PLUS = 0xBB,
            VK_OEM_COMMA = 0xBC,
            VK_OEM_MINUS = 0xBD,
            VK_OEM_PERIOD = 0xBE,
            VK_OEM_2 = 0xBF,
            VK_OEM_3 = 0xC0,
            VK_OEM_4 = 0xDB,
            VK_OEM_5 = 0xDC,
            VK_OEM_6 = 0xDD,
            VK_OEM_7 = 0xDE,
            VK_OEM_8 = 0xDF,
            VK_OEM_AX = 0xE1,
            VK_OEM_102 = 0xE1
        };

        private enum DIKeys : int
        {
            DIK_ESCAPE = 0x01,
            DIK_1 = 0x02,
            DIK_2 = 0x03,
            DIK_3 = 0x04,
            DIK_4 = 0x05,
            DIK_5 = 0x06,
            DIK_6 = 0x07,
            DIK_7 = 0x08,
            DIK_8 = 0x09,
            DIK_9 = 0x0A,
            DIK_0 = 0x0B,
            DIK_MINUS = 0x0C,    /* - on main keyboard */
            DIK_EQUALS = 0x0D,
            DIK_BACK = 0x0E,    /* backspace */
            DIK_TAB = 0x0F,
            DIK_Q = 0x10,
            DIK_W = 0x11,
            DIK_E = 0x12,
            DIK_R = 0x13,
            DIK_T = 0x14,
            DIK_Y = 0x15,
            DIK_U = 0x16,
            DIK_I = 0x17,
            DIK_O = 0x18,
            DIK_P = 0x19,
            DIK_LBRACKET = 0x1A,
            DIK_RBRACKET = 0x1B,
            DIK_RETURN = 0x1C,    /* Enter on main keyboard */
            DIK_LCONTROL = 0x1D,
            DIK_A = 0x1E,
            DIK_S = 0x1F,
            DIK_D = 0x20,
            DIK_F = 0x21,
            DIK_G = 0x22,
            DIK_H = 0x23,
            DIK_J = 0x24,
            DIK_K = 0x25,
            DIK_L = 0x26,
            DIK_SEMICOLON = 0x27,
            DIK_APOSTROPHE = 0x28,
            DIK_GRAVE = 0x29,    /* accent grave */
            DIK_LSHIFT = 0x2A,
            DIK_BACKSLASH = 0x2B,
            DIK_Z = 0x2C,
            DIK_X = 0x2D,
            DIK_C = 0x2E,
            DIK_V = 0x2F,
            DIK_B = 0x30,
            DIK_N = 0x31,
            DIK_M = 0x32,
            DIK_COMMA = 0x33,
            DIK_PERIOD = 0x34,    /* . on main keyboard */
            DIK_SLASH = 0x35,    /* / on main keyboard */
            DIK_RSHIFT = 0x36,
            DIK_MULTIPLY = 0x37,    /* * on numeric keypad */
            DIK_LMENU = 0x38,    /* left Alt */
            DIK_SPACE = 0x39,
            DIK_CAPITAL = 0x3A,
            DIK_F1 = 0x3B,
            DIK_F2 = 0x3C,
            DIK_F3 = 0x3D,
            DIK_F4 = 0x3E,
            DIK_F5 = 0x3F,
            DIK_F6 = 0x40,
            DIK_F7 = 0x41,
            DIK_F8 = 0x42,
            DIK_F9 = 0x43,
            DIK_F10 = 0x44,
            DIK_NUMLOCK = 0x45,
            DIK_SCROLL = 0x46,    /* Scroll Lock */
            DIK_NUMPAD7 = 0x47,
            DIK_NUMPAD8 = 0x48,
            DIK_NUMPAD9 = 0x49,
            DIK_SUBTRACT = 0x4A,    /* - on numeric keypad */
            DIK_NUMPAD4 = 0x4B,
            DIK_NUMPAD5 = 0x4C,
            DIK_NUMPAD6 = 0x4D,
            DIK_ADD = 0x4E,    /* + on numeric keypad */
            DIK_NUMPAD1 = 0x4F,
            DIK_NUMPAD2 = 0x50,
            DIK_NUMPAD3 = 0x51,
            DIK_NUMPAD0 = 0x52,
            DIK_DECIMAL = 0x53,    /* . on numeric keypad */
            DIK_OEM_102 = 0x56,    /* < > | on UK/Germany keyboards */
            DIK_F11 = 0x57,
            DIK_F12 = 0x58,
            DIK_F13 = 0x64,    /*                     (NEC PC98) */
            DIK_F14 = 0x65,    /*                     (NEC PC98) */
            DIK_F15 = 0x66,    /*                     (NEC PC98) */
            DIK_KANA = 0x70,    /* (Japanese keyboard)            */
            DIK_ABNT_C1 = 0x73,    /* / ? on Portugese (Brazilian) keyboards */
            DIK_CONVERT = 0x79,    /* (Japanese keyboard)            */
            DIK_NOCONVERT = 0x7B,    /* (Japanese keyboard)            */
            DIK_YEN = 0x7D,    /* (Japanese keyboard)            */
            DIK_ABNT_C2 = 0x7E,    /* Numpad . on Portugese (Brazilian) keyboards */
            DIK_NUMPADEQUALS = 0x8D,    /* = on numeric keypad (NEC PC98) */
            DIK_PREVTRACK = 0x90,    /* Previous Track (DIK_CIRCUMFLEX on Japanese keyboard) */
            DIK_AT = 0x91,    /*                     (NEC PC98) */
            DIK_COLON = 0x92,    /*                     (NEC PC98) */
            DIK_UNDERLINE = 0x93,    /*                     (NEC PC98) */
            DIK_KANJI = 0x94,    /* (Japanese keyboard)            */
            DIK_STOP = 0x95,    /*                     (NEC PC98) */
            DIK_AX = 0x96,    /*                     (Japan AX) */
            DIK_UNLABELED = 0x97,    /*                        (J3100) */
            DIK_NEXTTRACK = 0x99,    /* Next Track */
            DIK_NUMPADENTER = 0x9C,    /* Enter on numeric keypad */
            DIK_RCONTROL = 0x9D,
            DIK_MUTE = 0xA0,    /* Mute */
            DIK_CALCULATOR = 0xA1,    /* Calculator */
            DIK_PLAYPAUSE = 0xA2,    /* Play / Pause */
            DIK_MEDIASTOP = 0xA4,    /* Media Stop */
            DIK_VOLUMEDOWN = 0xAE,    /* Volume - */
            DIK_VOLUMEUP = 0xB0,    /* Volume + */
            DIK_WEBHOME = 0xB2,    /* Web home */
            DIK_NUMPADCOMMA = 0xB3,    /* , on numeric keypad (NEC PC98) */
            DIK_DIVIDE = 0xB5,    /* / on numeric keypad */
            DIK_SYSRQ = 0xB7,
            DIK_RMENU = 0xB8,    /* right Alt */
            DIK_PAUSE = 0xC5,    /* Pause */
            DIK_HOME = 0xC7,    /* Home on arrow keypad */
            DIK_UP = 0xC8,    /* UpArrow on arrow keypad */
            DIK_PRIOR = 0xC9,    /* PgUp on arrow keypad */
            DIK_LEFT = 0xCB,    /* LeftArrow on arrow keypad */
            DIK_RIGHT = 0xCD,    /* RightArrow on arrow keypad */
            DIK_END = 0xCF,    /* End on arrow keypad */
            DIK_DOWN = 0xD0,    /* DownArrow on arrow keypad */
            DIK_NEXT = 0xD1,    /* PgDn on arrow keypad */
            DIK_INSERT = 0xD2,    /* Insert on arrow keypad */
            DIK_DELETE = 0xD3,    /* Delete on arrow keypad */
            DIK_LWIN = 0xDB,    /* Left Windows key */
            DIK_RWIN = 0xDC,    /* Right Windows key */
            DIK_APPS = 0xDD,    /* AppMenu key */
            DIK_POWER = 0xDE,    /* System Power */
            DIK_SLEEP = 0xDF,    /* System Sleep */
            DIK_WAKE = 0xE3,    /* System Wake */
            DIK_WEBSEARCH = 0xE5,    /* Web Search */
            DIK_WEBFAVORITES = 0xE6,    /* Web Favorites */
            DIK_WEBREFRESH = 0xE7,    /* Web Refresh */
            DIK_WEBSTOP = 0xE8,    /* Web Stop */
            DIK_WEBFORWARD = 0xE9,    /* Web Forward */
            DIK_WEBBACK = 0xEA,    /* Web Back */
            DIK_MYCOMPUTER = 0xEB,    /* My Computer */
            DIK_MAIL = 0xEC,    /* Mail */
            DIK_MEDIASELECT = 0xED     /* Media Select */
        };

        private static KeyTableNode[] KeyTable = {
            new KeyTableNode ( (int) DIKeys.DIK_ESCAPE, (int) VKKeys.VK_ESCAPE, 27 ),
            new KeyTableNode ( (int) DIKeys.DIK_1,	'1','1' ),
            new KeyTableNode ( (int) DIKeys.DIK_2,	'2','2' ),
            new KeyTableNode ( (int) DIKeys.DIK_3,	'3','3' ),
            new KeyTableNode ( (int) DIKeys.DIK_4,	'4','4' ),
            new KeyTableNode ( (int) DIKeys.DIK_5,	'5','5' ),
            new KeyTableNode ( (int) DIKeys.DIK_6,	'6','6' ),
            new KeyTableNode ( (int) DIKeys.DIK_7,	'7','7' ),
            new KeyTableNode ( (int) DIKeys.DIK_8,	'8','8' ),
            new KeyTableNode ( (int) DIKeys.DIK_9,	'9','9' ),
            new KeyTableNode ( (int) DIKeys.DIK_0,	'0','0' ),
            new KeyTableNode ( (int) DIKeys.DIK_MINUS, (int) VKKeys.VK_OEM_MINUS, '-' ),
            new KeyTableNode ( (int) DIKeys.DIK_EQUALS, (int) VKKeys.VK_OEM_PLUS, '=' ),
            new KeyTableNode ( (int) DIKeys.DIK_BACK, (int) VKKeys.VK_BACK, 8 ),
            new KeyTableNode ( (int) DIKeys.DIK_TAB, (int) VKKeys.VK_TAB, 9 ),
            new KeyTableNode ( (int) DIKeys.DIK_Q,	'Q','Q' ),
            new KeyTableNode ( (int) DIKeys.DIK_W,	'W','W' ),
            new KeyTableNode ( (int) DIKeys.DIK_E,	'E','E' ),
            new KeyTableNode ( (int) DIKeys.DIK_R,	'R','R' ),
            new KeyTableNode ( (int) DIKeys.DIK_T,	'T','T' ),
            new KeyTableNode ( (int) DIKeys.DIK_Y,	'Y','Y' ),
            new KeyTableNode ( (int) DIKeys.DIK_U,	'U','U' ),
            new KeyTableNode ( (int) DIKeys.DIK_I,	'I','I' ),
            new KeyTableNode ( (int) DIKeys.DIK_O,	'O','O' ),
            new KeyTableNode ( (int) DIKeys.DIK_P,	'P','P' ),
            new KeyTableNode ( (int) DIKeys.DIK_LBRACKET, (int) VKKeys.VK_OEM_4, '[' ),
            new KeyTableNode ( (int) DIKeys.DIK_RBRACKET, (int) VKKeys.VK_OEM_6, ']' ),
            new KeyTableNode ( (int) DIKeys.DIK_RETURN, (int) VKKeys.VK_RETURN,  13 ),
            new KeyTableNode ( (int) DIKeys.DIK_LCONTROL, (int) VKKeys.VK_LCONTROL, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_A,	'A','A' ),
            new KeyTableNode ( (int) DIKeys.DIK_S,	'S','S' ),
            new KeyTableNode ( (int) DIKeys.DIK_D,	'D','D' ),
            new KeyTableNode ( (int) DIKeys.DIK_F,	'F','F' ),
            new KeyTableNode ( (int) DIKeys.DIK_G,	'G','G' ),
            new KeyTableNode ( (int) DIKeys.DIK_H,	'H','H' ),
            new KeyTableNode ( (int) DIKeys.DIK_J,	'J','J' ),
            new KeyTableNode ( (int) DIKeys.DIK_K,	'K','K' ),
            new KeyTableNode ( (int) DIKeys.DIK_L,	'L','L' ),
            new KeyTableNode ( (int) DIKeys.DIK_SEMICOLON, (int) VKKeys.VK_OEM_1, ';' ),
            new KeyTableNode ( (int) DIKeys.DIK_APOSTROPHE, (int) VKKeys.VK_OEM_7, '\'' ),
            new KeyTableNode ( (int) DIKeys.DIK_GRAVE, (int) VKKeys.VK_OEM_3, '`' ),
            new KeyTableNode ( (int) DIKeys.DIK_LSHIFT, (int) VKKeys.VK_LSHIFT,  0 ),
            new KeyTableNode ( (int) DIKeys.DIK_BACKSLASH, (int) VKKeys.VK_OEM_5, '\\' ),
            new KeyTableNode ( (int) DIKeys.DIK_Z,	'Z','Z' ),
            new KeyTableNode ( (int) DIKeys.DIK_X,	'X','X' ),
            new KeyTableNode ( (int) DIKeys.DIK_C,	'C','C' ),
            new KeyTableNode ( (int) DIKeys.DIK_V,	'V','V' ),
            new KeyTableNode ( (int) DIKeys.DIK_B,	'B','B' ),
            new KeyTableNode ( (int) DIKeys.DIK_N,	'N','N' ),
            new KeyTableNode ( (int) DIKeys.DIK_M,	'M','M' ),
            new KeyTableNode ( (int) DIKeys.DIK_COMMA, (int) VKKeys.VK_OEM_COMMA, ',' ),
            new KeyTableNode ( (int) DIKeys.DIK_PERIOD, (int) VKKeys.VK_OEM_PERIOD, '.' ),
            new KeyTableNode ( (int) DIKeys.DIK_SLASH, (int) VKKeys.VK_OEM_2, '/' ),
            new KeyTableNode ( (int) DIKeys.DIK_RSHIFT, (int) VKKeys.VK_RSHIFT,  0 ),
            new KeyTableNode ( (int) DIKeys.DIK_MULTIPLY, (int) VKKeys.VK_MULTIPLY, '*' ),
            new KeyTableNode ( (int) DIKeys.DIK_LMENU, (int) VKKeys.VK_LMENU, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_SPACE, (int) VKKeys.VK_SPACE, ' ' ),
            new KeyTableNode ( (int) DIKeys.DIK_CAPITAL, (int) VKKeys.VK_CAPITAL, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F1, (int) VKKeys.VK_F1, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F2, (int) VKKeys.VK_F2, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F3, (int) VKKeys.VK_F3, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F4, (int) VKKeys.VK_F4, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F5, (int) VKKeys.VK_F5, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F6, (int) VKKeys.VK_F6, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F7, (int) VKKeys.VK_F7, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F8, (int) VKKeys.VK_F8, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F9, (int) VKKeys.VK_F9, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F10, (int) VKKeys.VK_F10, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMLOCK, (int) VKKeys.VK_NUMLOCK, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_SCROLL, (int) VKKeys.VK_SCROLL, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD7, (int) VKKeys.VK_NUMPAD7, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD8, (int) VKKeys.VK_NUMPAD8, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD9, (int) VKKeys.VK_NUMPAD9, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_SUBTRACT, (int) VKKeys.VK_SUBTRACT,	0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD4, (int) VKKeys.VK_NUMPAD4, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD5, (int) VKKeys.VK_NUMPAD5, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD6, (int) VKKeys.VK_NUMPAD6, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_ADD, (int) VKKeys.VK_ADD, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD1, (int) VKKeys.VK_NUMPAD1, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD2, (int) VKKeys.VK_NUMPAD2, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD3, (int) VKKeys.VK_NUMPAD3, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPAD0, (int) VKKeys.VK_NUMPAD0, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_DECIMAL, (int) VKKeys.VK_DECIMAL, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F11, (int) VKKeys.VK_F11, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F12, (int) VKKeys.VK_F12, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F13, (int) VKKeys.VK_F13, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F14, (int) VKKeys.VK_F14, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_F15, (int) VKKeys.VK_F15, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NUMPADENTER, (int) VKKeys.VK_RETURN, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_RCONTROL, (int) VKKeys.VK_RCONTROL, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_DIVIDE,(int) VKKeys.VK_DIVIDE, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_SYSRQ, 0, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_RMENU,  (int) VKKeys.VK_RMENU, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_HOME, (int) VKKeys.VK_HOME, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_UP, (int) VKKeys.VK_UP, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_PRIOR, (int) VKKeys.VK_PRIOR, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_LEFT, (int) VKKeys.VK_LEFT, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_RIGHT, (int) VKKeys.VK_RIGHT, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_END, (int) VKKeys.VK_END, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_DOWN, (int) VKKeys.VK_DOWN, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_NEXT, (int) VKKeys.VK_NEXT, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_INSERT, (int) VKKeys.VK_INSERT, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_DELETE, (int) VKKeys.VK_DELETE, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_LWIN, (int) VKKeys.VK_LWIN, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_RWIN, (int) VKKeys.VK_RWIN, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_APPS, (int) VKKeys.VK_APPS, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_PAUSE, (int) VKKeys.VK_PAUSE, 0 ),
            new KeyTableNode ( 0, (int) VKKeys.VK_CANCEL, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_MUTE, (int) VKKeys.VK_VOLUME_MUTE, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_VOLUMEDOWN, (int) VKKeys.VK_VOLUME_DOWN, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_VOLUMEUP, (int) VKKeys.VK_VOLUME_UP, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_WEBHOME, (int) VKKeys.VK_BROWSER_HOME, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_WEBSEARCH, (int) VKKeys.VK_BROWSER_SEARCH, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_WEBFAVORITES, (int) VKKeys.VK_BROWSER_FAVORITES, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_WEBREFRESH, (int) VKKeys.VK_BROWSER_REFRESH, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_WEBSTOP, (int) VKKeys.VK_BROWSER_STOP, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_WEBFORWARD, (int) VKKeys.VK_BROWSER_FORWARD, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_WEBBACK, (int) VKKeys.VK_BROWSER_BACK, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_MAIL, (int) VKKeys.VK_LAUNCH_MAIL, 0 ),
            new KeyTableNode ( (int) DIKeys.DIK_MEDIASELECT, (int) VKKeys.VK_LAUNCH_MEDIA_SELECT, 0 )
        };

        public static int DIK2VK(int dik)
        {
            for (int i = 0; i < KeyTable.Length; i++)
                if (KeyTable[i].DIKKey == dik)
                    return KeyTable[i].VKKey;

            return -1;
        }

        public static int VK2DIK(int vk)
        {
            for (int i = 0; i < KeyTable.Length; i++)
                if (KeyTable[i].VKKey == vk)
                    return KeyTable[i].DIKKey;

            return -1;
        }
    }
}
