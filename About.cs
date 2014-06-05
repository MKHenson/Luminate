using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Luminate
{
    /// <summary>
    /// A Simple about form
    /// </summary>
    public partial class About : Form
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        public About()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hide the form instead
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            Hide();
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
        /// If esc pressed click ok
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                buttonOk.PerformClick();

            base.OnKeyUp(e);
        }

        /// <summary>
        /// Center to screen
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToScreen();
            BringToFront();
        }

        /// <summary>
        /// Hide the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClick(object sender, EventArgs e)
        {
            Hide();
        }

        /// <summary>
        /// Navigate to link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start( ((LinkLabel)(sender)).Text );
        }
    }
}
