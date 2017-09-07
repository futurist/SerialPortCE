using System;
using System.Runtime.InteropServices;
using System.Text;

namespace meter
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// Usage: INIFile ini = new INIFile("C:\\test.ini");
    /// Use IniWriteValue to write a new value to a specific key in a section or use IniReadValue to read a value FROM a key in a specific Section.
    /// </summary>
    public class INIFile
    {
        public string path;

        [DllImport("IniFileDll.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);

        [DllImport("IniFileDll.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        [DllImport("IniFileDll.dll", EntryPoint = "GetPrivateProfileInt")]
        static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName,
           int nDefault, string lpFileName);


        int MAX_LENGTH = 512;

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public INIFile(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string ReadValueAsString (string Section, string Key, string defaultValue)
        {
            StringBuilder temp = new StringBuilder(MAX_LENGTH);
            int i = GetPrivateProfileString(Section, Key, defaultValue, temp,
                                            MAX_LENGTH, this.path);
            return temp.ToString();

        }

        public int ReadValueAsInt (string Section, string Key, int defaultValue)
        {
            return (int)GetPrivateProfileInt(Section, Key, defaultValue, this.path);

        }

    }
}

