namespace Luminate
{
    partial class NewProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProject));
            this.mTextBoxName = new System.Windows.Forms.TextBox();
            this.mFolders = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.mButtonBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mTextBoxBrowse = new System.Windows.Forms.TextBox();
            this.mButtonOk = new System.Windows.Forms.Button();
            this.mButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mTextBoxName
            // 
            this.mTextBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mTextBoxName.Location = new System.Drawing.Point(65, 6);
            this.mTextBoxName.Name = "mTextBoxName";
            this.mTextBoxName.Size = new System.Drawing.Size(259, 20);
            this.mTextBoxName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // mButtonBrowse
            // 
            this.mButtonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonBrowse.Location = new System.Drawing.Point(299, 28);
            this.mButtonBrowse.Name = "mButtonBrowse";
            this.mButtonBrowse.Size = new System.Drawing.Size(25, 23);
            this.mButtonBrowse.TabIndex = 2;
            this.mButtonBrowse.Text = "...";
            this.mButtonBrowse.UseVisualStyleBackColor = true;
            this.mButtonBrowse.Click += new System.EventHandler(this.OnBrowseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Directory";
            // 
            // mTextBoxBrowse
            // 
            this.mTextBoxBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mTextBoxBrowse.Location = new System.Drawing.Point(65, 31);
            this.mTextBoxBrowse.Name = "mTextBoxBrowse";
            this.mTextBoxBrowse.Size = new System.Drawing.Size(228, 20);
            this.mTextBoxBrowse.TabIndex = 1;
            // 
            // mButtonOk
            // 
            this.mButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonOk.Location = new System.Drawing.Point(262, 61);
            this.mButtonOk.Name = "mButtonOk";
            this.mButtonOk.Size = new System.Drawing.Size(62, 23);
            this.mButtonOk.TabIndex = 2;
            this.mButtonOk.Text = "Ok";
            this.mButtonOk.UseVisualStyleBackColor = true;
            this.mButtonOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // mButtonCancel
            // 
            this.mButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonCancel.Location = new System.Drawing.Point(194, 61);
            this.mButtonCancel.Name = "mButtonCancel";
            this.mButtonCancel.Size = new System.Drawing.Size(62, 23);
            this.mButtonCancel.TabIndex = 3;
            this.mButtonCancel.Text = "Cancel";
            this.mButtonCancel.UseVisualStyleBackColor = true;
            this.mButtonCancel.Click += new System.EventHandler(this.OnCancelClicked);
            // 
            // NewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 96);
            this.ControlBox = false;
            this.Controls.Add(this.mButtonCancel);
            this.Controls.Add(this.mButtonOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mTextBoxBrowse);
            this.Controls.Add(this.mButtonBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mTextBoxName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewProject";
            this.Text = "New Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mTextBoxName;
        private System.Windows.Forms.FolderBrowserDialog mFolders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button mButtonBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mTextBoxBrowse;
        private System.Windows.Forms.Button mButtonOk;
        private System.Windows.Forms.Button mButtonCancel;
    }
}