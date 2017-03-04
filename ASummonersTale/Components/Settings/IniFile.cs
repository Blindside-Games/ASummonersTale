using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ASummonersTale.Components.Settings
{
    public class IniFile
    {
        private string path;
        private string executableName;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern UInt32 GetPrivateProfileSection
           (
               [In] [MarshalAs(UnmanagedType.LPStr)] string strSectionName,
               // Note that because the key/value pars are returned as null-terminated
               // strings with the last string followed by 2 null-characters, we cannot
               // use StringBuilder.
               [In] IntPtr pReturnedString,
               [In] UInt32 nSize,
               [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
           );

        public IniFile(string iniPath = null)
        {
            executableName = Assembly.GetExecutingAssembly().GetName().Name;

            path = new FileInfo(iniPath ?? $"{executableName}.ini").FullName;

            if (!File.Exists(path))
                File.Create(path);
        }

        public string Read(string key, string section = null)
        {
            var RetVal = new StringBuilder(255);

            GetPrivateProfileString(section ?? executableName, key, "", RetVal, RetVal.Capacity, path);

            return RetVal.ToString();
        }

        public void Write(string key, string value, string section = null)
        {
            WritePrivateProfileString(section ?? executableName, key, value, path);
        }

        public void DeleteKey(string key, string section = null)
        {
            Write(key, null, section ?? executableName);
        }

        public void DeleteSection(string section = null)
        {
            Write(null, null, section ?? executableName);
        }


        private string[] GetAllKeysInSection(string section)
        {
            // Allocate in unmanaged memory a buffer of suitable size.
            // I have specified here the max size of 32767 as documentated
            // in MSDN.
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);
            // Start with an array of 1 string only.
            // Will embellish as we go along.
            string[] strArray = new string[0];
            UInt32 uiNumCharCopied = 0;

            uiNumCharCopied = GetPrivateProfileSection(section, pBuffer, 32767, path);

            // iStartAddress will point to the first character of the buffer,
            int iStartAddress = pBuffer.ToInt32();
            // iEndAddress will point to the last null char in the buffer.
            int iEndAddress = iStartAddress + (int)uiNumCharCopied;

            // Navigate through pBuffer.
            while (iStartAddress < iEndAddress)
            {
                // Determine the current size of the array.
                int iArrayCurrentSize = strArray.Length;
                // Increment the size of the string array by 1.
                Array.Resize(ref strArray, iArrayCurrentSize + 1);
                // Get the current string which starts at "iStartAddress".
                string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                // Insert "strCurrent" into the string array.
                strArray[iArrayCurrentSize] = strCurrent;
                // Make "iStartAddress" point to the next string.
                iStartAddress += (strCurrent.Length + 1);
            }
            Marshal.FreeHGlobal(pBuffer);
            pBuffer = IntPtr.Zero;

            return strArray;
        }

        public bool KeyExists(string key, string section = null) => Read(key, section).Length > 0;

        public bool SectionExists(string section)
        {
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);

            bool result = GetPrivateProfileSection(section, pBuffer, 32767, path) > 0;

            Marshal.FreeHGlobal(pBuffer);

            return result;
        }
    }
}
