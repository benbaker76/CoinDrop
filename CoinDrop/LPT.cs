using System;
using System.Runtime.InteropServices;

class LPT
{
    public enum LPTAddress
    {
        LPT1 = 0x378,
        LPT2 = 0x278
    };

	[DllImport("inpout32.dll")]
	public static extern void Out32(int address, int value);

	[DllImport("inpout32.dll")]
	public static extern int Inp32(int address);
}
