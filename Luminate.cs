using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ScintillaNET;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using System.Threading;

namespace Luminate
{
    /// <summary>
    /// The main form which holds all dockable sub forms
    /// </summary>
    public partial class Luminate : Form
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
       delegate void enableBuild(bool enabled);
       delegate void setProgress(int value);
         

        private static Luminate mSingleton;
        private NewProject mNewProj;
        private List<Project> mProjects;
        private List<Document> mDocs;
        private About mAbout;
        private FTP_Options mFTPOptions;
        private BuildOptions mBuildOptions;
        private SolutionPreferences mPrefs;
        private Preferences mPrefForm;
        private Preview mPrevForm;
        private bool mListenForChanges;
        private DocumentLexer mLexer;

        /// <summary>
        /// Simple constructor
        /// </summary>
        public Luminate(string[] data)
        {
            InitializeComponent();
            mSingleton = this;

            mListenForChanges = true;
            mProjects = new List<Project>();
            mDocs = new List<Document>();
            mAbout = new About();
            mFTPOptions = new FTP_Options();
            mBuildOptions = new BuildOptions();
            mPrefForm = new Preferences();
            mPrevForm = new Preview();
            mLexer = new DocumentLexer();

            SolutionExplorer explorer = SolutionExplorer.Singleton;
            Logger logger = Logger.Singleton;

            //Only show the files if we dont have a layout
            if (!File.Exists(Application.StartupPath + "\\layout.lumf"))
            {
                explorer.Show(mDockPanel);
                logger.Show(mDockPanel);
                mPrevForm.Show(mDockPanel);

                explorer.DockTo(mDockPanel, DockStyle.Right);
                logger.DockTo(mDockPanel, DockStyle.Bottom);
                mPrevForm.DockTo(mDockPanel, DockStyle.Fill);
                
            }

            if (File.Exists(Application.StartupPath + "\\preferences.lumf"))
                mPrefs = SolutionPreferences.Load(Application.StartupPath + "\\preferences.lumf");
            else
                mPrefs = new SolutionPreferences();


            Builder.ignoreList = mPrefs.ignoreList;

            //Set the browser
            textBrowser.Tag = mPrefs.browser;
            string[] split = mPrefs.browser.Split('\\');
            textBrowser.Text = split[ split.Length - 1 ];

            mNewProj = new NewProject();

            buttonBuild.Image = imageList.Images[0];
            buttonRun.Image = imageList.Images[1];
            buttonMinify.Image = imageList.Images[3];
            buttonBrowserDiag.Image = imageList.Images[4];

            //Event handlers 
            explorer.ProjectSelected += new SolutionExplorer.projectSelected(OnProjectSelected);

            OnProjectSelected(null);

            // Open the general project settings - see if there are any projects already loaded
            try
            {
                // Example #2
                // Read each line of the file into a string array. Each element
                // of the array is one line of the file.
                string[] lines = File.ReadAllLines(Application.StartupPath + "\\startup.options");

                foreach (string projectLocation in lines)
                {
                    if ( projectLocation[0] == '*' )
                        AddCloseNode( projectLocation.Remove(0,1) );
                    else
                        OpenProject(projectLocation);
                }
            }
            catch
            {
            }

            //If arguments are passed to the app then try and open the file if its a lum file
            if (data != null && data.Length > 0)
            {
                
                if (data[0].Split('.')[1].ToString().ToLower() == "lum")
                    OpenProject(data[0]);
            }

            
        }

        /// <summary>
        /// Gets the class lexer. This is used to for auto-compete and help doc  generation. This will only return values after a build.
        /// </summary>
        public DocumentLexer lexer { get { return mLexer; } }


        /// <summary>
        /// Turns the watchers on and off
        /// </summary>
        public bool listenForChanges
        {
            get { return mListenForChanges; }
            set
            {
                foreach (Project p in mProjects)
                    p.watcher.EnableRaisingEvents = value;

                mListenForChanges = value;
            }
        }


        /// <summary>
        /// Gets the preferences for the project
        /// </summary>
        public SolutionPreferences Preferences { get { return mPrefs; } }

        /// <summary>
        /// When a file changes we need to update it.
        /// </summary>
        public void UpdateFile(string file)
        {
            if (mListenForChanges == false)
                return;

            foreach (Document doc in mDocs)
                if (doc.file == file)
                    doc.UpdateOnActive = true;
        }

        /// <summary>
        /// When the form loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormLoad(object sender, System.EventArgs e)
        {
            try
            {
                //Open existing layout
                if (File.Exists(Application.StartupPath + "\\layout.lumf"))
                {
                    // In order to load layout from XML, we need to close all the DockContents
                    CloseAllContents();

                    DeserializeDockContent m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
                    mDockPanel.LoadFromXml(Application.StartupPath + "\\layout.lumf", m_deserializeDockContent);                
                    mDockPanel.ResumeLayout(true, true);
                }
            }
            catch
            {
            }
        }

        private void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            SolutionExplorer.Singleton.DockPanel = null;
            Logger.Singleton.DockPanel = null;
        }

        /// <summary>
        /// Load the layout
        /// </summary>
        /// <param name="persistString"></param>
        /// <returns></returns>
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(SolutionExplorer).ToString())
                return SolutionExplorer.Singleton;
            else if (persistString == typeof(Logger).ToString())
                return Logger.Singleton;
            else if (persistString == typeof(Preview).ToString())
                return mPrevForm;
            else
            {
                // Document overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.

                string[] parsedStrings = persistString.Split( new string[] {"|||DOCKSPLIT|||"}, StringSplitOptions.None );
                if (parsedStrings.Length != 2)
                   return null;

                Document doc = new Document(parsedStrings[0], null);
                mDocs.Add(doc);
                return doc;
            }
        }

        /// <summary>
        /// When we select projects we change settings
        /// </summary>
        /// <param name="proj"></param>
        void OnProjectSelected(Project proj)
        {
            if (proj == null)
            {
                buttonMinify.Enabled = false;
                textBrowser.Enabled = false;
                buttonBrowserDiag.Enabled = false;
                buttonBuild.Enabled = false;
                buttonRun.Enabled = false;
                return;
            }
            else
            {
                buttonMinify.Enabled = true;
                textBrowser.Enabled = true;
                buttonBrowserDiag.Enabled = true;
                buttonBuild.Enabled = true;
                buttonRun.Enabled = true;
            }

            if (proj.Minify)
            {
                onToolStripMenuItem.Checked = true;
                offToolStripMenuItem.Checked = false;
                buttonMinify.Image = imageList.Images[2];
            }
            else
            {
                onToolStripMenuItem.Checked = false;
                offToolStripMenuItem.Checked = true;
                buttonMinify.Image = imageList.Images[3];
            }

            

            //textBrowser.Tag = proj.Browser;

            //string[] split = proj.Browser.Split('\\');
            //textBrowser.Text = split[ split.Length - 1 ];
        }

        /// <summary>
        /// When we click exit we exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitClicked(object sender, EventArgs e)
        {
            // Save the general project settings
            try
            {
                //Write 
                SaveLayout();
            }
            catch
            {
            }

            //Application.Exit();
            Close();
        }

        /// <summary>
        /// When we click new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewProjClicked(object sender, EventArgs e)
        {
            if (mNewProj.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                //Create the project
                Project proj = new Project(mNewProj.ProjecName, mNewProj.ProjecDirectory);
                mProjects.Add(proj);

                //Check if a valid project
                Project.CreateProjectDirectory(proj);           

                //Add project to solution explorer
                SolutionExplorer.Singleton.AddProject(proj);
            }

            BringToFront();
        }

        /// <summary>
        /// Deletes and removes a project 
        /// </summary>
        public void DeleteProject( Project proj, bool deleteFolder = true )
        {
            //Delete all files and folders
            try
            {
                if ( deleteFolder )
                    if (Directory.Exists(proj.directory))
                        Directory.Delete(proj.directory, true);
            }
            catch (Exception ex)
            {
                Logger.Singleton.Log("Not all directories successfully deleted." + ex.Message, Logger.MessageType.ERROR);
            }

            mProjects.Remove(proj);
            proj.Dispose();
        }
        
        /// <summary>
        /// Renames a file
        /// </summary>
        /// <param name="file"></param>
        public void FileRenamed(string oldFilePath, string newFilePath)
        {
            foreach ( Document doc in mDocs )
                if (doc.file == oldFilePath)
                {
                    doc.file = newFilePath;

                    string[] split = newFilePath.Split('\\');
                    doc.Text = (doc.Saved ? "" : "*") + split[split.Length - 1];

                    return;
                }
        }

        /// <summary>
        /// Renames a file
        /// </summary>
        /// <param name="file"></param>
        public void RemoveFile(string path, bool forceRemove = false)
        {
            foreach (Document doc in mDocs)
                if (doc.file == path)
                {
                    if (forceRemove == false && doc.Saved == false)
                    {
                        DialogResult message = MessageBox.Show(path + " has not been saved, do you want to save it before closing?", "Save file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (message == System.Windows.Forms.DialogResult.Yes)
                            doc.Save();
                    }

                    mDocs.Remove(doc);
                    doc.forceRemove = forceRemove;
                    doc.Close();
                    return;
                }
        }

        /// <summary>
        /// Closes all files
        /// </summary>
        /// <param name="file"></param>
        public void RemoveAll(string except)
        {
            List<Document> docs = new List<Document>();
            docs.AddRange(mDocs.ToArray());

            foreach (Document doc in docs)
                if (doc.file != except)
                    RemoveFile(doc.file);
        }


        /// <summary>
        /// When we click open 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpenClicked(object sender, EventArgs e)
        {
            if (mOpenDiag.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                BringToFront();
                string projectName = mOpenDiag.SafeFileName.Split('.')[0];

                //Check if project is already open
                foreach ( Project p in mProjects )
                    if (p.Name == projectName)
                    {
                        MessageBox.Show("A project with that name is already open", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                string[] fileNameParts = mOpenDiag.FileName.Split('.');
                if (fileNameParts[fileNameParts.Length - 1].ToString().ToLower() == "lum")
                {
                    OpenProject(mOpenDiag.FileName);
                }
            }
        }

        /// <summary>
        /// Adds a closed project reference
        /// </summary>
        /// <param name="path"></param>
        public void AddCloseNode(string path)
        {
            string[] parts = path.Split('\\');
            string name = parts[parts.Length - 1];
            parts = name.Split('.');
            name = parts[0];
            TreeNodeClosed closed = new TreeNodeClosed(name);
            closed.Tag = path;
            SolutionExplorer.Singleton.tree.Nodes.Add(closed);
        }

        /// <summary>
        /// Opens a project
        /// </summary>
        /// <param name="path"></param>
        public void OpenProject(string path)
        {
            mListenForChanges = false;

            //Check if a valid project
            Project proj = Project.OpenProjectDirectory(path);
            if (proj == null)
                return;

            //Check if project is already open
            foreach (Project p in mProjects)
                if (p.Name == proj.Name)
                    return;

            mProjects.Add(proj);

            string[] parts = path.Split('\\');
            string combined = "";
            for (int i = 0; i < parts.Length - 1; i++)
                combined += parts[i] + '\\';

            combined = combined.Substring(0, combined.Length - 1);

            proj.directory = combined;

            //Add project to solution explorer
            SolutionExplorer.Singleton.AddProject(proj);

            proj.Save();

            //Log message
            Logger.Singleton.Log("Opened project " + proj.Name, Logger.MessageType.MESSAGE);

            mListenForChanges = true;
        }

        /// <summary>
        /// Gets the singleton reference
        /// </summary>
        public static Luminate Singleton { get { return mSingleton; } }

        /// <summary>
        /// Creates a document
        /// </summary>
        /// <param name="content"></param>
        public Document CreateDocument( string content, string fileLoc )
        {
            foreach (Document doc in mDocs)
                if (doc.file == fileLoc)
                {
                    doc.Show(mDockPanel);
                    return doc;
                }

            Document newDoc = new Document(fileLoc, content);

            newDoc.Show(mDockPanel);
            mDocs.Add(newDoc);
            return newDoc;
        }
        
        /// <summary>
        /// When we click save all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveAllClick(object sender, EventArgs e)
        {
            foreach (Document doc in mDocs)
                if (!doc.Saved)
                    doc.Save();
        }

        /// <summary>
        /// When we click save on the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveClick(object sender, EventArgs e)
        {
            mListenForChanges = false;

            foreach ( Document doc in mDocs )
                if (doc.IsActivated && !doc.Saved)
                {
                    doc.Save();
                    mListenForChanges = true;
                    return;
                }

            mListenForChanges = true;
        }

        /// <summary>
        /// When the file opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFileOpening(object sender, EventArgs e)
        {
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;

            if (SolutionExplorer.Singleton.ActiveProject != null)
                    saveProjectToolStripMenuItem.Enabled = true;

            foreach ( Document doc in mDocs )
            {
                if (doc.IsActivated && !doc.Saved)
                {
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    break;
                }

                if (!doc.Saved)
                    saveAsToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// Checks or unchecks the minify option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMinifyClicked(object sender, EventArgs e)
        {
            if (sender == onToolStripMenuItem)
                offToolStripMenuItem.Checked = false;
            else
                onToolStripMenuItem.Checked = false;

            //Set the minify
            if (SolutionExplorer.Singleton.ActiveProject != null)
                SolutionExplorer.Singleton.ActiveProject.Minify = onToolStripMenuItem.Checked;

            if (onToolStripMenuItem.Checked)
                buttonMinify.Image = imageList.Images[2];
            else
                buttonMinify.Image = imageList.Images[3];
        }

        /// <summary>
        /// Center to screen
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToScreen();
            BringToFront();
        }

        /// <summary>
        /// Shows the browser dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowBrowserDiag(object sender, EventArgs e)
        {
            if ( openFileBrowser.ShowDialog(this) == System.Windows.Forms.DialogResult.OK )
            {
                BringToFront();
                textBrowser.Tag = openFileBrowser.FileName;
                textBrowser.Text = openFileBrowser.SafeFileName;

                //Set the browser
                mPrefs.browser = textBrowser.Tag.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveProjectClicked(object sender, EventArgs e)
        {
            if (SolutionExplorer.Singleton.ActiveProject != null)
                SolutionExplorer.Singleton.ActiveProject.Save();
        }

        /// <summary>
        /// When we click run we start the browser and pass in the startup path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRunClick(object sender, EventArgs e)
        {
            Project proj = null;

            if (mPrefs.project != null && mPrefs.project != "")
                foreach (Project p in mProjects)
                    if (p.Name == mPrefs.project)
                    {
                        proj = p;
                        break;
                    }

            if (proj == null)
                proj = SolutionExplorer.Singleton.ActiveProject;
            
            if (!File.Exists(textBrowser.Tag.ToString()) )
            {
                MessageBox.Show("You have not selected a browser or the path: " + textBrowser.Tag.ToString() + " does not exist", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if (proj == null)
            {
                MessageBox.Show("Please select a project to run", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Save all open docs
            saveAsToolStripMenuItem.PerformClick();

            //Fisrt build the project
            buttonBuild.PerformClick(); ;

            try
            {
                //Try and run the process
                System.Diagnostics.Process.Start(textBrowser.Tag.ToString(),
                    (proj.OverrideRunable.Trim() == "" ? "\"" + proj.RunPath + "\"" : "\"" + proj.OverrideRunable + "\""));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                return;
            }
        }

        /// <summary>
        /// Show the about form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAboutClick(object sender, EventArgs e)
        {
            mAbout.Show(this);
        }

        public void SaveLayout()
        {
            mDockPanel.SaveAsXml(Application.StartupPath + "\\layout.lumf");
        }

        /// <summary>
        /// Check if any projects need to be saved
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            foreach ( Project p in mProjects )
                if (p.Saved == false)
                {
                    DialogResult result = MessageBox.Show("Save changes to " + p.Name + "?", "Save?",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                        p.Save();

                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

            e.Cancel = false;

            // Save the general project settings
            try
            {
                StreamWriter file = new StreamWriter(Application.StartupPath + "\\startup.options");
                foreach (Project p in mProjects)
                    file.WriteLine(p.directory + "\\" + p.Name + ".lum");

                //Save all closed references
                foreach (TreeNode n in SolutionExplorer.Singleton.tree.Nodes )
                    if ( n is TreeNodeClosed )
                        file.WriteLine("*" + n.Tag.ToString());

                file.Close();

                mPrefs.Save(Application.StartupPath + "\\preferences.lumf");

                SaveLayout();
            }
            catch
            {
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// use this function to build a specific project
        /// </summary>
        public void BuildProject(Project proj)
        {
            mListenForChanges = false;

            if (proj != null)
            {
                EnableBuild(false);

                Logger.Singleton.Clear();
                ThreadStart starter = delegate { ThreadedBuild(proj); };
                Thread thread = new Thread(starter);
                thread.Start();
            }

            mPrevForm.LoadURL((proj.OverrideRunable.Trim() == "" ? proj.RunPath : proj.OverrideRunable));

            mListenForChanges = true;
        }

         /// <summary>
        /// use this function to enable or disable the ability to build
        /// </summary>
        public void EnableBuild(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.toolStrip.InvokeRequired)
            {
                enableBuild func = new enableBuild(EnableBuild);
                this.Invoke(func, new object[] { enabled });
            }
            else
            {
                if (buildProgress.IsDisposed)
                    return;
                buildProgress.ProgressBar.Value = 0;
                buttonBuild.Enabled = enabled;
                buildProgress.Visible = !enabled;
                buttonRun.Enabled = enabled;
            }
        }

        /// <summary>
        /// use this function to enable or disable the ability to build
        /// </summary>
        public void ProgressPercent(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.toolStrip.InvokeRequired)
            {
                setProgress func = new setProgress(ProgressPercent);
                this.Invoke(func, new object[] { value });
            }
            else
            {
                buildProgress.ProgressBar.Value = value;
            }
        }


        private static void ThreadedBuild(Project proj)
        {
            Luminate luminate = Luminate.Singleton;

            DateTime date = DateTime.Now;

            string catenatedBuilds = "";

            foreach (string s in proj.Dependencies)
            {
                bool foundProject = false;

                for (var i = 0; i < luminate.mProjects.Count; i++ )
                    if (luminate.mProjects[i].Name == s)
                    {
                        foundProject = true;
                        Logger.Singleton.Log("Building " + luminate.mProjects[i].Name + " on " + date.ToShortDateString() + " @" + date.ToLongTimeString(), Logger.MessageType.MESSAGE);
                        Builder.Build(luminate.mProjects[i], null);
                        Logger.Singleton.Log("Build " + luminate.mProjects[i].Name + " Complete: " + proj.OutputFile + " on " + date.ToShortDateString() + " @" + date.ToLongTimeString(), Logger.MessageType.MESSAGE);

                        catenatedBuilds += luminate.mProjects[i].CompiledJS;
                        break;
                    }

                if (!foundProject)
                    Logger.Singleton.Log("Project Dependency " + s + " was not found for " + proj.Name + ".", Logger.MessageType.ERROR);
            }

            Logger.Singleton.Log("Building " + proj.Name + " on " + date.ToShortDateString() + " @" + date.ToLongTimeString(), Logger.MessageType.MESSAGE);
            Builder.Build(proj, catenatedBuilds);
            Logger.Singleton.Log("Build " + proj.Name + " Complete: " +
                proj.OutputFile + " on " + date.ToShortDateString() + " @" + date.ToLongTimeString(), Logger.MessageType.MESSAGE);

            if (proj.UseFTP)
                Logger.Singleton.Log("Uploading to " + proj.FTPLocation + " with user " +
                    proj.FTPUser + "...", Logger.MessageType.MESSAGE);

            luminate.mLexer.parse(proj.CompiledJS);

            // Save to the override file
            try
            {
                if (proj.OverridePath.Trim() != "")
                {
                    StreamWriter file = new StreamWriter(proj.OverridePath);
                    file.Write(proj.CompiledJS);
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
            }

            luminate.EnableBuild(true);
        }

        /// <summary>
        /// When we click the build button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBuildClick(object sender, EventArgs e)
        {
            Project proj = null;

            if (mPrefs.project != null && mPrefs.project != "" )
                foreach ( Project p in mProjects )
                    if (p.Name == mPrefs.project)
                    {
                        proj = p;
                        break;
                    }

            if ( proj == null )
                proj = SolutionExplorer.Singleton.ActiveProject;

          
            BuildProject(proj);
        }

        /// <summary>
        /// Occurs when we click on the FTP options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFTPOptionsClick(object sender, EventArgs e)
        {
            mFTPOptions.ShowDialog();
            BringToFront();
        }

        /// <summary>
        /// On preferences opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPrefsOpening(object sender, EventArgs e)
        {
            if (SolutionExplorer.Singleton.ActiveProject == null)
            {
                fTPOptionsToolStripMenuItem.Enabled = false;
                optionsToolStripMenuItem1.Enabled = false;
            }
            else
            {
                fTPOptionsToolStripMenuItem.Enabled = true;
                optionsToolStripMenuItem1.Enabled = true;
            }

            solutionExplorerToolStripMenuItem.Checked = SolutionExplorer.Singleton.Visible;
            loggerToolStripMenuItem.Checked = Logger.Singleton.Visible;
            previewToolStripMenuItem.Checked = mPrevForm.Visible;
        }


        /// <summary>
        /// Returns the project list
        /// </summary>
        public List<Project> Projects { get { return mProjects; } }

        /// <summary>
        /// When we click build options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBuildOptionsClick(object sender, EventArgs e)
        {
            mBuildOptions.ShowDialog(this);
            BringToFront();
        }

        /// <summary>
        /// When edit is opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditOpening(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
            cutToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Enabled = false;
            pasteToolStripMenuItem.Enabled = false;
            findToolStripMenuItem.Enabled = false;
            
            foreach ( Document d in mDocs )
                if (d.IsActivated)
                {
                    cutToolStripMenuItem.Enabled = true;
                    copyToolStripMenuItem.Enabled = true;
                    findToolStripMenuItem.Enabled = true;
                        
                    if ( d.Scintilla.UndoRedo.CanRedo )
                        redoToolStripMenuItem.Enabled = true;
                    if ( d.Scintilla.UndoRedo.CanUndo )
                        undoToolStripMenuItem.Enabled = true;
                    if ( System.Windows.Forms.Clipboard.ContainsText() )
                        pasteToolStripMenuItem.Enabled = true;
                }
        }

        /// <summary>
        /// When we click paste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPasteClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.DoCommand(BindableCommand.Paste);
        }

        /// <summary>
        /// When we click cut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.DoCommand(BindableCommand.Cut);
        }

        /// <summary>
        /// When we click copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.DoCommand(BindableCommand.Copy);
        }

        /// <summary>
        /// When we click find
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFindClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.ShowFind();
        }

        /// <summary>
        /// When we click undo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUndoClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.DoCommand(BindableCommand.Undo);
        }

        /// <summary>
        /// When we click redo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRedoClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.DoCommand(BindableCommand.Redo);
        }

        /// <summary>
        /// Tells run to be executed
        /// </summary>
        public void CallRun()
        {
            buttonRun.PerformClick();
        }

        /// <summary>
        /// Tells run to be executed
        /// </summary>
        public void CallBuild()
        {
            saveAsToolStripMenuItem.PerformClick();
            buttonBuild.PerformClick();
        }

        /// <summary>
        /// Tells run to be executed
        /// </summary>
        public void CallBuildOptions()
        {
            optionsToolStripMenuItem1.PerformClick();
        }

        /// <summary>
        /// When we click a window visibility menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowVisibilityChecked(object sender, EventArgs e)
        {
            if (sender == solutionExplorerToolStripMenuItem)
                if (!solutionExplorerToolStripMenuItem.Checked)
                    SolutionExplorer.Singleton.Hide();
                else
                    SolutionExplorer.Singleton.Show(mDockPanel);
            else if (sender == previewToolStripMenuItem)
                if (!previewToolStripMenuItem.Checked)
                    mPrevForm.Hide();
                else
                    mPrevForm.Show(mDockPanel);
            else
                if (!loggerToolStripMenuItem.Checked)
                    Logger.Singleton.Hide();
                else
                    Logger.Singleton.Show(mDockPanel);
        }

        /// <summary>
        /// When we click the preferences we show to prefs form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreferencesCLicked(object sender, EventArgs e)
        {
            mPrefForm.ShowDialog(this);
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            // Save the general project settings
            try
            {
                //Write 
                //mDockPanel.SaveAsXml(Application.StartupPath + "\\layout.lumf");
            }
            catch
            {
            }
        }

        private void OnLineCommentClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.DoCommand(BindableCommand.LineComment);
        }

        private void OnLineUncommentClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.DoCommand(BindableCommand.LineUncomment);
        }

        private void OnBlockCommentClick(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.DoCommand(BindableCommand.StreamComment);
        }

        /// <summary>
        /// Gets the active doc
        /// </summary>
        public Document ActiveDocument
        {
            get
            {
                foreach (Document d in mDocs)
                    if (d.IsActivated)
                        return d;

                return null;
            }
        }

        /// <summary>
        /// Gets the by its location doc
        /// </summary>
        public Document FindDocument( string file )
        {
            foreach (Document d in mDocs)
                if (d.file.ToString() == file )
                    return d;

            return null;
        }

        /// <summary>
        /// Moves a line up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.LineUp();
        }

        /// <summary>
        /// Moves a line down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lineDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.LineDown();
        }


    }
}
