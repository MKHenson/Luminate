using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Luminate
{
    /// <summary>
    /// A class for holding all general preferences
    /// </summary>
    public class SolutionPreferences
    {
        public bool useAutoComplete;
        public string browser;
        public List<string> additionalKeywords;
        public List<string> ignoreList;
        public string project;
        public bool autoBraceClosure;

        /// <summary>
        /// Simple constructor
        /// </summary>
        public SolutionPreferences()
        {
            //Set the application to be IE by default
            RegistryKey registry = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\IEXPLORE.EXE");
            object path2 = registry.GetValue("(Default)");
            string[] subkeys = registry.GetValueNames();
            string path = registry.GetValue(subkeys[0].ToString()).ToString();

            browser = path;
            useAutoComplete = true;
            autoBraceClosure = true;
            additionalKeywords = new List<string>();
            project = "";
        }

        /// <summary>
        /// Saves the project
        /// </summary>
        /// <param name="proj"></param>
        public void Save( string file )
        {
            //And write the preferences file
            try
            {
                ignoreList = Builder.ignoreList;

                TextWriter writer = new StreamWriter(file);
                XmlSerializer x = new XmlSerializer(this.GetType());
                x.Serialize(writer, this);
                writer.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Loads the preferences from an XML file
        /// </summary>
        /// <returns></returns>
        public static SolutionPreferences Load(string file)
        {
            SolutionPreferences toReturn = null; 

            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(SolutionPreferences));
                TextReader textReader = new StreamReader(file);            
                toReturn = (SolutionPreferences)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            catch
            {
                toReturn = new SolutionPreferences();
            }

           return toReturn;
        }
    }
}
