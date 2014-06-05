using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Xml;

namespace Luminate
{
    /// <summary>
    /// Represents an Animate project
    /// </summary>
    [Serializable()]
    public class Project : ISerializable
    {
        private bool mSaved;
        private string mName;
        private string mDiectory;

        private string mFirst;
        private string mLast;

        //private string mBrowser;
        private string mOutputFile;
        private string mRunPath;
        private bool mMinify;

        private bool mUseFTP;
        private string mFTPLocation;
        private string mFTPUser;
        private string mFTPPassword;

        private string mCompiledJS;

        private List<string> mDependencies;

        private string mOverridePath;
        private string mOverrideRunable;
        public FileSystemWatcher watcher;

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        public Project(string name, string directory)
        {
            mSaved = true;
            mName = name;
            mDiectory = directory;
            mMinify = false;
            mOutputFile = directory + "\\bin\\application.js";
            mFirst = directory + "\\src\\begin.js";
            mLast = directory + "\\src\\end.js";
            mRunPath = directory + "\\bin\\index.html";
            mOverridePath = "";
            mOverrideRunable = "";
            mDependencies = new List<string>();

            mUseFTP = false;
            mFTPLocation = "";
            mFTPUser = "";
            mFTPPassword = "";
            mCompiledJS = "";

            ////Set the application to be IE by default
           // RegistryKey registry = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\IEXPLORE.EXE");
            //object path2 = registry.GetValue("(Default)");
           // string[] subkeys = registry.GetValueNames();
            //string path = registry.GetValue(subkeys[0].ToString()).ToString();
           // mBrowser = path;

            //Create a watcher for this doc
            CreateWatcher();
        }

        /// <summary>
        /// When this file changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Luminate.Singleton.UpdateFile(e.FullPath);
        }

        /// <summary>
        /// Saves the project
        /// </summary>
        /// <param name="proj"></param>
        public void Save()
        {
            CreateProjectDirectory( this );
            mSaved = true;
        }

        /// <summary>
        /// Creates a project directory save file (.lum)
        /// </summary>
        /// <param name="proj"></param>
        public static void CreateProjectDirectory( Project proj )
        {
            //Create the project file
            Stream stream = File.Open(proj.mDiectory + "\\" + proj.mName + ".lum", FileMode.Create);
            IFormatter bFormatter = new XmlFormatter(typeof(Project));
            bFormatter.Serialize(stream, proj);
            stream.Close();
        }

        /// <summary>
        /// Opens from a project save file (.lum)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Project OpenProjectDirectory(string file)
        {
            Project proj = null;
            Stream stream = File.Open(file, FileMode.Open);
            IFormatter bFormatter = new XmlFormatter(typeof(Project));

            try
            {
                proj = (Project)bFormatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show( "Failed opening XML format, trying binary - " + ex.Message, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    stream.Close();

                    bFormatter = new BinaryFormatter();
                    stream = File.Open(file, FileMode.Open);
                    proj = (Project)bFormatter.Deserialize(stream);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.Message, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            stream.Close();
            return proj;
        }

        /// <summary>
        /// Serializable writing to
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public Project(SerializationInfo info, StreamingContext ctxt)
        {
            try { mName = (string)info.GetValue("name", typeof(string)); }
            catch { mName = (string)info.GetValue("mName", typeof(string)); }
            if ( mName == null )
                mName = "";

            try { mDiectory = (string)info.GetValue("diectory", typeof(string)); }
            catch { mDiectory = (string)info.GetValue("mDiectory", typeof(string)); }
            if (mDiectory == null)
                mDiectory = "";

            //try { mBrowser = (string)info.GetValue("browser", typeof(string));}
            //catch { mBrowser = (string)info.GetValue("mBrowser", typeof(string)); }
            //if (mBrowser == null)
             //   mBrowser = "";

            try { mOutputFile = (string)info.GetValue("outputFile", typeof(string));}
            catch { mOutputFile = (string)info.GetValue("mOutputFile", typeof(string)); }
            if (mOutputFile == null)
                mOutputFile = "";

            try { mMinify = (bool)info.GetValue("minify", typeof(bool));}
            catch { mMinify = (bool)info.GetValue("mMinify", typeof(bool)); }

            try { mRunPath = (string)info.GetValue("runPath", typeof(string));}
            catch { mRunPath = (string)info.GetValue("mRunPath", typeof(string)); }
            if (mRunPath == null)
                mRunPath = "";

            try { mLast = (string)info.GetValue("last", typeof(string));}
            catch { mLast = (string)info.GetValue("mLast", typeof(string)); }
            if (mLast == null)
                mLast = "";

            try { mFirst = (string)info.GetValue("first", typeof(string));}
            catch { mFirst = (string)info.GetValue("mFirst", typeof(string)); }
            if (mFirst == null)
                mFirst = "";

            try {  mUseFTP = (bool)info.GetValue("useFTP", typeof(bool));}
            catch { mUseFTP = (bool)info.GetValue("mUseFTP", typeof(bool)); }
           

            try { mFTPLocation = (string)info.GetValue("ftpLocation", typeof(string));}
            catch { mFTPLocation = (string)info.GetValue("mFTPLocation", typeof(string)); }
            if (mFTPLocation == null)
                mFTPLocation = "";

            try { mFTPUser = (string)info.GetValue("ftpUser", typeof(string));}
            catch { mFTPUser = (string)info.GetValue("mFTPUser", typeof(string)); }
            if (mFTPUser == null)
                mFTPUser = "";

            try { mFTPPassword = (string)info.GetValue("ftpPassword", typeof(string));}
            catch { mFTPPassword = (string)info.GetValue("mFTPPassword", typeof(string)); }
            if (mFTPPassword == null)
                mFTPPassword = "";

            try { mOverridePath = (string)info.GetValue("overridePath", typeof(string));}
            catch { mOverridePath = (string)info.GetValue("mOverridePath", typeof(string)); }
            if (mOverridePath == null)
                mOverridePath = "";

            try { mOverrideRunable = (string)info.GetValue("overrideRunable", typeof(string));}
            catch { mOverrideRunable = (string)info.GetValue("mOverrideRunable", typeof(string)); }
            if (mOverrideRunable == null)
                mOverrideRunable = "";

            try { mDependencies = (List<string>)info.GetValue("mDependencies", typeof(List<string>)); }
            catch 
            { 
                mDependencies = new List<string>();
                string depStr = (string)info.GetValue("dependencies", typeof(string));
                if (depStr != null)
                {
                    string[] data = depStr.Split('|');
                    foreach (string s in data)
                        if ( s != "" )
                            mDependencies.Add(s);
                }
            }

            if (mDependencies == null)
                mDependencies = new List<string>();

            //Load watcher if none exists
            if (watcher == null)
                CreateWatcher();
               
        }

        /// <summary>
        /// Creates the directory watcher
        /// </summary>
        private void CreateWatcher()
        {
            //Create a watcher for this doc
            // Create a new FileSystemWatcher and set its properties.
            if (!Directory.Exists(directory))
              throw new Exception("The project directory " + directory + " does not exist. You can fix this by opening the APN file and changing the directory variable.");

            watcher = new FileSystemWatcher(directory);

            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.IncludeSubdirectories = true;

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            watcher.Created += new FileSystemEventHandler(OnFileChanged);
            watcher.Deleted += new FileSystemEventHandler(OnFileChanged);
            watcher.Renamed += new RenamedEventHandler(OnFileChanged);            
        }
        /// <summary>
        /// Serializable opening from
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("name", mName);
            info.AddValue("diectory", mDiectory);
            //info.AddValue("browser", mBrowser);
            info.AddValue("outputFile", mOutputFile);
            info.AddValue("minify", mMinify);
            info.AddValue("runPath", mRunPath);
            info.AddValue("last", mLast);
            info.AddValue("first", mFirst);
            info.AddValue("useFTP", mUseFTP);
            info.AddValue("ftpLocation", mFTPLocation);
            info.AddValue("ftpUser", mFTPUser);
            info.AddValue("ftpPassword", mFTPPassword);            
            info.AddValue("overridePath", mOverridePath);
            info.AddValue("overrideRunable", mOverrideRunable);

            string dependencies = "";
            foreach (string s in mDependencies)
                dependencies += s + "|";
            info.AddValue("dependencies", dependencies);
        }

        /// <summary>
        /// Gets/Sets the overriden path of the output file
        /// </summary>
        public string OverridePath
        {
            get { return mOverridePath; }
            set { mOverridePath = value; mSaved = false; }
        }

        /// <summary>
        /// Gets/Sets the overriden path of the runnable
        /// </summary>
        public string OverrideRunable
        {
            get { return mOverrideRunable; }
            set { mOverrideRunable = value; mSaved = false; }
        }

        /// <summary>
        /// Gets the dependency list for this project
        /// </summary>
        public List<string> Dependencies
        {
            get { return mDependencies; }
        }

        /// <summary>
        /// Gets/Sets if the FTP upload option is used
        /// </summary>
        public bool UseFTP
        {
            get { return mUseFTP; }
            set { mUseFTP = value; mSaved = false; }
        }

        /// <summary>
        /// Gets/Sets the upload location for FTP
        /// </summary>
        public string FTPLocation
        {
            get { return mFTPLocation; }
            set { mFTPLocation = value; mSaved = false; }
        }

        /// <summary>
        /// Gets/Sets the usename for the FTP
        /// </summary>
        public string FTPUser
        {
            get { return mFTPUser; }
            set { mFTPUser = value; mSaved = false; }
        }

        /// <summary>
        /// Gets/Sets the password for the FTP
        /// </summary>
        public string FTPPassword
        {
            get { return mFTPPassword; }
            set { mFTPPassword = value; mSaved = false; }
        }

        /// <summary>
        /// Gets/Sets the name of the project
        /// </summary>
        public string Name 
        { 
            get { return mName; }
            set { if (value == null) return;  mName = value; mSaved = false; }
        }

        /// <summary>
        /// Gets the directory of the project
        /// </summary>
        public string directory
        {
            get { return mDiectory; }
            set { mDiectory = value; mSaved = false; }
        }

        /// <summary>
        /// Gets/Sets the if the project should minify.
        /// </summary>
        public bool Minify
        {
            get { return mMinify; }
            set { mMinify = value; mSaved = false; }
        }

        /// <summary>
        /// Gets if the project has been saved
        /// </summary>
        public bool Saved
        {
            get { return mSaved; }
        }

        /// <summary>
        /// Gets/Sets the file that will act as the first file to be added in the merge process
        /// </summary>
        public string First
        {
            get { return mFirst; }
            set { mFirst = value; mSaved = false; }
        }

        /// <summary>
        /// Gets/Sets the last file that will be appended to the merged file
        /// </summary>
        public string Last
        {
            get { return mLast; }
            set { mLast = value; mSaved = false; }
        }

        ///// <summary>
        ///// Gets/Sets the the project browser.
        ///// </summary>
        //public string Browser
        //{
        //    get { return mBrowser; }
        //    set { mBrowser = value; mSaved = false; }
        //}

        /// <summary>
        /// Gets/Sets the the project output file.
        /// </summary>
        public string OutputFile
        {
            get { return mOutputFile; }
            set { mOutputFile = value; mSaved = false; }
        }

        /// <summary>
        /// Gets/Sets the the project file that will be run when debuggin.
        /// </summary>
        public string RunPath
        {
            get { return mRunPath; }
            set { mRunPath = value; mSaved = false; }
        }
        

        /// <summary>
        /// Destroys the project
        /// </summary>
        public void Dispose()
        {
            mDiectory = null;
            mName = null;

            watcher.Changed -= new FileSystemEventHandler(OnFileChanged);
            watcher.Created -= new FileSystemEventHandler(OnFileChanged);
            watcher.Deleted -= new FileSystemEventHandler(OnFileChanged);
            watcher.Renamed -= new RenamedEventHandler(OnFileChanged);
            watcher = null;
        }

        /// <summary>
        /// Gets/Sets the compiled JS of the project
        /// </summary>
        public string CompiledJS
        {
            get { return mCompiledJS; }
            set { mCompiledJS = value;}
        }
    }
}
