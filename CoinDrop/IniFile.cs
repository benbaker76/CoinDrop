using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

public partial class Win32
{
    [DllImport("kernel32")]
    public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    [DllImport("kernel32")]
    public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    [DllImport("kernel32")]
    public static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
}

public class IniFile
{
	public string path;

	public IniFile(string INIPath)
	{
		path = INIPath;
	}

	public void WriteValue(string Section, string Key, string Value)
	{
		Win32.WritePrivateProfileString(Section, Key, Value, this.path);
	}

    public void WriteValueArrayList(string Section, List<IniKey> KeyArray)
    {
        foreach (IniKey Key in KeyArray)
            WriteValue(Section, Key.Key, Key.Value);
    }

	public string ReadValue(string Section, string Key)
	{
		StringBuilder temp = new StringBuilder(256);

        int i = Win32.GetPrivateProfileString(Section, Key, "", temp, 255, this.path);

		return temp.ToString();
	}

	public string ReadValueDefault(string Section, string Key, string Default)
	{
		StringBuilder temp = new StringBuilder(256);

        int i = Win32.GetPrivateProfileString(Section, Key, "", temp, 255, this.path);

		if(i == 0)
			return Default;

		return temp.ToString();
	}
	
	public void DeleteKey(string Section, string Key)
	{
		WriteValue(Section, Key, null);
	}
	
	public void DeleteSection(string Section)
	{
		WriteValue(Section, null, null);
	}

	public string[] EnumerateSections()
	{
		byte[] buf = new byte[16384];

        int i = Win32.GetPrivateProfileString(null, null, null, buf, 16384, this.path);

		String s = System.Text.Encoding.Default.GetString(buf);

		String[] sections = s.Split('\0');

        List<string> al = new List<string>();

		foreach (String section in sections)
		{
			if (section == "")
				break;
			
			al.Add(section);
		}

		return (string[]) al.ToArray();
	}
	
	public string[] EnumerateSectionKeys(string Section)
	{
		byte[] buf = new byte[16384];

        int i = Win32.GetPrivateProfileString(Section, null, null, buf, 16384, this.path);

		String s = System.Text.Encoding.Default.GetString(buf);

		String[] keys = s.Split('\0');

        List<string> al = new List<string>();

		foreach (String key in keys)
		{
			if (key == "")
				break;

			al.Add(key);
		}

		return (string[]) al.ToArray();
	}

	public string[] EnumerateSectionValues(string Section)
	{
		int i=0;

		String[] keys = EnumerateSectionKeys(Section);
		String[] values = new String[keys.GetUpperBound(0)+1];

		foreach (String key in keys)
			values[i++] = ReadValue(Section, key);

		return values;
	}

	public List<IniKey> EnumerateSectionArrayList(string Section)
	{
		String[] keys = EnumerateSectionKeys(Section);
        List<IniKey> values = new List<IniKey>();

		foreach (String key in keys)
			values.Add(new IniKey(key, ReadValue(Section, key)));

		return values;
	}
}

public class IniKey
{
    public string Key = null;
    public string Value = null;

    public IniKey(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
