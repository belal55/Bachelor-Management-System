using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
  @author : Md. Belal Khan
  @Email: mdbelal.aiub@gmail.com
  @Mobile: 01753011019
     
 */

namespace BachelorMangementSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           AppDomain.CurrentDomain.SetData("DataDirectory", System.Environment.CurrentDirectory.Replace("\\bin\\Debug", ""));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            
        }
    }
}
