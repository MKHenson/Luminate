using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Awesomium.Core;
using Awesomium.Windows.Forms;

namespace Luminate
{
    public partial class Preview : DockContent
    {
        private WebControl mControl;

        public Preview()
        {
            InitializeComponent();

            mControl = new WebControl();
            Controls.Add(mControl);
            mControl.Dock = DockStyle.Fill;

            mControl.JSConsoleMessageAdded += new JSConsoleMessageAddedEventHandler(OnJSError);
            mControl.BeginNavigation += new BeginNavigationEventHandler(OnNavigating);
            mControl.BeginLoading += new BeginLoadingEventHandler(OnBeginLoading);
            mControl.DomReady += new EventHandler(OnDomReady);
           
            this.Resize += new EventHandler(OnWindowResized);

            TabPageContextMenuStrip = contextMenuStrip;
        }

        /// <summary>
        /// When the window resizes move the logo around
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnWindowResized(object sender, EventArgs e)
        {
            luminateLogo.Location = new Point(Width / 2 - luminateLogo.Width / 2, Height / 2 - luminateLogo.Height / 2);
            luminateLogo.BackColor = Color.FromArgb(255, 255, 255);
        }

        void OnDomReady(object sender, EventArgs e) { TabText = "Preview - Dom Ready"; }
        void OnBeginLoading(object sender, BeginLoadingEventArgs e) 
        {
            if (mControl.TargetURL == "" || mControl.TargetURL == null )
                TabText = "Preview"; 
            else
                TabText = "Preview - Loading..."; 
        
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
        /// Removes all the files from the panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseAllClick(object sender, EventArgs e)
        {
            Luminate.Singleton.RemoveAll("");
            this.Hide();
        }

        void OnNavigating(object sender, BeginNavigationEventArgs e) { TabText = "Preview - Navigating to URL..."; }

        /// <summary>
        /// When we receive a javascript error we print it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnJSError(object sender, JSConsoleMessageEventArgs e)
        {
            Logger.Singleton.Log("Javascript Error: " + e.Message + " on line " + e.LineNumber + ". " + e.Source, Logger.MessageType.ERROR, new string[]{ e.Source, e.LineNumber.ToString() } );
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

        /// <summary>
        /// Try to load a given URL for a project
        /// </summary>
        /// <param name="url"></param>
        public void LoadURL(string url)
        {
            if (luminateLogo.Parent != null)
                luminateLogo.Parent.Controls.Remove(luminateLogo);

            if (Visible && mControl.IsLive)
                mControl.Source = new Uri(url);
            else
                TabText = "Preview - Not Live.";
        } 
    }
}
