using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Luminate
{
    /// <summary>
    /// A form for setting general preferences
    /// </summary>
    public partial class Preferences : Form
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        public Preferences()
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
        /// When we click ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClick(object sender, EventArgs e)
        {
            SolutionPreferences prefs = Luminate.Singleton.Preferences;

            prefs.useAutoComplete = checkBox1.Checked;
            prefs.additionalKeywords.Clear();
            prefs.autoBraceClosure = autoBraceClosure.Checked;

            string[] keywords = textBoxKeywords.Text.Split(',');
            foreach (string keyword in keywords)
                prefs.additionalKeywords.Add(keyword.Trim());

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Hide();
        }

        /// <summary>
        /// On key up
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                mButtonCancel.PerformClick();

            base.OnKeyUp(e);
        }

        /// <summary>
        /// On Cancel click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Hide();
        }

        /// <summary>
        /// Center to screen
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToScreen();
            SolutionPreferences prefs = Luminate.Singleton.Preferences;
            checkBox1.Checked = prefs.useAutoComplete;
            textBoxKeywords.Text = "";
            autoBraceClosure.Checked = prefs.autoBraceClosure;

            foreach ( string keyword in prefs.additionalKeywords )
                textBoxKeywords.Text += keyword.Trim() + ",";

            if ( textBoxKeywords.Text.Length > 0 )
                textBoxKeywords.Text = textBoxKeywords.Text.Substring(0, textBoxKeywords.Text.Length - 1);
        }
    }
}
