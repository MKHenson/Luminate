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
    /// This form is used to find and replace text on a scintilla document
    /// </summary>
    public partial class FindReplace : Form
    {
        private static FindReplace mSingleton;
        public Document mDocument; 

        /// <summary>
        /// Simple constructor
        /// </summary>
        protected FindReplace()
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
        /// Hide the form instead of closing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// Gets the static instance of the singleton
        /// </summary>
        public static FindReplace get
        {
            get 
            {
                if (mSingleton == null)
                    mSingleton = new FindReplace();

                return mSingleton;
            }
        }

        /// <summary>
        /// Gets the static instance of the singleton
        /// </summary>
        public Document document
        {
            get { return mDocument; }
            set { mDocument = value; }
        }

        /// <summary>
        /// Find the next term
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFindNext(object sender, EventArgs e)
        {
            if (document == null)
                return;

            if (textBoxFind.Text == "")
                return;

            ScintillaNET.SearchFlags flags = ScintillaNET.SearchFlags.Empty;
            if (checkboxCase.Checked)
                flags |= ScintillaNET.SearchFlags.MatchCase;
            if (checkBoxWord.Checked)
                flags |= ScintillaNET.SearchFlags.WholeWord;
            if (checkBoxRegex.Checked)
                flags |= ScintillaNET.SearchFlags.RegExp;

            ScintillaNET.Range range = document.Scintilla.FindReplace.FindNext(textBoxFind.Text, true, flags);
            if (range != null)
            {
                range.GotoStart();
                range.Select();
            }
        }

        /// <summary>
        /// Finds the previous text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFindPrevious(object sender, EventArgs e)
        {
            if (document == null)
                return;

            if (textBoxFind.Text == "")
                return;

            ScintillaNET.SearchFlags flags = ScintillaNET.SearchFlags.Empty;
            if (checkboxCase.Checked)
                flags |= ScintillaNET.SearchFlags.MatchCase;
            if (checkBoxWord.Checked)
                flags |= ScintillaNET.SearchFlags.WholeWord;
            if (checkBoxRegex.Checked)
                flags |= ScintillaNET.SearchFlags.RegExp;

            ScintillaNET.Range range = document.Scintilla.FindReplace.FindPrevious(textBoxFind.Text, true, flags);
            if (range != null)
            {
                range.GotoStart();
                range.Select();
            }
        }

        /// <summary>
        /// Finds all references
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFindAll(object sender, EventArgs e)
        {
            if (document == null)
                return;

            if (textBoxFind.Text == "")
                return;

            ScintillaNET.SearchFlags flags = ScintillaNET.SearchFlags.Empty;
            if (checkboxCase.Checked)
                flags |= ScintillaNET.SearchFlags.MatchCase;
            if (checkBoxWord.Checked)
                flags |= ScintillaNET.SearchFlags.WholeWord;
            if (checkBoxRegex.Checked)
                flags |= ScintillaNET.SearchFlags.RegExp;
            
            List<ScintillaNET.Range> ranges = document.Scintilla.FindReplace.FindAll(textBoxFind.Text, flags);
            Logger.Singleton.Clear();

            //Log each result
            foreach (ScintillaNET.Range range in ranges)
                Logger.Singleton.Log("'" + range.Text + "' in " + document.Name + " on line " + range.StartingLine.Number, Logger.MessageType.MESSAGE, new SearchResult(range, document.file, document));
        }

        /// <summary>
        /// Replaces next text from cursor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReplaceNext(object sender, EventArgs e)
        {
            if (document == null)
                return;

            if (textBoxFind.Text == "")
                return;

            ScintillaNET.SearchFlags flags = ScintillaNET.SearchFlags.Empty;
            if (checkboxCase.Checked)
                flags |= ScintillaNET.SearchFlags.MatchCase;
            if (checkBoxWord.Checked)
                flags |= ScintillaNET.SearchFlags.WholeWord;
            if (checkBoxRegex.Checked)
                flags |= ScintillaNET.SearchFlags.RegExp;

            ScintillaNET.Range range = document.Scintilla.FindReplace.ReplaceNext(textBoxFind.Text, textBoxReplace.Text, true, flags);

            if (range != null)
            {
                range.GotoStart();
                range.Select();
            }
        }

        /// <summary>
        /// Replaces previous text from cursor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReplacePrevious(object sender, EventArgs e)
        {
            if (document == null)
                return;

            if (textBoxFind.Text == "")
                return;

            ScintillaNET.SearchFlags flags = ScintillaNET.SearchFlags.Empty;
            if (checkboxCase.Checked)
                flags |= ScintillaNET.SearchFlags.MatchCase;
            if (checkBoxWord.Checked)
                flags |= ScintillaNET.SearchFlags.WholeWord;
            if (checkBoxRegex.Checked)
                flags |= ScintillaNET.SearchFlags.RegExp;

            ScintillaNET.Range range = document.Scintilla.FindReplace.ReplacePrevious(textBoxFind.Text, textBoxReplace.Text, true, flags);

            if (range != null)
            {
                range.GotoStart();
                range.Select();
            }
        }

        /// <summary>
        /// Replaces all text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReplaceAll(object sender, EventArgs e)
        {
            if (document == null)
                return;

            if (textBoxFind.Text == "")
                return;

            ScintillaNET.SearchFlags flags = ScintillaNET.SearchFlags.Empty;
            if (checkboxCase.Checked)
                flags |= ScintillaNET.SearchFlags.MatchCase;
            if (checkBoxWord.Checked)
                flags |= ScintillaNET.SearchFlags.WholeWord;
            if (checkBoxRegex.Checked)
                flags |= ScintillaNET.SearchFlags.RegExp;

            List<ScintillaNET.Range> ranges = document.Scintilla.FindReplace.ReplaceAll(textBoxFind.Text, textBoxReplace.Text, flags);
            Logger.Singleton.Clear();

            //Log each result
            foreach (ScintillaNET.Range range in ranges)
                Logger.Singleton.Log("'" + range.Text + "' in " + document.Name + " on line " + range.StartingLine.Number, Logger.MessageType.MESSAGE, new SearchResult(range, document.file, document));
        }

        /// <summary>
        /// If enter is pressed we hit the find button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                buttonFindNext.PerformClick();
        }

        /// <summary>
        /// When the radio buttons change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRadioChanged(object sender, EventArgs e)
        {
            if (radioButtonAll.Checked)
            {
                buttonFindNext.Enabled = false;
                buttonFindPrevious.Enabled = false;
                buttonFindAll.Enabled = true;

                buttonReplaceNext.Enabled = false;
                buttonReplacePrevious.Enabled = false;
                buttonReplaceAll.Enabled = true;
            }
            else
            {
                buttonFindNext.Enabled = true;
                buttonFindPrevious.Enabled = true;
                buttonFindAll.Enabled = true;

                buttonReplaceNext.Enabled = true;
                buttonReplacePrevious.Enabled = true;
                buttonReplaceAll.Enabled = true;
            }
        }
    }
}
