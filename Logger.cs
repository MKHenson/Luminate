using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Luminate
{
    /// <summary>
    /// A simple class for logging messages
    /// </summary>
    public partial class Logger : DockContent
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void logMessage(string message, MessageType type = MessageType.WARNING, object tag = null);

        public enum MessageType
        {
            /// <summary>
            /// Log an error message
            /// </summary>
            ERROR = 0,

            /// <summary>
            /// Log an message
            /// </summary>
            MESSAGE = 1,

            /// <summary>
            /// Log an error message
            /// </summary>
            WARNING = 2
        }

        private static Logger mSingleton;

        /// <summary>
        /// Simple constructor
        /// </summary>
        public Logger()
        {
            InitializeComponent();

            this.ContextMenuStrip = contextMenuStrip;
        }

        /// <summary>
        /// When we click clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClearClicked( object sender, EventArgs e )
        {
            foreach (ListViewItem item in mList.Items)
                if (item.Tag is SearchResult)
                    (item.Tag as SearchResult).Dispose();

            mList.Items.Clear();
        }

        /// <summary>
        /// Log a message to message box
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void Log(string message, MessageType type = MessageType.WARNING, object tag = null)
        {
            ListViewItem toAdd = new ListViewItem(message);
            mList.SmallImageList = imageList;
            toAdd.Tag = tag;

            if (type == MessageType.ERROR)
                toAdd.ImageIndex = 0;
            else if (type == MessageType.MESSAGE)
                toAdd.ImageIndex = 2;
            else
                toAdd.ImageIndex = 1;

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.mList.InvokeRequired)
            {
                if (this.mList.IsDisposed == false)
                {
                    logMessage func = new logMessage(Log);
                    this.Invoke(func, new object[] { message, type, tag });
                }
            }
            else
                mList.Items.Add(toAdd);
        }

        /// <summary>
        /// Closes the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseClick(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Clears all messages
        /// </summary>
        public void Clear()
        {
            mList.Items.Clear();
        }

        /// <summary>
        /// Gets the singleton reference
        /// </summary>
        public static Logger Singleton
        {
            get
            {
                if (mSingleton == null)
                    mSingleton = new Logger();

                return mSingleton;
            }
        }

        /// <summary>
        /// When we right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                contextMenuStrip.Show(new Point(Cursor.Position.X, Cursor.Position.Y));
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

        private void OnMouseDClick(object sender, MouseEventArgs e)
        {
            if (mList.SelectedItems.Count > 0 && mList.SelectedItems[0].Tag != null )
            {
                //If a search result, we show the document.
                if (mList.SelectedItems[0].Tag is SearchResult)
                {
                    SearchResult result = mList.SelectedItems[0].Tag as SearchResult;
                    result.document.Show();
                    
                    result.range.GotoStart();
                    result.range.Select();

                    return;
                }

                string doc = ((string[])(mList.SelectedItems[0].Tag))[0];
                int line = int.Parse( ((string[])(mList.SelectedItems[0].Tag))[1] );

                string fileContent = null;
                doc = doc.Replace("file:///", "");

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                try
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(doc))
                        fileContent = sr.ReadToEnd();
                }
                catch
                {
                    return;
                }

                Document d = Luminate.Singleton.CreateDocument(fileContent, doc);
                d.canSave = false;
                d.Show();
                if (d != null)
                {
                    d.Scintilla.Selection.Start = d.Scintilla.Lines[line - 1].StartPosition;
                    d.Scintilla.Selection.End = d.Scintilla.Lines[line - 1].EndPosition;
                    d.Scintilla.Scrolling.ScrollToCaret();
                    d.Scintilla.BackColor = SystemColors.AppWorkspace;
                    //d.Scintilla.Enabled = false; //Do not allow for editing of this document.
                }
            }
        }

        private void copyMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mList.SelectedItems.Count > 0)
                Clipboard.SetData(System.Windows.Forms.DataFormats.Text, mList.SelectedItems[0].Text);
        }
    }
}
