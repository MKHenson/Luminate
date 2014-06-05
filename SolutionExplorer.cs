using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Luminate
{
    /// <summary>
    /// A class that shows our directory structures
    /// </summary>
    public partial class SolutionExplorer : DockContent
    {
        public delegate void projectSelected(Project proj);
        public event projectSelected ProjectSelected;

        private static SolutionExplorer mSingleton;

        // Node being dragged
        private TreeNode mDragNode = null;

        // Temporary drop node for selection
        private TreeNode mTempDropNode = null;

        // Timer for scrolling
        private Timer mTimer = new Timer();
        
        private Project mPrevProject;

        /// <summary>
        /// Simple constructor
        /// </summary>
        public SolutionExplorer()
        {
            InitializeComponent();

            mTimer.Interval = 200;
            mTimer.Tick += new EventHandler(timer_Tick);

            //Initialise the events
            ProjectSelected += new projectSelected(OnProjectSelected);
        }

        /// <summary>
        /// Get the main tree view
        /// </summary>
        public TreeView tree { get { return mTree; } }

        /// <summary>
        /// Called when a different project is selected
        /// </summary>
        /// <param name="proj"></param>
        void OnProjectSelected(Project proj) { }

        /// <summary>
        /// Adds a project to the solution explorer
        /// </summary>
        /// <param name="proj"></param>
        public void AddProject(Project proj)
        {
            //Stop updates
            mTree.SuspendLayout();

            //Remove closed project reference
            TreeNode existingNode = null;
            foreach(TreeNode n in mTree.Nodes)
            {
                if (n.Text == proj.Name)
                {
                    existingNode = n;
                    mTree.Nodes.Remove(existingNode);
                    break;
                }
            }

            //Add the project node
            TreeNode projNode = null;
            projNode = new TreeNode(proj.Name);
            mTree.Nodes.Add(projNode);

            projNode.Tag = proj;
            projNode.ImageIndex = 2;
            projNode.SelectedImageIndex = 2;
           

            if (Luminate.Singleton.Preferences.project == proj.Name)
                projNode.NodeFont = new System.Drawing.Font(mTree.Font, mTree.Font.Style | FontStyle.Bold);
                            
            //Now populate the subnodes based on the project directory
            PopulateNode(projNode, proj.directory, proj);
            CreateFileNodes(proj.directory, projNode, proj);

            projNode.ExpandAll();

            //Resume updates
            mTree.ResumeLayout();

            mTree.SelectedNode = mTree.Nodes[0];
            sortNodes();
        }

        private void sortNodes()
        {
            mTree.SuspendLayout();
            mTree.TreeViewNodeSorter = null;
            mTree.Sort();
            mTree.TreeViewNodeSorter = new NodeSorter();
            mTree.ResumeLayout();
            //mTree.Sort();
            //mTree.TreeViewNodeSorter = prev;
        }

        /// <summary>
        /// This is a small fix to make sure the doc is saved before it closes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true; e.Cancel = true;
            Hide();

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Populates the tree with nodes 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="directoryPath"></param>
        private void PopulateNode(TreeNode parent, string directoryPath, Project proj)
        {
            string[] dirs = Directory.GetDirectories(directoryPath);

            foreach (string directory in dirs)
            {
                // Specify the directories you want to manipulate.
                DirectoryInfo di = new DirectoryInfo(directory);

                if ((di.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    continue;

                //First create the directory nodes
                string[] dirSections = directory.Split('\\');
                TreeNode dirNode = new TreeNode(dirSections[ dirSections.Length - 1 ] );
                dirNode.Tag = directory;
                dirNode.ImageIndex = 0;
                dirNode.SelectedImageIndex = 0;
                parent.Nodes.Add(dirNode);

                //Now populate the subnodes
                PopulateNode(dirNode, directory, proj);

                CreateFileNodes(directory, dirNode, proj);
            }
        }

        /// <summary>
        /// Creates the files nodes for a given directory node
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="dirNode"></param>
        /// <param name="proj"></param>
        private void CreateFileNodes(string directory, TreeNode dirNode, Project proj)
        {
            //Now create the file nodes
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                string[] fileSections = file.Split('\\');
                if ( fileSections[fileSections.Length - 1].Contains(".") && (fileSections[fileSections.Length - 1].Split('.')[1] == "lum" || fileSections[fileSections.Length - 1].Split('.')[1] == "lum"))
                    continue;

                TreeNode fileNode = new TreeNode(fileSections[fileSections.Length - 1]);
                dirNode.Nodes.Add(fileNode);
                fileNode.Tag = file;

                if (fileNode is TreeNodeClosed)
                {
                    fileNode.ImageIndex = 8;
                    fileNode.SelectedImageIndex = 8;
                }
                else if ( Builder.ignoreList.Contains( file ) )
                {
                    fileNode.ImageIndex = 9;
                    fileNode.SelectedImageIndex = 9;
                }
                else if (file == proj.RunPath)
                {
                    fileNode.ImageIndex = 3;
                    fileNode.SelectedImageIndex = 3;
                }
                else if (file == proj.First)
                {
                    fileNode.ImageIndex = 4;
                    fileNode.SelectedImageIndex = 4;
                }
                else if (file == proj.Last)
                {
                    fileNode.ImageIndex = 5;
                    fileNode.SelectedImageIndex = 5;
                }
                else if (file == proj.OutputFile)
                {
                    fileNode.ImageIndex = 6;
                    fileNode.SelectedImageIndex = 6;
                }
                else if (
                    fileNode.Tag.ToString().ToLower().Contains(".jpg") ||
                    fileNode.Tag.ToString().ToLower().Contains(".jpeg") ||
                    fileNode.Tag.ToString().ToLower().Contains(".png") ||
                    fileNode.Tag.ToString().ToLower().Contains(".bmp") ||
                    fileNode.Tag.ToString().ToLower().Contains(".gif"))
                {
                    fileNode.ImageIndex = 7;
                    fileNode.SelectedImageIndex = 7;
                }
                else
                {
                    fileNode.ImageIndex = 1;
                    fileNode.SelectedImageIndex = 1;
                }
            }
        }

        /// <summary>
        /// Gets the singleton reference
        /// </summary>
        public static SolutionExplorer Singleton
        {
            get
            {
                if (mSingleton == null)
                    mSingleton = new SolutionExplorer();

                return mSingleton;
            }
        }

        
        /// <summary>
        /// When we click a node with the mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            setBeginFileToolStripMenuItem.Enabled = false;
            setEndFileToolStripMenuItem.Enabled = false;
            setAsOutputFileToolStripMenuItem.Enabled = false;
            setAsProjectRunnableToolStripMenuItem.Enabled = false;
            untagFileToolStripMenuItem.Enabled = false;
            refreshToolStripMenuItem.Visible = false;
            buildOptionsToolStripMenuItem.Visible = false;
            goToFolderToolStripMenuItem.Visible = false;
            setAsDefaultApplicationToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Visible = false;
            buildToolStripMenuItem.Visible = false;
            //mMenuDelete.Visible = false;
            mMenuClose.Visible = false;
            mMenuRename.Visible = false;
            mMenuEdit.Visible = false;
            collapseAllToolStripMenuItem.Enabled = false;
            expandAllToolStripMenuItem.Enabled = false;

            mCreateClass.Enabled = false;
            mMenuCreateFile.Enabled = false;
            mMenuCreateFolder.Enabled = false;

            ignoredToolStripMenuItem.Visible = false;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                mTree.SelectedNode = e.Node;

                

                //Show / hide close
                if (mTree.SelectedNode != null && mTree.SelectedNode.Tag is Project)
                {
                    collapseAllToolStripMenuItem.Enabled = true;
                    expandAllToolStripMenuItem.Enabled = true;
                    mMenuClose.Visible = true;
                    mMenuCreateFile.Visible = false;
                    setEndFileToolStripMenuItem.Enabled = false;
                    refreshToolStripMenuItem.Visible = true;
                    goToFolderToolStripMenuItem.Visible = true;
                    buildOptionsToolStripMenuItem.Visible = true;
                    setAsDefaultApplicationToolStripMenuItem.Enabled = true;
                    buildToolStripMenuItem.Visible = true;
                    mMenuClose.Visible = true;
                    mCreateClass.Enabled = true;
                    mMenuCreateFile.Enabled = true;
                    mMenuCreateFolder.Enabled = true;
                }
                //Check if a closed node
                else if (mTree.SelectedNode is TreeNodeClosed)
                {
                    //mMenuDelete.Visible = true;
                }
                else
                {
                    mCreateClass.Enabled = true;
                    mMenuCreateFile.Enabled = true;
                    mMenuCreateFolder.Enabled = true;

                    Project p = GetRootNode(mTree.SelectedNode).Tag as Project;

                    if (File.Exists(mTree.SelectedNode.Tag.ToString()))
                    {
                        copyToolStripMenuItem.Visible = true;
                        mMenuRename.Visible = true;
                        mMenuEdit.Visible = true;
                    }

                    if (File.Exists(mTree.SelectedNode.Tag.ToString()) && mTree.SelectedNode.Tag.ToString().ToLower().Contains(".js"))
                    {
                        

                        if (mTree.SelectedNode.Tag.ToString() != p.First && mTree.SelectedNode.Tag.ToString() != p.Last && mTree.SelectedNode.Tag.ToString() != p.OutputFile)
                        {
                            setAsOutputFileToolStripMenuItem.Enabled = true;
                            setBeginFileToolStripMenuItem.Enabled = true;
                            setEndFileToolStripMenuItem.Enabled = true;

                            //Its not a special js file - so now check if its being ignored ...
                            ignoredToolStripMenuItem.Visible = true;
                            if (Builder.ignoreList.Contains(mTree.SelectedNode.Tag.ToString()))
                                ignoredToolStripMenuItem.Checked = true;
                            else
                                ignoredToolStripMenuItem.Checked = false;
                        }
                        else
                            untagFileToolStripMenuItem.Enabled = true;
                    }
                    else if (File.Exists(mTree.SelectedNode.Tag.ToString()))
                    {
                        copyToolStripMenuItem.Visible = true;
                        if (mTree.SelectedNode.Tag.ToString() != p.First && mTree.SelectedNode.Tag.ToString() != p.Last && mTree.SelectedNode.Tag.ToString() != p.OutputFile)
                        {
                            setAsProjectRunnableToolStripMenuItem.Enabled = true;
                        }
                    }

                    mMenuClose.Visible = false;
                    mMenuCreateFile.Visible = true;
                }
                mContext.Show(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        /// <summary>
        /// Handle he delete click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if ( mTree.SelectedNode != null)
            {
                if (mTree.SelectedNode is TreeNodeClosed)
                {
                    mTree.Nodes.Remove(mTree.SelectedNode);
                    return;
                }
                //If a project then ask if the use is sure and finally delete.
                if (mTree.SelectedNode.Tag is Project)
                {
                    if (MessageBox.Show("Are you sure you want to delete this project? This is irreversible.",
                        "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        List<TreeNode> nodes = GetAllNodes( mTree.SelectedNode, null );
                        foreach (TreeNode n in nodes)
                            Luminate.Singleton.RemoveFile(n.Tag.ToString());
                        
                        Luminate.Singleton.DeleteProject(mTree.SelectedNode.Tag as Project, true);
                        mTree.Nodes.Remove(mTree.SelectedNode);
                    }
                }
                else if (mTree.SelectedNode is TreeNodeClosed)
                {
                    mTree.Nodes.Remove(mTree.SelectedNode);
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to delete " + mTree.SelectedNode.Tag + "?",
                       "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {                        
                        try
                        {
                            //Finally delete
                            if ( Directory.Exists( mTree.SelectedNode.Tag.ToString() ) )
                                Directory.Delete( mTree.SelectedNode.Tag.ToString(), true );
                            else if ( File.Exists( mTree.SelectedNode.Tag.ToString() ) )
                                File.Delete(mTree.SelectedNode.Tag.ToString());

                            if (ActiveProject.First == mTree.SelectedNode.Tag.ToString())
                                ActiveProject.First = "";
                            if (ActiveProject.Last == mTree.SelectedNode.Tag.ToString())
                                ActiveProject.Last = "";
                            if (ActiveProject.OutputFile == mTree.SelectedNode.Tag.ToString())
                                ActiveProject.OutputFile = "";


                            //Remove all open files
                            Luminate.Singleton.RemoveFile(mTree.SelectedNode.Tag.ToString(), true );

                            mTree.SelectedNode.Parent.Nodes.Remove(mTree.SelectedNode);

                            //Select new node
                            if (mTree.Nodes.Count > 0)
                                mTree.SelectedNode = mTree.Nodes[0];
                            else
                                ProjectSelected(null);

                            
                        }
                        catch (Exception ex)
                        {
                            Logger.Singleton.Log("Not all directories successfully deleted." + ex.Message, Logger.MessageType.ERROR);
                        }
                    }
                }
            }
        }

        private List<TreeNode> GetAllNodes( TreeNode n, List<TreeNode> nodes)
        {
            if (nodes == null)
                nodes = new List<TreeNode>();

            foreach (TreeNode subN in n.Nodes)
            {
                nodes.Add(subN);
                GetAllNodes(subN, nodes);
            }

            return nodes;
        }

        /// <summary>
        /// Close the selected project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseClicked(object sender, EventArgs e)
        {
            if ( mTree.SelectedNode != null)
            {
                //If a project then ask if the use is sure and finally delete.
                if (mTree.SelectedNode.Tag is Project)
                    if (MessageBox.Show("Are you sure you want to close this project?",
                        "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //Create Closed node
                        TreeNodeClosed closedNode = new TreeNodeClosed(mTree.SelectedNode.Text);
                        closedNode.Tag = ((Project)(mTree.SelectedNode.Tag)).directory + "\\" + ((Project)(mTree.SelectedNode.Tag)).Name + ".lum";
                        

                        List<TreeNode> nodes = GetAllNodes(mTree.SelectedNode, null);
                        foreach (TreeNode n in nodes)
                            Luminate.Singleton.RemoveFile(n.Tag.ToString());

                        Luminate.Singleton.DeleteProject(mTree.SelectedNode.Tag as Project, false);
                        mTree.Nodes.Remove(mTree.SelectedNode);

                        mTree.Nodes.Add(closedNode);

                        //Select new node
                        if (mTree.Nodes.Count > 0)
                            mTree.SelectedNode = mTree.Nodes[0];
                        else
                            ProjectSelected(null);

                        //sortNodes();
                    }
            }
        }

        /// <summary>
        /// When we double click a node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDoubleClickNode(object sender, TreeNodeMouseClickEventArgs e)
        {
            OpenFileForEditing(e.Node);
        }

        /// <summary>
        /// Opens a file for editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Document OpenFileForEditing( TreeNode node )
        {
            if (node is TreeNodeClosed)
            {
                Luminate.Singleton.OpenProject(node.Tag.ToString());
            }
            else if (node.Tag is Project == false && File.Exists(node.Tag.ToString()))
            {
                try
                {
                    string fileContent = null;

                    // Create an instance of StreamReader to read from a file.
                    // The using statement also closes the StreamReader.
                    using (StreamReader sr = new StreamReader(node.Tag.ToString()))
                        fileContent = sr.ReadToEnd();

                    Document doc = Luminate.Singleton.CreateDocument( fileContent, node.Tag.ToString() );
                    return doc;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateFolder(object sender, EventArgs e)
        {
            CreateNode(false, "");
        }


       /// <summary>
       /// This function is used to create a directory or file on the system
       /// </summary>
       /// <param name="createFile"></param>
        private void CreateNode( bool createFile = false, string content = "" )
        {
            if (mTree.SelectedNode != null)
            {
                string baseDir = "";
                bool isFile = false;

                //First get the base Dir
                if (mTree.SelectedNode.Tag is Project)
                    baseDir = (mTree.SelectedNode.Tag as Project).directory;
                else if (Directory.Exists(mTree.SelectedNode.Tag.ToString()))
                    baseDir = mTree.SelectedNode.Tag.ToString();
                else if (File.Exists(mTree.SelectedNode.Tag.ToString()))
                {
                    baseDir = Path.GetDirectoryName(mTree.SelectedNode.Tag.ToString());
                    isFile = true;
                }

                //Now check it exists
                int counter = 0;
                string finalStr = baseDir + "\\New " + (createFile ? "File" : "Folder");
                while ( Directory.Exists(finalStr) || File.Exists(finalStr) )
                {
                    counter++;
                    finalStr = baseDir + "\\New " + (createFile ? "File" : "Folder") + "(" + counter.ToString() + ")";
                }

                try
                {
                    //Finally create it
                    if ( createFile )
                    {
                        StreamWriter file = new StreamWriter(finalStr);
                        file.Write(content);
                        file.Close();

                        //using (FileStream fs = File.Create( finalStr, 1024 ) )
                       // {
                        //}
                    }
                    else
                        Directory.CreateDirectory(finalStr);

                    //Create and add the tree node
                    string[] dirSplit = finalStr.Split('\\');
                    string nodeName = dirSplit[dirSplit.Length - 1];
                    TreeNode toAdd = new TreeNode(nodeName);
                    if (isFile)
                        mTree.SelectedNode.Parent.Nodes.Add(toAdd);
                    else
                        mTree.SelectedNode.Nodes.Add(toAdd);

                    if (createFile)
                    {
                        toAdd.ImageIndex = 1;
                        toAdd.SelectedImageIndex = 1;
                    }
                    else
                    {
                        toAdd.ImageIndex = 0;
                        toAdd.SelectedImageIndex = 0;
                    }

                    toAdd.Tag = finalStr;
                    mTree.SelectedNode = toAdd;
                    toAdd.BeginEdit();
                }
                catch (Exception ex)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(ex.Message);
                }
            }

            //sortNodes();
        }

        /// <summary>
        /// Gets the root node of a node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private TreeNode GetRootNode(TreeNode node)
        {
            if (node == null)
                return null;

            TreeNode toRet = node;

            while (toRet.Parent != null)
                toRet = toRet.Parent;

            return toRet;
        }

        /// <summary>
        /// Gets the root node of a node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Project ActiveProject
        {
            get
            {
                TreeNode root = GetRootNode( mTree.SelectedNode );
                if (root == null)
                    return null;

                Project toRet = root.Tag as Project;
                return toRet;
            }
        }

        /// <summary>
        /// A folder has been renamed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLabelEdited(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Tag is Project == false)
            {
                string[] dirParts = e.Node.Tag.ToString().Split('\\');

                if (e.Label == null || e.Node.Text == e.Label)
                {
                    e.CancelEdit = true;
                    return;
                }

                try
                {
                    if (Directory.Exists(e.Node.Tag.ToString()))
                    {
                        string oldDir = e.Node.Tag.ToString();
                        string destination = Directory.GetParent(e.Node.Tag.ToString()) + "\\" + e.Label;
                        Directory.Move(e.Node.Tag.ToString(), destination);

                        //Reassign the tag
                        e.Node.Tag = destination;

                        AdjustTags(e.Node, oldDir, destination);
                    }
                    else
                    {
                        string oldFile = e.Node.Tag.ToString();
                        string baseDir = Path.GetDirectoryName(mTree.SelectedNode.Tag.ToString());
                        File.Copy(e.Node.Tag.ToString(), baseDir + "\\" + e.Label);
                        File.Delete(oldFile);

                        Luminate.Singleton.FileRenamed(oldFile, baseDir + "\\" + e.Label);

                        //Reassign the tag
                        e.Node.Tag = baseDir + "\\" + e.Label;
                        e.Node.Text = e.Label;
                        sortNodes();
                        mTree.SelectedNode = e.Node;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                    e.CancelEdit = true;
                }
            }
            else
            {
                if (e.Label == null || (e.Node.Tag as Project).Name == e.Label)
                {
                    e.CancelEdit = true;
                    return;
                }

                //If the file exists then just send a message and exit
                if (File.Exists((e.Node.Tag as Project).directory + "\\" + e.Label + ".lum"))
                {
                    MessageBox.Show("A file with the name " + e.Label + " already exists in the folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Singleton.Log("A file with the name " + e.Label + " already exists in the folder.", Logger.MessageType.ERROR);
                    e.CancelEdit = true;
                    return;
                }

                //Make sure no other projects with that name exist.
                foreach ( TreeNode n in mTree.Nodes )
                    if ((n.Tag as Project).Name == e.Label)
                    {
                        MessageBox.Show("A project with the name " + e.Label + " already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Logger.Singleton.Log("A project with the name " + e.Label + " already exists.", Logger.MessageType.ERROR);
                        e.CancelEdit = true;
                        return;
                    }
                try
                {
                    //Copy the file
                    File.Copy((e.Node.Tag as Project).directory + "\\" + (e.Node.Tag as Project).Name + ".lum",
                        (e.Node.Tag as Project).directory + "\\" + e.Label + ".lum");

                    //Delete the old one
                    File.Delete((e.Node.Tag as Project).directory + "\\" + (e.Node.Tag as Project).Name + ".lum");

                    //Set the new name
                    (e.Node.Tag as Project).Name = e.Label;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                    e.CancelEdit = true;
                }
                
            }
        }

        /// <summary>
        /// When we click rename
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRenameClicked(object sender, EventArgs e)
        {
            if (mTree.SelectedNode != null)
                mTree.SelectedNode.BeginEdit();
        }

        /// <summary>
        /// When we click the edit menu button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditClicked(object sender, EventArgs e)
        {
            OpenFileForEditing(mTree.SelectedNode);
        }

        /// <summary>
        /// Creates a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateFileClick(object sender, EventArgs e)
        {
            if (sender == mMenuCreateFile)
                CreateNode(true, "");
            else
            {
                string content = "/***\n";
                content += "* imports go here:\n";
                content += "*/\n\n";

                content += "/** A simple JS class*/\n";
                content += "function MyClass()\n{\n}";

                CreateNode(true, content);
            }
        }
        
        /// <summary>
        /// When the user presses the F2 key on the explorer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mTree_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (mTree.SelectedNode != null)
                    mTree.SelectedNode.BeginEdit();
            }
            else if (e.KeyCode == Keys.F6)
                Luminate.Singleton.CallBuild();
            else if (e.KeyCode == Keys.F5)
                Luminate.Singleton.CallRun();
            else if (e.KeyCode == Keys.Delete)
                mMenuDelete.PerformClick();
        }






        /// <summary>
        /// When we drag an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			// Get drag node and select it
			this.mDragNode = (TreeNode)e.Item;
			this.mTree.SelectedNode = this.mDragNode;

			// Reset image list used for drag image
			this.imageListDrag.Images.Clear();
			this.imageListDrag.ImageSize = new Size(this.mDragNode.Bounds.Size.Width + this.mTree.Indent, this.mDragNode.Bounds.Height);

			// Create new bitmap
			// This bitmap will contain the tree node image to be dragged
			Bitmap bmp = new Bitmap(this.mDragNode.Bounds.Width + this.mTree.Indent, this.mDragNode.Bounds.Height);

			// Get graphics from bitmap
			Graphics gfx = Graphics.FromImage(bmp);

			// Draw node icon into the bitmap
			gfx.DrawImage(this.imageListTreeView.Images[0], 0, 0);

			// Draw node label into bitmap
			gfx.DrawString(this.mDragNode.Text,
				this.mTree.Font,
				new SolidBrush(this.mTree.ForeColor),
				(float)this.mTree.Indent, 1.0f);

			// Add bitmap to imagelist
			this.imageListDrag.Images.Add(bmp);

			// Get mouse position in client coordinates
			Point p = this.mTree.PointToClient(Control.MousePosition);

			// Compute delta between mouse position and node bounds
			int dx = p.X + this.mTree.Indent - this.mDragNode.Bounds.Left;
			int dy = p.Y - this.mDragNode.Bounds.Top;

			// Begin dragging image
			if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, dx, dy))
			{
				// Begin dragging
				this.mTree.DoDragDrop(bmp, DragDropEffects.Move);
				// End dragging image
				DragHelper.ImageList_EndDrag();
			}		
		
		}

        /// <summary>
        /// When we drag over the tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Compute drag position and move image
			Point formP = this.PointToClient(new Point(e.X, e.Y));
			DragHelper.ImageList_DragMove(formP.X - this.mTree.Left, formP.Y - this.mTree.Top);

			// Get actual drop node
			TreeNode dropNode = this.mTree.GetNodeAt(this.mTree.PointToClient(new Point(e.X, e.Y)));
			if(dropNode == null)
			{
				e.Effect = DragDropEffects.None;
				return;
			}

            if (dropNode == mDragNode)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            //If its not a directory then just return
            if ( !Directory.Exists( dropNode.Tag.ToString() ) )
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            string oldDest = mDragNode.Tag.ToString();
            string newDest = dropNode.Tag.ToString();
            string[] split = mDragNode.Tag.ToString().Split('\\');
            string newLocation = dropNode.Tag.ToString() + "\\" + split[split.Length - 1];

            //Check if you're not over writing anything
            if (Directory.Exists(newLocation) || File.Exists(newLocation))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
			
			e.Effect = DragDropEffects.Move;

			// if mouse is on a new node select it
			if(this.mTempDropNode != dropNode)
			{
				DragHelper.ImageList_DragShowNolock(false);
				this.mTree.SelectedNode = dropNode;
				DragHelper.ImageList_DragShowNolock(true);
				mTempDropNode = dropNode;
			}
			
			// Avoid that drop node is child of drag node 
			TreeNode tmpNode = dropNode;
			while(tmpNode.Parent != null)
			{
				if(tmpNode.Parent == this.mDragNode) e.Effect = DragDropEffects.None;
				tmpNode = tmpNode.Parent;
			}
		}

        /// <summary>
        /// When we drop on the tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            // Unlock updates
            DragHelper.ImageList_DragLeave(this.mTree.Handle);

            // Get drop node
            TreeNode dropNode = this.mTree.GetNodeAt(this.mTree.PointToClient(new Point(e.X, e.Y)));


			// If drop node isn't equal to drag node, add drag node as child of drop node
			if(this.mDragNode != dropNode)
			{
                //Move the files and folders and re-assign the tag
                try
                {
                    string[] split = mDragNode.Tag.ToString().Split('\\');

                    if (File.Exists(mDragNode.Tag.ToString()))
                    {
                        File.Move(mDragNode.Tag.ToString(), dropNode.Tag.ToString() + "\\" + split[split.Length - 1]);
                        Luminate.Singleton.FileRenamed(mDragNode.Tag.ToString(), dropNode.Tag.ToString() + "\\" + split[split.Length - 1]);
                    }
                    else if (Directory.Exists(mDragNode.Tag.ToString()))
                    {
                        //Directory.Move(mDragNode.Tag.ToString(), dropNode.Tag.ToString() );
                        CopyDirectory(mDragNode.Tag.ToString(), dropNode.Tag.ToString() + "\\" + split[split.Length - 1], true);

                        Directory.Delete(mDragNode.Tag.ToString(), true);
                    }
                    string oldDest = mDragNode.Tag.ToString();
                    string newDest = dropNode.Tag.ToString() + "\\" + split[split.Length - 1];

                    mDragNode.Tag = dropNode.Tag.ToString() + "\\" + split[split.Length - 1];
                    
                    AdjustTags(mDragNode, oldDest, newDest);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


				// Remove drag node from parent
				if(this.mDragNode.Parent == null)
					this.mTree.Nodes.Remove(this.mDragNode);
				else
					this.mDragNode.Parent.Nodes.Remove(this.mDragNode);

				// Add drag node to drop node
				dropNode.Nodes.Add(this.mDragNode);
				dropNode.ExpandAll();

				// Disable scroll timer
				this.mTimer.Enabled = false;

                // Set drag node to null
                this.mDragNode = null;
			}
		}

        private static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
           
            SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
            DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

            if (Directory.Exists(SourcePath))
            {
                if (Directory.Exists(DestinationPath) == false)
                    Directory.CreateDirectory(DestinationPath);

                foreach (string fls in Directory.GetFiles(SourcePath))
                {
                    FileInfo flinfo = new FileInfo(fls);
                    flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                }
                foreach (string drs in Directory.GetDirectories(SourcePath))
                {
                    DirectoryInfo drinfo = new DirectoryInfo(drs);
                    if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                        ret = false;
                }
            }
            ret = true;
            
            return ret;
        }

        /// <summary>
        /// Re-assign the tags
        /// </summary>
        /// <param name="node"></param>
        /// <param name="oldDest"></param>
        /// <param name="newDest"></param>
        private void AdjustTags(TreeNode node, string oldDest, string newDest)
        {
            Luminate.Singleton.FileRenamed(node.Tag.ToString(), node.Tag.ToString());

            string tag = node.Tag.ToString();
            node.Tag = tag.Replace(oldDest, newDest);
            
            foreach (TreeNode n in node.Nodes)
                AdjustTags(n, oldDest, newDest);
        }

        /// <summary>
        /// When the drag enters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			DragHelper.ImageList_DragEnter(this.mTree.Handle, e.X - this.mTree.Left,
				e.Y - this.mTree.Top);

			// Enable timer for scrolling dragged item
			this.mTimer.Enabled = true;
		}

        /// <summary>
        /// When the drag leaves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void OnDragLeave(object sender, System.EventArgs e)
		{
			DragHelper.ImageList_DragLeave(this.mTree.Handle);

			// Disable timer for scrolling dragged item
			this.mTimer.Enabled = false;
		}

        /// <summary>
        /// Give feedback when we move over the nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void OnGiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			if(e.Effect == DragDropEffects.Move) 
			{
				// Show pointer cursor while dragging
				e.UseDefaultCursors = false;
				this.mTree.Cursor = Cursors.Default;
			}
			else e.UseDefaultCursors = true;
			
		}

        /// <summary>
        /// When the timer ticks we check if we need to scroll or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void timer_Tick(object sender, EventArgs e)
		{
			// get node at mouse position
			Point pt = PointToClient(Control.MousePosition);
			TreeNode node = this.mTree.GetNodeAt(pt);

			if(node == null) return;

			// if mouse is near to the top, scroll up
			if(pt.Y < 30)
			{
				// set actual node to the upper one
				if (node.PrevVisibleNode!= null) 
				{
					node = node.PrevVisibleNode;
				
					// hide drag image
					DragHelper.ImageList_DragShowNolock(false);
					// scroll and refresh
					node.EnsureVisible();
					this.mTree.Refresh();
					// show drag image
					DragHelper.ImageList_DragShowNolock(true);
					
				}
			}
			// if mouse is near to the bottom, scroll down
			else if(pt.Y > this.mTree.Size.Height - 30)
			{
				if (node.NextVisibleNode!= null) 
				{
					node = node.NextVisibleNode;
				
					DragHelper.ImageList_DragShowNolock(false);
					node.EnsureVisible();
					this.mTree.Refresh();
					DragHelper.ImageList_DragShowNolock(true);
				}
			} 
		}
        

        /// <summary>
        /// When we select a node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeNodeSelect(object sender, TreeViewEventArgs e)
        {
            if (mPrevProject != ActiveProject)
            {
                mPrevProject = ActiveProject;
                ProjectSelected(mPrevProject);
            }
        }

        /// <summary>
        /// When we click runnable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSetRunnable(object sender, EventArgs e)
        {
            TreeNode root = GetRootNode(mTree.SelectedNode);
            if (root != null)
            {
                (root.Tag as Project).RunPath = mTree.SelectedNode.Tag.ToString();

                //Turn off the current runnable
                foreach (TreeNode n in root.Nodes)
                    ChangeRunnable(n, root.Tag as Project);
            }
        }

        /// <summary>
        /// Turns off the runnable icon
        /// </summary>
        /// <param name="n"></param>
        private void ChangeRunnable(TreeNode n, Project p)
        {
            if ( n.Tag.ToString() == p.RunPath )
            {
                n.ImageIndex = 3;
                n.SelectedImageIndex = 3;
            }
            else if (n.Tag.ToString() == p.First)
            {
                n.ImageIndex = 4;
                n.SelectedImageIndex = 4;
            }
            else if (n.Tag.ToString() == p.Last)
            {
                n.ImageIndex = 5;
                n.SelectedImageIndex = 5;
            }
            else if (n.Tag.ToString() == p.OutputFile)
            {
                n.ImageIndex = 6;
                n.SelectedImageIndex = 6;
            }
            else if (
                    n.Tag.ToString().ToLower().Contains(".jpg") ||
                    n.Tag.ToString().ToLower().Contains(".jpeg") ||
                    n.Tag.ToString().ToLower().Contains(".png") ||
                    n.Tag.ToString().ToLower().Contains(".bmp") ||
                    n.Tag.ToString().ToLower().Contains(".gif"))
            {
                n.ImageIndex = 7;
                n.SelectedImageIndex = 7;
            }
            else if (File.Exists(n.Tag.ToString()))
            {
                n.ImageIndex = 1;
                n.SelectedImageIndex = 1;
            }

            foreach (TreeNode child in n.Nodes)
                ChangeRunnable(child, p);
        }

        /// <summary>
        /// When we set a begin file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSetBegin(object sender, EventArgs e)
        {
            TreeNode root = GetRootNode(mTree.SelectedNode);
            if (root != null)
            {
                (root.Tag as Project).First = mTree.SelectedNode.Tag.ToString();

                //Turn off the current runnable
                foreach (TreeNode n in root.Nodes)
                    ChangeRunnable(n, root.Tag as Project);
            }
        }

        /// <summary>
        /// When we set an end file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSetEnd(object sender, EventArgs e)
        {
            TreeNode root = GetRootNode(mTree.SelectedNode);
            if (root != null)
            {
                (root.Tag as Project).Last = mTree.SelectedNode.Tag.ToString();

                //Turn off the current runnable
                foreach (TreeNode n in root.Nodes)
                    ChangeRunnable(n, root.Tag as Project);
            }
        }

        /// <summary>
        /// When we click on a file that we want as our output file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSetOutput(object sender, EventArgs e)
        {
            TreeNode root = GetRootNode(mTree.SelectedNode);
            if (root != null)
            {
                (root.Tag as Project).OutputFile = mTree.SelectedNode.Tag.ToString();

                //Turn off the current runnable
                foreach (TreeNode n in root.Nodes)
                    ChangeRunnable(n, root.Tag as Project);
            }
        }

        /// <summary>
        /// When you click refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRefreshClick(object sender, EventArgs e)
        {
            //Stop updates
            mTree.SuspendLayout();

            //Clear all current nodes
            mTree.SelectedNode.Nodes.Clear();
            
            //Now populate the subnodes based on the project directory
            PopulateNode(mTree.SelectedNode, (mTree.SelectedNode.Tag as Project).directory, (mTree.SelectedNode.Tag as Project));
            CreateFileNodes((mTree.SelectedNode.Tag as Project).directory, mTree.SelectedNode, (mTree.SelectedNode.Tag as Project));

            //Resume updates
            mTree.ResumeLayout();
        }

        /// <summary>
        /// When we click go to folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGoToFolderClick(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                string windir = Environment.GetEnvironmentVariable("WINDIR");
                prc.StartInfo.FileName = windir + @"\explorer.exe";
                prc.StartInfo.Arguments = ActiveProject.directory;
                prc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
            }
        }

        /// <summary>
        /// Opens build options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBuildOptions(object sender, EventArgs e)
        {
            Luminate.Singleton.CallBuildOptions();
        }

        /// <summary>
        /// Untags a file - so its normal again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UntagAFile(object sender, EventArgs e)
        {
            TreeNode root = GetRootNode(mTree.SelectedNode);
            if (root != null)
            {
                if ((root.Tag as Project).Last == mTree.SelectedNode.Tag.ToString())
                    (root.Tag as Project).Last = "";
                if ((root.Tag as Project).First == mTree.SelectedNode.Tag.ToString())
                    (root.Tag as Project).First = "";
                if ((root.Tag as Project).OutputFile == mTree.SelectedNode.Tag.ToString())
                    (root.Tag as Project).OutputFile = "";

                //Turn off the current runnable
                mTree.SelectedNode.ImageIndex = 1;
                mTree.SelectedNode.SelectedImageIndex = 1;
            }
        }

        /// <summary>
        /// Set a default application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefaultAppSelected(object sender, EventArgs e)
        {
             Luminate.Singleton.Preferences.project = ((Project)mTree.SelectedNode.Tag).Name;

            mTree.SelectedNode.NodeFont = new System.Drawing.Font( mTree.Font, mTree.Font.Style | FontStyle.Bold);
            mTree.SelectedNode.Text += string.Empty;

            foreach (TreeNode n in mTree.Nodes)
                if ( n is TreeNodeClosed == false )
                    if (((Project)n.Tag).Name != Luminate.Singleton.Preferences.project)
                        n.NodeFont = null;
        }

        /// <summary>
        /// When we click collapse all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollapseAllClick(object sender, EventArgs e)
        {
            mTree.CollapseAll();
        }

        /// <summary>
        /// When we click expand all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExpandAllClick(object sender, EventArgs e)
        {
            mTree.ExpandAll();
        }

        /// <summary>
        /// When we click the context menu build button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBuildClick(object sender, EventArgs e)
        {
            if ( mTree.SelectedNode != null )
                if ( mTree.SelectedNode.Tag is Project )
                  Luminate.Singleton.BuildProject( mTree.SelectedNode.Tag as Project );
        }

        /// <summary>
        /// When we click the copy context menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyClick(object sender, EventArgs e)
        {
            if (mTree.SelectedNode != null)
            {
                string oldFile = mTree.SelectedNode.Tag.ToString();
                string baseDir = Path.GetDirectoryName(mTree.SelectedNode.Tag.ToString());

                int counter = 0;
                string newFile = baseDir + "\\Copy_" + mTree.SelectedNode.Text;
                while( File.Exists( newFile ) )
                {
                    newFile = baseDir + "\\Copy"+ counter.ToString() +"_" + mTree.SelectedNode.Text;
                    counter++;
                }

                File.Copy( oldFile, newFile );

                //Create and add the tree node
                string[] dirSplit = newFile.Split('\\');
                string nodeName = dirSplit[dirSplit.Length - 1];
                TreeNode toAdd = new TreeNode(nodeName);
                mTree.SelectedNode.Parent.Nodes.Add(toAdd);
             
                toAdd.ImageIndex = 1;
                toAdd.SelectedImageIndex = 1;
                toAdd.Tag = newFile;
                mTree.SelectedNode = toAdd;
                toAdd.BeginEdit();
            }
        }

        /// <summary>
        /// Use this to add to or remove from the ignore list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIgnoredClick(object sender, EventArgs e)
        {
            if (mTree.SelectedNode != null)
            {
                if (ignoredToolStripMenuItem.Checked)
                {
                    if (!Builder.ignoreList.Contains(mTree.SelectedNode.Tag.ToString()))
                        Builder.ignoreList.Add(mTree.SelectedNode.Tag.ToString());

                    mTree.SelectedNode.ImageIndex = 9;
                    mTree.SelectedNode.SelectedImageIndex = 9;
                }
                else
                {
                    Builder.ignoreList.Remove(mTree.SelectedNode.Tag.ToString());
                    mTree.SelectedNode.ImageIndex = 1;
                    mTree.SelectedNode.SelectedImageIndex = 1;
                }

            }
        }
    }
}

// Create a node sorter that implements the IComparer interface. 
public class NodeSorter : IComparer
{
    // Compare the length of the strings, or the strings 
    // themselves, if they are the same length. 
    public int Compare(object x, object y)
    {
        TreeNode tx = x as TreeNode;
        TreeNode ty = y as TreeNode;

        // Compare the length of the strings, returning the difference. 
        if ( tx.ImageIndex == 0 && ty.ImageIndex != 0 )
            return -1;

        int c = string.Compare(tx.Text, ty.Text);
        // If they are the same length, call Compare. 
        return c;
    }
}