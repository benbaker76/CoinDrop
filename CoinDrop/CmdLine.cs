using System;
using System.Collections.Generic;
using System.Text;

namespace CoinDrop
{
    class CmdLine
    {
        public Dictionary<string, string> CmdHash = null;

        public CmdLine(string[] args)
        {
            int argCount = 0;
            CmdHash = new Dictionary<string, string>();

            while (argCount < args.Length)
            {
                if (args[argCount].StartsWith("-"))
                {
                    string Name = args[argCount].ToLower();
                    string Value = null;

                    if (argCount + 1 < args.Length)
                    {
                        if (!args[argCount + 1].StartsWith("-"))
                        {
                            Value= args[argCount + 1];
                            argCount++;
                        }
                    }

                    if(!CmdHash.ContainsKey(Name.ToLower()))
                        CmdHash.Add(Name, Value);
                }

                argCount++;
            }
        }
    }
}
