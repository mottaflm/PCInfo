using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCInfo.Forms;

namespace PCInfo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void ErrorDialog(string info)
        {
            MessageBox.Show(info, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void WarningDialog(string info)
        {
            MessageBox.Show(info, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void InfoDialog(string info)
        {
            MessageBox.Show(info, "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
