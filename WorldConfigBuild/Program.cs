using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BFramework.World;

namespace WorldConfigBuild
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form = new MainForm();
            Application.Run(form);
        }
    }
}
