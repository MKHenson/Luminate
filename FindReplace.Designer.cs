namespace Luminate
{
    partial class FindReplace
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
            this.textBoxFind = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonCurrent = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxRegex = new System.Windows.Forms.CheckBox();
            this.checkBoxWord = new System.Windows.Forms.CheckBox();
            this.checkboxCase = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxReplace = new System.Windows.Forms.TextBox();
            this.buttonFindNext = new System.Windows.Forms.Button();
            this.buttonFindPrevious = new System.Windows.Forms.Button();
            this.buttonFindAll = new System.Windows.Forms.Button();
            this.buttonReplaceNext = new System.Windows.Forms.Button();
            this.buttonReplacePrevious = new System.Windows.Forms.Button();
            this.buttonReplaceAll = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFind
            // 
            this.textBoxFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFind.Location = new System.Drawing.Point(12, 25);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.Size = new System.Drawing.Size(209, 20);
            this.textBoxFind.TabIndex = 0;
            this.textBoxFind.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Find what:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButtonAll);
            this.groupBox1.Controls.Add(this.radioButtonCurrent);
            this.groupBox1.Location = new System.Drawing.Point(12, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 46);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Seach in:";
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Checked = true;
            this.radioButtonAll.Location = new System.Drawing.Point(90, 19);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(60, 17);
            this.radioButtonAll.TabIndex = 4;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All Files";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonCurrent
            // 
            this.radioButtonCurrent.AutoSize = true;
            this.radioButtonCurrent.Location = new System.Drawing.Point(6, 19);
            this.radioButtonCurrent.Name = "radioButtonCurrent";
            this.radioButtonCurrent.Size = new System.Drawing.Size(78, 17);
            this.radioButtonCurrent.TabIndex = 3;
            this.radioButtonCurrent.Text = "Current File";
            this.radioButtonCurrent.UseVisualStyleBackColor = true;
            this.radioButtonCurrent.CheckedChanged += new System.EventHandler(this.OnRadioChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBoxRegex);
            this.groupBox2.Controls.Add(this.checkBoxWord);
            this.groupBox2.Controls.Add(this.checkboxCase);
            this.groupBox2.Location = new System.Drawing.Point(15, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(211, 88);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // checkBoxRegex
            // 
            this.checkBoxRegex.AutoSize = true;
            this.checkBoxRegex.Location = new System.Drawing.Point(6, 65);
            this.checkBoxRegex.Name = "checkBoxRegex";
            this.checkBoxRegex.Size = new System.Drawing.Size(139, 17);
            this.checkBoxRegex.TabIndex = 6;
            this.checkBoxRegex.Text = "Use Regular Expression";
            this.checkBoxRegex.UseVisualStyleBackColor = true;
            // 
            // checkBoxWord
            // 
            this.checkBoxWord.AutoSize = true;
            this.checkBoxWord.Location = new System.Drawing.Point(6, 42);
            this.checkBoxWord.Name = "checkBoxWord";
            this.checkBoxWord.Size = new System.Drawing.Size(119, 17);
            this.checkBoxWord.TabIndex = 5;
            this.checkBoxWord.Text = "Match Whole Word";
            this.checkBoxWord.UseVisualStyleBackColor = true;
            // 
            // checkboxCase
            // 
            this.checkboxCase.AutoSize = true;
            this.checkboxCase.Location = new System.Drawing.Point(6, 19);
            this.checkboxCase.Name = "checkboxCase";
            this.checkboxCase.Size = new System.Drawing.Size(83, 17);
            this.checkboxCase.TabIndex = 4;
            this.checkboxCase.Text = "Match Case";
            this.checkboxCase.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Replace with:";
            // 
            // textBoxReplace
            // 
            this.textBoxReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReplace.Location = new System.Drawing.Point(12, 64);
            this.textBoxReplace.Name = "textBoxReplace";
            this.textBoxReplace.Size = new System.Drawing.Size(209, 20);
            this.textBoxReplace.TabIndex = 4;
            // 
            // buttonFindNext
            // 
            this.buttonFindNext.Location = new System.Drawing.Point(12, 236);
            this.buttonFindNext.Name = "buttonFindNext";
            this.buttonFindNext.Size = new System.Drawing.Size(104, 23);
            this.buttonFindNext.TabIndex = 6;
            this.buttonFindNext.Text = "Find Next";
            this.buttonFindNext.UseVisualStyleBackColor = true;
            this.buttonFindNext.Click += new System.EventHandler(this.OnFindNext);
            // 
            // buttonFindPrevious
            // 
            this.buttonFindPrevious.Location = new System.Drawing.Point(12, 265);
            this.buttonFindPrevious.Name = "buttonFindPrevious";
            this.buttonFindPrevious.Size = new System.Drawing.Size(104, 23);
            this.buttonFindPrevious.TabIndex = 7;
            this.buttonFindPrevious.Text = "Find Previous";
            this.buttonFindPrevious.UseVisualStyleBackColor = true;
            this.buttonFindPrevious.Click += new System.EventHandler(this.OnFindPrevious);
            // 
            // buttonFindAll
            // 
            this.buttonFindAll.Location = new System.Drawing.Point(12, 294);
            this.buttonFindAll.Name = "buttonFindAll";
            this.buttonFindAll.Size = new System.Drawing.Size(104, 23);
            this.buttonFindAll.TabIndex = 8;
            this.buttonFindAll.Text = "Find All";
            this.buttonFindAll.UseVisualStyleBackColor = true;
            this.buttonFindAll.Click += new System.EventHandler(this.OnFindAll);
            // 
            // buttonReplaceNext
            // 
            this.buttonReplaceNext.Location = new System.Drawing.Point(122, 236);
            this.buttonReplaceNext.Name = "buttonReplaceNext";
            this.buttonReplaceNext.Size = new System.Drawing.Size(104, 23);
            this.buttonReplaceNext.TabIndex = 9;
            this.buttonReplaceNext.Text = "Replace Next";
            this.buttonReplaceNext.UseVisualStyleBackColor = true;
            this.buttonReplaceNext.Click += new System.EventHandler(this.OnReplaceNext);
            // 
            // buttonReplacePrevious
            // 
            this.buttonReplacePrevious.Location = new System.Drawing.Point(122, 265);
            this.buttonReplacePrevious.Name = "buttonReplacePrevious";
            this.buttonReplacePrevious.Size = new System.Drawing.Size(104, 23);
            this.buttonReplacePrevious.TabIndex = 10;
            this.buttonReplacePrevious.Text = "Replace Previous";
            this.buttonReplacePrevious.UseVisualStyleBackColor = true;
            this.buttonReplacePrevious.Click += new System.EventHandler(this.OnReplacePrevious);
            // 
            // buttonReplaceAll
            // 
            this.buttonReplaceAll.Location = new System.Drawing.Point(122, 294);
            this.buttonReplaceAll.Name = "buttonReplaceAll";
            this.buttonReplaceAll.Size = new System.Drawing.Size(104, 23);
            this.buttonReplaceAll.TabIndex = 11;
            this.buttonReplaceAll.Text = "Replace All";
            this.buttonReplaceAll.UseVisualStyleBackColor = true;
            this.buttonReplaceAll.Click += new System.EventHandler(this.OnReplaceAll);
            // 
            // FindReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 327);
            this.Controls.Add(this.buttonReplaceAll);
            this.Controls.Add(this.buttonReplacePrevious);
            this.Controls.Add(this.buttonReplaceNext);
            this.Controls.Add(this.buttonFindAll);
            this.Controls.Add(this.buttonFindPrevious);
            this.Controls.Add(this.buttonFindNext);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxReplace);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFind);
            this.Name = "FindReplace";
            this.Text = "Find & Replace";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.RadioButton radioButtonCurrent;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxWord;
        private System.Windows.Forms.CheckBox checkboxCase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxReplace;
        private System.Windows.Forms.Button buttonFindPrevious;
        private System.Windows.Forms.Button buttonFindAll;
        private System.Windows.Forms.Button buttonReplaceNext;
        private System.Windows.Forms.Button buttonReplacePrevious;
        private System.Windows.Forms.Button buttonReplaceAll;
        private System.Windows.Forms.CheckBox checkBoxRegex;
        public System.Windows.Forms.TextBox textBoxFind;
        public System.Windows.Forms.Button buttonFindNext;
    }
}