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
    /// The build options form
    /// </summary>
    public partial class BuildOptions : Form
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        public BuildOptions()
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

            Text = proj.Name + " Options";
            
            listBoxProjectDeps.Items.Clear();
            listBoxProjects.Items.Clear();
            labelCurProj.Text = proj.Name + " Dependencies:";

            List<string> projects = new List<string>();

            foreach (Project p in Luminate.Singleton.Projects)
                if ( p.Name != proj.Name )
                    projects.Add(p.Name);

            foreach (string s in proj.Dependencies)
                listBoxProjectDeps.Items.Add(s);

            foreach (string s in projects)
                if (!proj.Dependencies.Contains(s))
                    listBoxProjects.Items.Add(s);

            textBoxOutput.Text = proj.OverridePath;
            textBoxRunnable.Text = proj.OverrideRunable;
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
        /// If esc pressed we cancel
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

            proj.Dependencies.Clear();
            foreach (object item in listBoxProjectDeps.Items)
                proj.Dependencies.Add(item.ToString() );

            proj.OverridePath = textBoxOutput.Text;
            proj.OverrideRunable = textBoxRunnable.Text;

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

        /// <summary>
        /// When we click add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddClick(object sender, EventArgs e)
        {
            if (listBoxProjects.SelectedItem == null)
                return;

            string proj = listBoxProjects.SelectedItem.ToString();
            listBoxProjects.Items.Remove(listBoxProjects.SelectedItem);
            listBoxProjectDeps.Items.Add(proj);
        }

        /// <summary>
        /// When we click on remove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveClick(object sender, EventArgs e)
        {
            if (listBoxProjectDeps.SelectedItem == null)
                return;

            string proj = listBoxProjectDeps.SelectedItem.ToString();
            listBoxProjectDeps.Items.Remove(listBoxProjectDeps.SelectedItem);
            listBoxProjects.Items.Add(proj);
        }
    }
}
