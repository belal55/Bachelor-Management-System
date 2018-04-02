using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BachelorMangementSystem
{
    public partial class Form1 : Form
    {
        //DataClasses1DataContext db = new DataClasses1DataContext();
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if (usertextbox.Text.Equals("manager") && passtextbox.Text.Equals("manager"))
            {
                AdminDashboard ad = new AdminDashboard();
                this.Hide();
                ad.Show();
            }
            else
            {
                MessageBox.Show("Username or Password is incorrect!!!");
            }

        }

        private void passtextbox_Enter(object sender, EventArgs e)
        {
            passtextbox.Text = "";
        }

        private void passtextbox_Leave(object sender, EventArgs e)
        {
            passtextbox.Text = "Enter Password";
        }
    }
}
