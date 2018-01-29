using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;

namespace Youtube_DL_GFX
{
    static class Program
    {
        public static MainForm main;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            main = new MainForm();
            if (!Directory.Exists(MainForm.BaseFolder))
            {
                Directory.CreateDirectory(MainForm.BaseFolder);
            }

            Application.Run(main);
        }
    }
}