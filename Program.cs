using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Luminate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string[] data )
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Luminate(data));
        }
    }
}
