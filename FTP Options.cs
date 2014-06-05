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
    /// The options class for uploading the file when we are done saving it
    /// </summary>
    public partial class FTP_Options : Form
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        public FTP_Options()
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
        /// When shown we fill it with project data
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToScreen();
            BringToFront();

            Project proj = SolutionExplorer.Singleton.ActiveProject;
            textLocation.Text = proj.FTPLocation;
            textPassword.Text = proj.FTPPassword;
            textUsername.Text = proj.FTPUser;
            checkEnabled.Checked = proj.UseFTP;
        }

        /// <summary>
        /// Not allowed to close it.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            Hide();
        }

        /// <summary>
        /// If escape clicked we cancel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                buttonCancel.PerformClick();

            base.OnKeyUp(e);
        }

        /// <summary>
        /// When we click on OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClick(object sender, EventArgs e)
        {
            Project proj = SolutionExplorer.Singleton.ActiveProject;
            proj.FTPLocation = textLocation.Text;
            proj.FTPPassword = textPassword.Text;
            proj.FTPUser = textUsername.Text;
            proj.UseFTP = checkEnabled.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Hide();
        }

        /// <summary>
        /// When we click on cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Hide();
        }
    }
}
