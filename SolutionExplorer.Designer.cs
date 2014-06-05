namespace Luminate
{
    partial class SolutionExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionExplorer));
            this.mTree = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.mContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mMenuCreateFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenuCreateFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mCreateClass = new System.Windows.Forms.ToolStripMenuItem();
            this.goToFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.setAsDefaultApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsProjectRunnableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.setBeginFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setEndFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsOutputFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.untagFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoredToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTree
            // 
            this.mTree.AllowDrop = true;
            this.mTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTree.ImageIndex = 0;
            this.mTree.ImageList = this.imageListTreeView;
            this.mTree.LabelEdit = true;
            this.mTree.Location = new System.Drawing.Point(0, 0);
            this.mTree.Name = "mTree";
            this.mTree.SelectedImageIndex = 0;
            this.mTree.Size = new System.Drawing.Size(284, 262);
            this.mTree.TabIndex = 0;
            this.mTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnLabelEdited);
            this.mTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnItemDrag);
            this.mTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeNodeSelect);
            this.mTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClick);
            this.mTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnDoubleClickNode);
            this.mTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.mTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.mTree.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
            this.mTree.DragLeave += new System.EventHandler(this.OnDragLeave);
            this.mTree.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.OnGiveFeedback);
            this.mTree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mTree_KeyUp);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "dir.png");
            this.imageListTreeView.Images.SetKeyName(1, "file.png");
            this.imageListTreeView.Images.SetKeyName(2, "lumniateIcon.png");
            this.imageListTreeView.Images.SetKeyName(3, "file-run.png");
            this.imageListTreeView.Images.SetKeyName(4, "file-begin.png");
            this.imageListTreeView.Images.SetKeyName(5, "file-end.png");
            this.imageListTreeView.Images.SetKeyName(6, "file-app.png");
            this.imageListTreeView.Images.SetKeyName(7, "image2.png");
            this.imageListTreeView.Images.SetKeyName(8, "folder_small-closed.png");
            this.imageListTreeView.Images.SetKeyName(9, "file-ignored.png");
            // 
            // mContext
            // 
            this.mContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenuCreateFolder,
            this.mMenuCreateFile,
            this.mCreateClass,
            this.goToFolderToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.buildOptionsToolStripMenuItem,
            this.toolStripSeparator4,
            this.collapseAllToolStripMenuItem,
            this.expandAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.mMenuClose,
            this.copyToolStripMenuItem,
            this.mMenuEdit,
            this.mMenuRename,
            this.mMenuDelete,
            this.toolStripSeparator3,
            this.setAsDefaultApplicationToolStripMenuItem,
            this.setAsProjectRunnableToolStripMenuItem,
            this.buildToolStripMenuItem,
            this.toolStripSeparator2,
            this.setBeginFileToolStripMenuItem,
            this.setEndFileToolStripMenuItem,
            this.setAsOutputFileToolStripMenuItem,
            this.untagFileToolStripMenuItem,
            this.ignoredToolStripMenuItem});
            this.mContext.Name = "mContext";
            this.mContext.Size = new System.Drawing.Size(212, 490);
            // 
            // mMenuCreateFolder
            // 
            this.mMenuCreateFolder.Image = global::Luminate.Properties.Resources.folder_add_16;
            this.mMenuCreateFolder.Name = "mMenuCreateFolder";
            this.mMenuCreateFolder.Size = new System.Drawing.Size(211, 22);
            this.mMenuCreateFolder.Text = "Create Folder";
            this.mMenuCreateFolder.Click += new System.EventHandler(this.OnCreateFolder);
            // 
            // mMenuCreateFile
            // 
            this.mMenuCreateFile.Image = global::Luminate.Properties.Resources.document_add_24;
            this.mMenuCreateFile.Name = "mMenuCreateFile";
            this.mMenuCreateFile.Size = new System.Drawing.Size(211, 22);
            this.mMenuCreateFile.Text = "Create File";
            this.mMenuCreateFile.Click += new System.EventHandler(this.OnCreateFileClick);
            // 
            // mCreateClass
            // 
            this.mCreateClass.Image = global::Luminate.Properties.Resources.document_add_24;
            this.mCreateClass.Name = "mCreateClass";
            this.mCreateClass.Size = new System.Drawing.Size(211, 22);
            this.mCreateClass.Text = "Create Class";
            this.mCreateClass.Click += new System.EventHandler(this.OnCreateFileClick);
            // 
            // goToFolderToolStripMenuItem
            // 
            this.goToFolderToolStripMenuItem.Image = global::Luminate.Properties.Resources.folder_small;
            this.goToFolderToolStripMenuItem.Name = "goToFolderToolStripMenuItem";
            this.goToFolderToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.goToFolderToolStripMenuItem.Text = "Go to Folder";
            this.goToFolderToolStripMenuItem.Click += new System.EventHandler(this.OnGoToFolderClick);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Image = global::Luminate.Properties.Resources.refresh;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.OnRefreshClick);
            // 
            // buildOptionsToolStripMenuItem
            // 
            this.buildOptionsToolStripMenuItem.Name = "buildOptionsToolStripMenuItem";
            this.buildOptionsToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.buildOptionsToolStripMenuItem.Text = "Build Options";
            this.buildOptionsToolStripMenuItem.Click += new System.EventHandler(this.OnBuildOptions);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(208, 6);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.OnCollapseAllClick);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.OnExpandAllClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(208, 6);
            // 
            // mMenuClose
            // 
            this.mMenuClose.Image = global::Luminate.Properties.Resources.delete_16;
            this.mMenuClose.Name = "mMenuClose";
            this.mMenuClose.Size = new System.Drawing.Size(211, 22);
            this.mMenuClose.Text = "Close";
            this.mMenuClose.Click += new System.EventHandler(this.OnCloseClicked);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::Luminate.Properties.Resources.copy_small;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.OnCopyClick);
            // 
            // mMenuEdit
            // 
            this.mMenuEdit.Name = "mMenuEdit";
            this.mMenuEdit.Size = new System.Drawing.Size(211, 22);
            this.mMenuEdit.Text = "Edit";
            this.mMenuEdit.Click += new System.EventHandler(this.OnEditClicked);
            // 
            // mMenuRename
            // 
            this.mMenuRename.Name = "mMenuRename";
            this.mMenuRename.Size = new System.Drawing.Size(211, 22);
            this.mMenuRename.Text = "Rename";
            this.mMenuRename.Click += new System.EventHandler(this.OnRenameClicked);
            // 
            // mMenuDelete
            // 
            this.mMenuDelete.Image = global::Luminate.Properties.Resources.delete_16;
            this.mMenuDelete.Name = "mMenuDelete";
            this.mMenuDelete.Size = new System.Drawing.Size(211, 22);
            this.mMenuDelete.Text = "Delete";
            this.mMenuDelete.Click += new System.EventHandler(this.OnDeleteClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(208, 6);
            // 
            // setAsDefaultApplicationToolStripMenuItem
            // 
            this.setAsDefaultApplicationToolStripMenuItem.Name = "setAsDefaultApplicationToolStripMenuItem";
            this.setAsDefaultApplicationToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.setAsDefaultApplicationToolStripMenuItem.Text = "Set As Default Application";
            this.setAsDefaultApplicationToolStripMenuItem.Click += new System.EventHandler(this.OnDefaultAppSelected);
            // 
            // setAsProjectRunnableToolStripMenuItem
            // 
            this.setAsProjectRunnableToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("setAsProjectRunnableToolStripMenuItem.Image")));
            this.setAsProjectRunnableToolStripMenuItem.Name = "setAsProjectRunnableToolStripMenuItem";
            this.setAsProjectRunnableToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.setAsProjectRunnableToolStripMenuItem.Text = "Set As Project Runnable";
            this.setAsProjectRunnableToolStripMenuItem.Click += new System.EventHandler(this.OnSetRunnable);
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.Image = global::Luminate.Properties.Resources.build;
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.buildToolStripMenuItem.Text = "Build";
            this.buildToolStripMenuItem.Click += new System.EventHandler(this.OnBuildClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(208, 6);
            // 
            // setBeginFileToolStripMenuItem
            // 
            this.setBeginFileToolStripMenuItem.Image = global::Luminate.Properties.Resources.file_begin;
            this.setBeginFileToolStripMenuItem.Name = "setBeginFileToolStripMenuItem";
            this.setBeginFileToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.setBeginFileToolStripMenuItem.Text = "Set Begin File";
            this.setBeginFileToolStripMenuItem.Click += new System.EventHandler(this.OnSetBegin);
            // 
            // setEndFileToolStripMenuItem
            // 
            this.setEndFileToolStripMenuItem.Image = global::Luminate.Properties.Resources.file_end;
            this.setEndFileToolStripMenuItem.Name = "setEndFileToolStripMenuItem";
            this.setEndFileToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.setEndFileToolStripMenuItem.Text = "Set End File";
            this.setEndFileToolStripMenuItem.Click += new System.EventHandler(this.OnSetEnd);
            // 
            // setAsOutputFileToolStripMenuItem
            // 
            this.setAsOutputFileToolStripMenuItem.Image = global::Luminate.Properties.Resources.file_app;
            this.setAsOutputFileToolStripMenuItem.Name = "setAsOutputFileToolStripMenuItem";
            this.setAsOutputFileToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.setAsOutputFileToolStripMenuItem.Text = "Set As Output File";
            this.setAsOutputFileToolStripMenuItem.Click += new System.EventHandler(this.OnSetOutput);
            // 
            // untagFileToolStripMenuItem
            // 
            this.untagFileToolStripMenuItem.Image = global::Luminate.Properties.Resources.file;
            this.untagFileToolStripMenuItem.Name = "untagFileToolStripMenuItem";
            this.untagFileToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.untagFileToolStripMenuItem.Text = "Untag File";
            this.untagFileToolStripMenuItem.Click += new System.EventHandler(this.UntagAFile);
            // 
            // ignoredToolStripMenuItem
            // 
            this.ignoredToolStripMenuItem.CheckOnClick = true;
            this.ignoredToolStripMenuItem.Name = "ignoredToolStripMenuItem";
            this.ignoredToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.ignoredToolStripMenuItem.Text = "Ignored";
            this.ignoredToolStripMenuItem.Click += new System.EventHandler(this.OnIgnoredClick);
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // SolutionExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.mTree);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SolutionExplorer";
            this.Text = "Solution Explorer";
            this.mContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView mTree;
        private System.Windows.Forms.ContextMenuStrip mContext;
        private System.Windows.Forms.ToolStripMenuItem mMenuCreateFolder;
        private System.Windows.Forms.ToolStripMenuItem mMenuEdit;
        private System.Windows.Forms.ToolStripMenuItem mMenuDelete;
        private System.Windows.Forms.ToolStripMenuItem mMenuRename;
        private System.Windows.Forms.ToolStripMenuItem mMenuCreateFile;
        private System.Windows.Forms.ToolStripMenuItem mMenuClose;
        private System.Windows.Forms.ImageList imageListTreeView;
        private System.Windows.Forms.ImageList imageListDrag;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem setAsProjectRunnableToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem setBeginFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setEndFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAsOutputFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mCreateClass;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildOptionsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem untagFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAsDefaultApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoredToolStripMenuItem;
    }
}