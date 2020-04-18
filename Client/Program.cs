using P1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P1.GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //MediaTagsClient c = new MediaTagsClient();
            //Media m = c.GetMediaByPath("F:\\Downloads\\BT96BXC.jpg");
            //c.AddTagToMedia(m, new Tags { Name = "CLAUS" });
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
