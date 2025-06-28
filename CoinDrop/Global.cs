using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.Windows.Forms;
using System.IO;

namespace CoinDrop
{
    public enum InsertCreditDetectType
    {
        Never,
        Anytime,
        WhileMAMEsRunning
    }

    class Global
    {
        public static string[] BaudRateNames =
        {
            "110",
            "300",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"
        };

        public static string[] DataBitNames =
        {
            "5",
            "6",
            "7",
            "8"
        };

        public static string[] ParityNames =
        {
            "N",
            "O",
            "E",
            "M",
            "S"
        };

        public static string[] StopBitNames =
        {
            "0",
            "1",
            "2",
            "1.5"
        };

        public static string[] HandshakeNames =
        {
            "None",
            "XON/XOFF",
            "RTS",
            "RTS & XON/XOFF"
        };

        public static string[] LPTNames =
        {
            "LPT1",
            "LPT2"
        };

        public static bool Mode64 = false;

        public static frmMain MainForm = null;

        public static KeyboardHook KeyboardHook = null;
        public static InputHook InputHook = null;
        public static MameManager MameManager = null;
        public static SoundPlayer SoundPlayer = null;

        public static void PlaySound(string fileName, bool playSync)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    SoundPlayer.SoundLocation = fileName;
                    SoundPlayer.Load();

                    if (playSync)
                        SoundPlayer.PlaySync();
                    else
                        SoundPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteEntry("PlaySound", "Global", ex.Message, ex.StackTrace);
            }
        }
    }

    class Settings
    {
        public class General
        {
            public static bool Minimized = false;
            public static int CreditCount = 0;
            public static bool RunOnStartup = false;
            public static int StartCreditCount = 0;
            public static bool CoinDropSoundEnable = true;
            public static string CoinDropSound = null;
            public static bool SkipDisclaimerEnable = false;
            public static int SkipDisclaimerDelay = 5;
            public static bool PasswordProtectEnable = false;
            public static string PasswordProtect = null;
            public static bool SoundEventsEnable = true;
        }

        public class Management
        {
            public static bool LimitGameTimeEnable = false;
            public static int LimitGameTime = 30;
            public static bool LimitGameCreditsEnable = false;
            public static int LimitGameCredits = 5;
            public static InsertCreditDetectType InsertCreditDetect = InsertCreditDetectType.Anytime;
            public static bool IgnoreExtraInserts = false;
            public static bool LimitMaximumCreditsEnable = false;
            public static int LimitMaximumCredits = 30;
            public static bool InsertCreditTimerEnable = false;
            public static int InsertCreditTimerMinutes = 30;
            public static int InsertCreditTimer = 5;
        }

        public class SoundEvents
        {
            public static string FreeCredits = null;
            public static string FiveMinutesRemaining = null;
            public static string MaximumCreditLimit = null;
            public static string GameCreditLimit = null;
        }

        public class Input
        {
            public static uint[] StartKeys = { (uint)Keys.D1, (uint)Keys.D2, (uint)Keys.D3, (uint)Keys.D4 };
            public static uint[] CoinKeys = { (uint)Keys.D5, (uint)Keys.D6, (uint)Keys.D7, (uint)Keys.D8 };
        }

        public class Folder
        {
            public static string App = null;
            public static string Samples = null;
        }

        public class FileName
        {
            public static string Log = null;
            public static string Ini = null;
        }
    }
}
