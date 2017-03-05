using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

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
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);
            string[] strArray = new string[0];
            UInt32 uiNumCharCopied = 0;

            uiNumCharCopied = GetPrivateProfileSection(section, pBuffer, 32767, path);
            
            int iStartAddress = pBuffer.ToInt32();
            int iEndAddress = iStartAddress + (int)uiNumCharCopied;

            while (iStartAddress < iEndAddress)
            {
                int iArrayCurrentSize = strArray.Length;
                
                Array.Resize(ref strArray, iArrayCurrentSize + 1);
                
                string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                
                strArray[iArrayCurrentSize] = strCurrent;
                
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

        public IEnumerable<string> GetSectionNames()
        {
            string ini = File.ReadAllText(path);

            Regex pattern = new Regex(@"\[[^\]]+\]");

            IEnumerator matchEnumerator = pattern.Matches(ini).GetEnumerator();

            while (matchEnumerator.MoveNext())
                yield return matchEnumerator.Current.ToString();
        }
    }
}
