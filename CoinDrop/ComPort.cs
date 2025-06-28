using System;
using System.Runtime.InteropServices;

public class ComPort
{
	private const uint FILE_SHARE_READ = 0x1;
	private const uint FILE_SHARE_WRITE = 0x2;
	private const uint OPEN_EXISTING = 0x3;
	private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

	[StructLayout(LayoutKind.Sequential)]
	private struct SECURITY_ATTRIBUTES 
	{
		public int nLength;
		public int lpSecurityDescriptor;
		public int bInheritHandle;
	}

	[DllImport("kernel32.dll")]
	private static extern int CreateFile(
		string lpFileName,
		uint dwDesiredAccess,
		uint dwShareMode,
		uint lpSecurityAttributes,
		uint dwCreationDisposition,
		uint dwFlagsAndAttributes,
		uint hTemplateFile
		);

	[DllImport("kernel32.dll", SetLastError=true)]
	private static extern int CloseHandle(
		int hObject   // handle to object
		);

	[DllImport("winspool.drv")]
	private static extern int ConfigurePort(string pName, IntPtr hwnd, string pPortName);

	public static bool CheckPort(int Port)
	{
		int hPort;

		// String representing port
		string sPort;

		if(Port > 0)
		{
			// Note: No trailing colon (e.g. not COM1:)
			sPort = @"\\.\COM" + Port.ToString();

			// Attempt to open the port

			hPort = CreateFile(sPort, 0, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);

			// We're done, so close it
			if(hPort > 0)
				CloseHandle(hPort);

			return (hPort > 0);
		}

		return false;
	}

	public static bool ConfigPort(IntPtr hWnd, int Port)
	{
		string sPort;

		if(Port > 0)
		{
			//This API can also be used to configure
			//COM and LPTP ports on remote machines
			//and servers by passing the machine name
			//as the first parameter in the format
			// "\\servername". ByVal vbNullString or
			//ByRef 0& can be passed to configure the
			//local machine. The hwnd parameter specified
			//the window that owns the dialog - it will
			//appear modal to the specified window.
			//Returns 1 if OK is pressed, or 0 if Cancelled.
			//
			//This call does not return the values set or
			//changed in the dialog, nor does it indicate
			//whether the user pressed Apply prior to
			//pressing OK or Cancel. This is important
			//in so far as changes made and Applied are
			//set even if the dialog is cancelled.
		        
			//The port string for this call must be
			//in the format COM<portnumber>:

			sPort = "COM" + Port.ToString() + ":";

			return (ConfigurePort(null, hWnd, sPort) > 0);
		}

		return false;
	}

	public static int GetFirstAvailPort()
	{
		int Port;

		// Find first port not already in use.
		// Return either the port number if
		// Available, or zero otherwise

		for(Port=1;Port<=16;Port++)
			if(CheckPort(Port))
				return Port;

		// No useable port was found
		return 0;
	}

	public static bool OpenPort(string sPort)
	{
		int hFakePort;

		hFakePort = CreateFile(sPort, 0, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
     
		return (hFakePort != -1);
	}
}
