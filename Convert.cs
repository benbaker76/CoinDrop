using System;
using System.Collections.Generic;
using System.Text;

namespace CoinDrop
{
    class Convert
    {
        public static bool StrToBool(string val)
        {
            if (val == null)
                return false;

            if (val.Trim().ToLower() == "true" || val.Trim().ToLower() == "1")
                return true;
            else
                return false;
        }

        public static int StrToInt(string val)
        {
            int result = 0;

            if (Int32.TryParse(val, out result))
                return result;

            return result;
        }
    }
}
