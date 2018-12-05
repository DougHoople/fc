using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FCFormVocab
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1(); 
            form.BaseDir = args[0];
            Application.Run(form);
        }
    }
}
