using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ScintillaNET;

namespace AnimateJS
{
    /// <summary>
    /// The main form which holds all dockable sub forms
    /// </summary>
    public partial class Animate : Form
    {
        private static Animate mSingleton;
        private NewProject mNewProj;
        private List<Project> mProjects;
        private List<Document> mDocs;
        private About mAbout;
        private FTP_Options mFTPOptions;
        private BuildOptions mBuildOptions;

        public Animate()
        {
            InitializeComponent();
            mSingleton = this;

            mProjects = new List<Project>();
            mDocs = new List<Document>();
            mAbout = new About();
            mFTPOptions = new FTP_Options();
            mBuildOptions = new BuildOptions();

            SolutionExplorer explorer = SolutionExplorer.Singleton;
            Logger logger = Logger.singleton;

            explorer.Show(mDockPanel);
            logger.Show(mDockPanel);

            explorer.DockTo(mDockPanel, DockStyle.Right);
            logger.DockTo(mDockPanel, DockStyle.Bottom);

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
                    OpenProject(projectLocation);
            }
            catch
            {
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

            

            textBrowser.Tag = proj.Browser;

            string[] split = proj.Browser.Split('\\');
            textBrowser.Text = split[ split.Length - 1 ];
        }

        /// <summary>
        /// When we click exit we exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitClicked(object sender, EventArgs e)
        {
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
                Logger.singleton.Log("Not all directories successfully deleted." + ex.Message, Logger.MessageType.ERROR);
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
        public void RemoveFile(string path)
        {
            foreach (Document doc in mDocs)
                if (doc.file == path)
                {
                    mDocs.Remove(doc);
                    return;
                }
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
                string projectName = mOpenDiag.SafeFileName.Split('.')[0];

                //Check if project is already open
                foreach ( Project p in mProjects )
                    if (p.Name == projectName)
                    {
                        MessageBox.Show("A project with that name is already open", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                if (mOpenDiag.FileName.Split('.')[1].ToString().ToLower() == "anp")
                {
                    OpenProject(mOpenDiag.FileName);
                }
            }
        }

        /// <summary>
        /// Opens a project
        /// </summary>
        /// <param name="path"></param>
        public void OpenProject(string path)
        {
            //Check if a valid project
            Project proj = Project.OpenProjectDirectory(path);
            mProjects.Add(proj);

            //Add project to solution explorer
            SolutionExplorer.Singleton.AddProject(proj);

            string[] parts = path.Split('\\');
            string combined = "";
            for (int i = 0; i < parts.Length - 1; i++ )
                combined += parts[i] + '\\';

            combined = combined.Substring(0, combined.Length - 1);

            proj.directory = combined;

            //Log message
            Logger.singleton.Log("Opened project " + proj.Name, Logger.MessageType.MESSAGE);
        }

        /// <summary>
        /// Gets the singleton reference
        /// </summary>
        public static Animate Singleton { get { return mSingleton; } }

        /// <summary>
        /// Creates a document
        /// </summary>
        /// <param name="content"></param>
        public void CreateDocument( string content, string fileLoc )
        {
            foreach (Document doc in mDocs)
                if (doc.file == fileLoc)
                {
                    doc.Show(mDockPanel);
                    return;
                }

            Document newDoc = new Document(fileLoc, content);

            newDoc.Show(mDockPanel);
            mDocs.Add(newDoc);
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
            foreach ( Document doc in mDocs )
                if (doc.IsActivated && !doc.Saved)
                {
                    doc.Save();
                    return;
                }
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
            {
                if (SolutionExplorer.Singleton.ActiveProject.Saved)
                    saveProjectToolStripMenuItem.Enabled = false;
                else
                    saveProjectToolStripMenuItem.Enabled = true;
            }
            else
                saveProjectToolStripMenuItem.Enabled = false;

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
                textBrowser.Tag = openFileBrowser.FileName;
                textBrowser.Text = openFileBrowser.SafeFileName;

                //Set the browser
                if ( SolutionExplorer.Singleton.ActiveProject != null )
                    SolutionExplorer.Singleton.ActiveProject.Browser = textBrowser.Tag.ToString();
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
            Project p = SolutionExplorer.Singleton.ActiveProject;

            if (!File.Exists(textBrowser.Tag.ToString()) )
            {
                MessageBox.Show("You have not selected a browser or the path: " + textBrowser.Tag.ToString() + " does not exist", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if (p == null)
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
                    (p.OverrideRunable.Trim() == "" ? p.RunPath : p.OverrideRunable.Trim()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.singleton.Log(ex.Message, Logger.MessageType.ERROR);
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

        /// <summary>
        /// Check if any projects need to be saved
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

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


            // Save the general project settings
            try
            {
                StreamWriter file = new StreamWriter(Application.StartupPath + "\\startup.options");
                foreach (Project p in mProjects)
                    file.WriteLine(p.directory + "\\" + p.Name + ".anp");
                file.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// When we click the build button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBuildClick(object sender, EventArgs e)
        {
            if (SolutionExplorer.Singleton.ActiveProject != null)
            {
                Logger.singleton.Clear();

                string catenatedBuilds = "";

                foreach (string s in SolutionExplorer.Singleton.ActiveProject.Dependencies)
                {
                    bool foundProject = false;

                    foreach (Project p in mProjects)
                        if (p.Name == s)
                        {
                            foundProject = true;
                            Logger.singleton.Log("Building " + p.Name, Logger.MessageType.MESSAGE);
                            Builder.Build(p, null);
                            Logger.singleton.Log("Build " + p.Name + " Complete: " + SolutionExplorer.Singleton.ActiveProject.OutputFile, Logger.MessageType.MESSAGE);

                            catenatedBuilds += p.CompiledJS;
                            break;
                        }

                    if ( !foundProject )
                        Logger.singleton.Log("Project " + s + " was not found.", Logger.MessageType.ERROR);
                }

                Logger.singleton.Log("Building " + SolutionExplorer.Singleton.ActiveProject.Name, Logger.MessageType.MESSAGE);
                Builder.Build(SolutionExplorer.Singleton.ActiveProject, catenatedBuilds);
                Logger.singleton.Log("Build " + SolutionExplorer.Singleton.ActiveProject.Name + " Complete: " + 
                    SolutionExplorer.Singleton.ActiveProject.OutputFile, Logger.MessageType.MESSAGE);

                // Save to the override file
                try
                {
                    StreamWriter file = new StreamWriter(SolutionExplorer.Singleton.ActiveProject.OverridePath);
                    file.Write(SolutionExplorer.Singleton.ActiveProject.CompiledJS);
                    file.Close();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Occurs when we click on the FTP options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFTPOptionsClick(object sender, EventArgs e)
        {
            mFTPOptions.ShowDialog();
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
            foreach (Document d in mDocs)
                if (d.IsActivated)
                {
                    d.Scintilla.Commands.Execute(BindableCommand.Paste);
                    return;
                }
        }

        /// <summary>
        /// When we click cut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutClick(object sender, EventArgs e)
        {
            foreach (Document d in mDocs)
                if (d.IsActivated)
                {
                    d.Scintilla.Commands.Execute(BindableCommand.Cut);
                    return;
                }
        }

        /// <summary>
        /// When we click copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyClick(object sender, EventArgs e)
        {
            foreach (Document d in mDocs)
                if (d.IsActivated)
                {
                    d.Scintilla.Commands.Execute(BindableCommand.Copy);
                    return;
                }
        }

        /// <summary>
        /// When we click find
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFindClick(object sender, EventArgs e)
        {
            foreach (Document d in mDocs)
                if (d.IsActivated)
                {
                    d.Scintilla.Commands.Execute(BindableCommand.ShowFind);
                    return;
                }
        }

        /// <summary>
        /// When we click undo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUndoClick(object sender, EventArgs e)
        {
            foreach (Document d in mDocs)
                if (d.IsActivated)
                {
                    d.Scintilla.Commands.Execute(BindableCommand.Undo);
                    return;
                }
        }

        /// <summary>
        /// When we click redo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRedoClick(object sender, EventArgs e)
        {
            foreach (Document d in mDocs)
                if (d.IsActivated)
                {
                    d.Scintilla.Commands.Execute(BindableCommand.Redo);
                    return;
                }
        }

        /// <summary>
        /// Tells run to be executed
        /// </summary>
        public void CallRun()
        {
            buttonRun.PerformClick();
        }
    }
}
