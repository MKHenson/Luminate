namespace Luminate
{
    partial class Document
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Document));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllButThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.wordWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextComment = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.commentClass = new System.Windows.Forms.ToolStripMenuItem();
            this.commentMemberFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.commentMemberVariable = new System.Windows.Forms.ToolStripMenuItem();
            this.commentStaticFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.commentStaticVariable = new System.Windows.Forms.ToolStripMenuItem();
            this.commentEnum = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.contextComment.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem,
            this.closeAllToolStripMenuItem,
            this.closeAllButThisToolStripMenuItem,
            this.toolStripSeparator2,
            this.wordWrapToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(168, 224);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextOpening);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::Luminate.Properties.Resources.copy_small;
            this.copyToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.OnCopyClick);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = global::Luminate.Properties.Resources.scissors_small;
            this.cutToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.OnCutClick);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::Luminate.Properties.Resources.paste_small;
            this.pasteToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.OnPasteClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::Luminate.Properties.Resources.Save_20;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.OnSaveClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::Luminate.Properties.Resources.close_16;
            this.closeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Image = global::Luminate.Properties.Resources.close_16;
            this.closeAllToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.OnCloseAllClick);
            // 
            // closeAllButThisToolStripMenuItem
            // 
            this.closeAllButThisToolStripMenuItem.Image = global::Luminate.Properties.Resources.close_16;
            this.closeAllButThisToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.closeAllButThisToolStripMenuItem.Name = "closeAllButThisToolStripMenuItem";
            this.closeAllButThisToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.closeAllButThisToolStripMenuItem.Text = "Close All But this";
            this.closeAllButThisToolStripMenuItem.Click += new System.EventHandler(this.OnRemoveAllButClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(164, 6);
            // 
            // wordWrapToolStripMenuItem
            // 
            this.wordWrapToolStripMenuItem.CheckOnClick = true;
            this.wordWrapToolStripMenuItem.Name = "wordWrapToolStripMenuItem";
            this.wordWrapToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.wordWrapToolStripMenuItem.Text = "Word Wrap";
            this.wordWrapToolStripMenuItem.Click += new System.EventHandler(this.OnWordWrapClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "function.png");
            this.imageList.Images.SetKeyName(1, "custom.png");
            this.imageList.Images.SetKeyName(2, "class.png");
            this.imageList.Images.SetKeyName(3, "variable.png");
            this.imageList.Images.SetKeyName(4, "enum.png");
            this.imageList.Images.SetKeyName(5, "keyword.png");
            // 
            // contextComment
            // 
            this.contextComment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.commentClass,
            this.commentMemberFunction,
            this.commentMemberVariable,
            this.commentStaticFunction,
            this.commentStaticVariable,
            this.commentEnum});
            this.contextComment.Name = "contextComment";
            this.contextComment.Size = new System.Drawing.Size(170, 158);
            this.contextComment.Opening += new System.ComponentModel.CancelEventHandler(this.OnCommentContextOpening);
            this.contextComment.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnCommentItemClicked);
            // 
            // commentClass
            // 
            this.commentClass.Name = "commentClass";
            this.commentClass.Size = new System.Drawing.Size(169, 22);
            this.commentClass.Text = "Class";
            // 
            // commentMemberFunction
            // 
            this.commentMemberFunction.Name = "commentMemberFunction";
            this.commentMemberFunction.Size = new System.Drawing.Size(169, 22);
            this.commentMemberFunction.Text = "Member Function";
            // 
            // commentMemberVariable
            // 
            this.commentMemberVariable.Name = "commentMemberVariable";
            this.commentMemberVariable.Size = new System.Drawing.Size(169, 22);
            this.commentMemberVariable.Text = "Member Variable";
            // 
            // commentStaticFunction
            // 
            this.commentStaticFunction.Name = "commentStaticFunction";
            this.commentStaticFunction.Size = new System.Drawing.Size(169, 22);
            this.commentStaticFunction.Text = "Static Function";
            // 
            // commentStaticVariable
            // 
            this.commentStaticVariable.Name = "commentStaticVariable";
            this.commentStaticVariable.Size = new System.Drawing.Size(169, 22);
            this.commentStaticVariable.Text = "Static Variable";
            // 
            // commentEnum
            // 
            this.commentEnum.Name = "commentEnum";
            this.commentEnum.Size = new System.Drawing.Size(169, 22);
            this.commentEnum.Text = "Enum";
            // 
            // Document
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Document";
            this.Text = "Document Viewer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.contextMenuStrip.ResumeLayout(false);
            this.contextComment.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllButThisToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem wordWrapToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextComment;
        private System.Windows.Forms.ToolStripMenuItem commentClass;
        private System.Windows.Forms.ToolStripMenuItem commentMemberFunction;
        private System.Windows.Forms.ToolStripMenuItem commentMemberVariable;
        private System.Windows.Forms.ToolStripMenuItem commentStaticFunction;
        private System.Windows.Forms.ToolStripMenuItem commentStaticVariable;
        private System.Windows.Forms.ToolStripMenuItem commentEnum;
    }
}