using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Luminate
{
    /// <summary>
    /// New project Form
    /// </summary>
    public partial class NewProject : Form
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        public NewProject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hides the window when we click escape
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Hide();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// When we click cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelClicked(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Hide();
        }

        /// <summary>
        /// When escape pressed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                mButtonCancel.PerformClick();

            base.OnKeyUp(e);
        }

        /// <summary>
        /// When we click ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClick(object sender, EventArgs e)
        {
            mTextBoxBrowse.Text = mTextBoxBrowse.Text.Trim();
            mTextBoxName.Text = mTextBoxName.Text.Trim();

            if ( mTextBoxBrowse.Text == "" )
            {
                 MessageBox.Show("Please select a workig directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

            if (mTextBoxName.Text == "")
            {
                MessageBox.Show("Please enter a project name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(mTextBoxBrowse.Text + "\\" + mTextBoxName.Text + ".lum"))
            {
                MessageBox.Show("There is already a project with that name in the folder. Please use a different name", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(mTextBoxBrowse.Text))
            {
                if (MessageBox.Show("The directory: " + mTextBoxBrowse.Text + " does not exist, do you want to create it?", "Does not exist",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    //Create directory
                    try
                    {
                        //Create dir
                        Directory.CreateDirectory(mTextBoxBrowse.Text);
                    }
                    catch ( Exception ex )
                    {
                        //Failed so return
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                    return;
            }

            //Directory created so lets create sub directories
            if ( !Directory.Exists( mTextBoxBrowse.Text + "\\src" ) )
                Directory.CreateDirectory(mTextBoxBrowse.Text + "\\src");
            if (!Directory.Exists(mTextBoxBrowse.Text + "\\bin"))
                Directory.CreateDirectory(mTextBoxBrowse.Text + "\\bin");
            if (!File.Exists(mTextBoxBrowse.Text + "\\bin\\index.html"))
            {
                // Example #3: Write only some strings in an array to a file.
                using (StreamWriter file = new StreamWriter(mTextBoxBrowse.Text + "\\bin\\index.html") )
                {
                    file.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"");
                    file.WriteLine("\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                    file.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" dir=\"ltr\" lang=\"en\">");
                    file.WriteLine("<head>");
                    file.WriteLine("<script type='text/javascript' src='application.js'></script>");
                    file.WriteLine("</head>");
                    file.WriteLine("<body></body>");
                    file.WriteLine("</html>");
                }
            }
            if (!File.Exists(mTextBoxBrowse.Text + "\\bin\\application.js"))
            {
                // Writes a blank application file
                using (StreamWriter file = new StreamWriter(mTextBoxBrowse.Text + "\\bin\\application.js"))
                {
                }
            }
            if (!File.Exists(mTextBoxBrowse.Text + "\\src\\begin.js"))
            {
                // Writes a blank begin file
                using (StreamWriter file = new StreamWriter(mTextBoxBrowse.Text + "\\src\\begin.js"))
                {
                    file.WriteLine("/***This file is placed before all other JS files before they are merged. Its a good place to put global functions or utilities.*/");
                    file.WriteLine("__definedClasses = new Array();\n");
                    file.WriteLine("/** This is a helper function to create subclasses */");
                    file.WriteLine("function subclass(subClass, baseClass) {");
                    file.WriteLine("\tfunction inheritance() {}");
                    file.WriteLine("\tinheritance.prototype = baseClass.prototype;");
                    file.WriteLine("\tsubClass.prototype = new inheritance();");

                    file.WriteLine("\tsubClass.prototype.constructor = subClass;");
                    file.WriteLine("\tsubClass.baseConstructor = baseClass;");
                    file.WriteLine("\tsubClass.superClass = baseClass.prototype;");
                    file.WriteLine("\tif (__definedClasses[subClass])");
                    file.WriteLine("\t\talert( subClass + \" has already been defined. Make sure your classes arn't named twice.\" );");
                    file.WriteLine("\t__definedClasses[subClass] = true;");
                    file.WriteLine("}");
                }
            }

            if (!File.Exists(mTextBoxBrowse.Text + "\\src\\end.js"))
            {
                // Writes a blank end file
                using (StreamWriter file = new StreamWriter(mTextBoxBrowse.Text + "\\src\\end.js"))
                {
                }
            }
           
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Hide();
        }

        /// <summary>
        /// Gets the name of the project
        /// </summary>
        public string ProjecName { get { return mTextBoxName.Text; } }

        /// <summary>
        /// Gets the directory of the project
        /// </summary>
        public string ProjecDirectory { get { return mTextBoxBrowse.Text; } }

        /// <summary>
        /// When we click browse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBrowseClick(object sender, EventArgs e)
        {
            if (mFolders.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                mTextBoxBrowse.Text = mFolders.SelectedPath.Trim();
            }
        }

        /// <summary>
        /// Center to screen
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToScreen();
            mTextBoxBrowse.Text = "";
            mTextBoxName.Text = "";
            mTextBoxName.Focus();
        }
    }
}
