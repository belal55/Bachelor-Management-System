using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BachelorMangementSystem
{
    public partial class AdminDashboard : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        float mealrate = 0;
       
        public AdminDashboard()
        {
            InitializeComponent();
            
            
        }

        public void updateMess()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Balances);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Bazars);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Meals);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Members);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Messes);
            var data = from m in db.Messes
                        where m.Id > 0
                        select m;
            foreach (Mess mm in data)
            {
                mm.TotalBalance = 0;
                var data1 = from b in db.Balances
                           where b.Id > 0
                           select b;
                foreach (var i in data1)
                {
                    mm.TotalBalance += i.amount;

                }

                mm.TotalMeal = 0;
                var data2 = from tm in db.Meals
                            where tm.Id > 0
                            select tm;
                            foreach(var t in data2)
                            {
                                mm.TotalMeal += t.day;
                                mm.TotalMeal += t.night;

                            }
                            mm.TotalMember = 0;
                            var data3 = from m in db.Members
                                        where m.Id > 0
                                        select m;
                            foreach (var me in data3)
                            {
                                mm.TotalMember += 1;
                            }
                            mm.TotalBazar = 0;
                            var data4 = from B in db.Bazars
                                        where B.Id > 0
                                        select B;
                             
                            foreach (var Bb in data4)
                            {
                                mm.TotalBalance -= Bb.amount;
                                mm.TotalBazar += Bb.amount;
                            }

                            mm.TodaysMeal = 0;
                            var data5 = from tdm in db.Meals
                                        where tdm.date == DateTime.Today
                                        select tdm;
                            foreach (var T in data5)
                            {
                                mm.TodaysMeal += T.day;
                                mm.TodaysMeal += T.night;
                            }
                            mm.TodaysShopping = 0;
                            var data6 = from bz in db.Bazars
                                        where bz.date == DateTime.Today
                                        select bz;
                            foreach (var z in data6)
                            {
                                mm.TodaysShopping += z.amount;
                            }


                            mm.MealRate = 0;
                            if (mm.TotalBazar != 0 && mm.TotalMeal != 0)
                            {
                                mm.MealRate = mm.TotalBazar / mm.TotalMeal;
                                mm.MealRate = (float)Math.Round(float.Parse(mm.MealRate.ToString()) * 100f) / 100f;
                            }


                            db.SubmitChanges();
                            float F = (float)Math.Round(float.Parse(mm.TotalBalance.ToString()) * 100f) / 100f;
                            label2.Text = F.ToString();

                            float F1 =(float)Math.Round(float.Parse(mm.MealRate.ToString()) * 100f) / 100f;
                            label5.Text = F1.ToString();
                            mealrate = F1;

                            float F2 = (float)Math.Round(float.Parse(mm.TotalBazar.ToString()) * 100f) / 100f;
                            label1.Text = F2.ToString();

                            label6.Text = mm.TotalMeal.ToString();
                            label7.Text = mm.TodaysMeal.ToString();

                            float F3 = (float)Math.Round(float.Parse(mm.TodaysShopping.ToString()) * 100f) / 100f;
                            label8.Text = F3.ToString();
            }

            comboBox1.DataSource = db.Members;
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndex = -1;
            comboBox1.Text = "Select A Member";
            
        }

        public void updatedatagrid()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Balances);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Bazars);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Meals);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Members);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Messes);

            var data = from m in db.Members
                       where m.Id > 0
                       select m;
            foreach (var mb in data)
            {
                mb.balance = 0;
                var data1 = from b in db.Balances
                            where b.uid == mb.Id
                            select b;
                foreach (var bl in data1)
                {
                    mb.balance += bl.amount;
                }
                mb.totalmeals = 0;
                var data2 = from ml in db.Meals
                            where ml.uid == mb.Id
                            select ml;
                foreach (var mk in data2)
                {
                    mb.totalmeals += mk.day;
                    mb.totalmeals += mk.night;
                }
                mb.bills = 0;
                mb.due = 0;
                mb.bills = mb.totalmeals * mealrate;
                mb.due = mb.balance - mb.bills;
                mb.bills = (float)Math.Round(float.Parse(mb.bills.ToString()) * 100f) / 100f;
                mb.due = (float)Math.Round(float.Parse(mb.due.ToString()) * 100f) / 100f;
                db.SubmitChanges();
            }


            dataGridView1.DataSource = data;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f = new Form1();
            f.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            memberpanel.Visible = false;
            MealEntryPanel.Visible = false;
            AboutPanel.Visible = false;
            bunifuFlatButton1.selected = true;
            dashpanel.Visible = true;
           var data = from m in db.Members
                       where m.balance==0
                       select m;
            if (data != null)
            {
                updateMess();
            }
            else if (data == null)
            {

            }
           
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
           
            memberpanel.Visible = false;
            MealEntryPanel.Visible = false;
            AboutPanel.Visible = false;
            dashpanel.Visible = true;
         
           
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            dashpanel.Visible = false;
            MealEntryPanel.Visible = false;
            AboutPanel.Visible = false;
            memberpanel.Visible = true;

            var data = from mss in db.Messes
                       where mss.Id > 0
                       select new { mss.TotalBalance, mss.TotalBazar, mss.TotalMeal, mss.MealRate};
            dataGridView2.DataSource = data;
            updatedatagrid();

        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Balances);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Bazars);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Meals);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Members);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Messes);
            dashpanel.Visible = false;
            memberpanel.Visible = false;
            AboutPanel.Visible = false;
            MealEntryPanel.Visible = true;
            comboBox2.DataSource = db.Members;
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
            comboBox2.SelectedIndex = -1;
            comboBox2.Text = "Select A Member";

            comboBox3.DataSource = db.Members;
            comboBox3.DisplayMember = "name";
            comboBox3.ValueMember = "id";
            comboBox3.SelectedIndex = -1;
            comboBox3.Text = "Select A Member";
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
           
            
        }

        private void bunifuFlatButton5_Click_1(object sender, EventArgs e)
        {
            if (bunifuMaterialTextbox2.Text.Equals(""))
            {
                MessageBox.Show("Name can't be empty");
            }
            else
            {
                try
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    Member m = new Member();
                    m.name = bunifuMaterialTextbox2.Text;
                    db.Members.InsertOnSubmit(m);
                    db.SubmitChanges();
                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Members);
                    MessageBox.Show("Member Added");
                    updatedatagrid();
                    updateMess();
                }
                catch (Exception em)
                {
                    MessageBox.Show("Name can't contain more than 50 character!!");
                }
            }
            
        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                Meal m = new Meal();
                m.uid = Int32.Parse(comboBox2.SelectedValue.ToString());
                m.day = Int32.Parse(textBox1.Text.ToString());
                m.night = Int32.Parse(textBox2.Text.ToString());
                m.date = DateTime.Today;
                db.Meals.InsertOnSubmit(m);
                db.SubmitChanges();
                MessageBox.Show("Meal Added..");
                updatedatagrid();
                updateMess();
                textBox1.Text = "Day";
                textBox2.Text = "Night";
            }
            catch (Exception em)
            {
                MessageBox.Show("Invalid Input!!\nSelect a member first\nthen input numeric value for day and night's meal");
                textBox1.Text = "Day";
                textBox2.Text = "Night";
            }
        }

        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                Balance b = new Balance();
                b.uid = Int32.Parse(comboBox3.SelectedValue.ToString());
                b.amount = Int32.Parse(textBox4.Text.ToString());
                b.date = DateTime.Today;
                db.Balances.InsertOnSubmit(b);
                db.SubmitChanges();
                MessageBox.Show("Payment Added.");
                updatedatagrid();
                updateMess();
                textBox4.Text = "Amount";
            }
            catch (Exception em)
            {
                MessageBox.Show("Invalid input!!\nSelect a member first\nthen input numeric value in amount box.");
                textBox4.Text = "Amount";
            }
        }

        private void bunifuCustomLabel5_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            try
            {
                Bazar b = new Bazar();
                b.uid = Int32.Parse(comboBox1.SelectedValue.ToString());
                b.amount = float.Parse(bunifuMaterialTextbox1.Text);
                b.date = DateTime.Today;

                db.Bazars.InsertOnSubmit(b);
                db.SubmitChanges();
                updateMess();
                MessageBox.Show("Shopping Added..");
            }
            catch (Exception em)
            {
                MessageBox.Show("Invalid input!!\nInput numeric value in amount box first\nthen select a member.");
            }

        }

        private void bunifuFlatButton9_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want delete all data?","Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                db.Meals.DeleteAllOnSubmit(db.Meals);
                db.Bazars.DeleteAllOnSubmit(db.Bazars);
                db.Balances.DeleteAllOnSubmit(db.Balances);
                db.Members.DeleteAllOnSubmit(db.Members);
                db.SubmitChanges();
                updateMess();
                updatedatagrid();
                
               
            }

            
            
             
        }

        private void bunifuFlatButton8_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Balances);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Bazars);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Meals);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Members);
            db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.Messes);
            try
            {
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Result.pdf", FileMode.Create));
                doc.Open();



                PdfPTable table = new PdfPTable(1);


                PdfPCell cell = new PdfPCell(new Phrase("Bachelor Mangement System\n ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE)));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(0, 150, 0);
                cell.Colspan = 1;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                PdfPCell cell2 = new PdfPCell(new Phrase("Developed by Md Belal Khan\nContact: mdbelal.aiub@gmail.com\n ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 16f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE)));
                cell2.BackgroundColor = new iTextSharp.text.BaseColor(0, 150, 0);
                cell2.Colspan = 1;
                cell2.HorizontalAlignment = 1;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Total Calculation Of Current Month\n ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell3.Colspan = 1;
                cell3.HorizontalAlignment = 1;
                table.AddCell(cell3);


                PdfPTable table3 = new PdfPTable(dataGridView2.Columns.Count);
                for (int m = 0; m < dataGridView2.Columns.Count; m++)
                {
                    table3.AddCell(new Phrase(dataGridView2.Columns[m].HeaderText));
                }

                table3.HeaderRows = 1;

                for (int p = 0; p < dataGridView2.Rows.Count; p++)
                {
                    for (int q = 0; q < dataGridView2.Columns.Count; q++)
                    {
                        if (dataGridView1[q, p].Value != null)
                        {
                            table3.AddCell(new Phrase(dataGridView2[q, p].Value.ToString()));
                        }
                    }
                }

                table.AddCell(table3);


                PdfPCell cell4 = new PdfPCell(new Phrase("Member Wise Calculation Of Current Month\n ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell4.Colspan = 1;
                cell4.HorizontalAlignment = 1;
                table.AddCell(cell4);


                PdfPTable table2 = new PdfPTable(dataGridView1.Columns.Count);
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    table2.AddCell(new Phrase(dataGridView1.Columns[i].HeaderText));
                }

                table2.HeaderRows = 1;

                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    for (int l = 0; l < dataGridView1.Columns.Count; l++)
                    {
                        if (dataGridView1[l, k].Value != null)
                        {
                            table2.AddCell(new Phrase(dataGridView1[l, k].Value.ToString()));
                        }
                    }
                }

                table.AddCell(table2);




                doc.Add(table);


                doc.Close();
                MessageBox.Show("Pdf created successfully!!");
                System.Diagnostics.Process.Start("Result.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show("First view members by clicking Member tab\nthen click on Get Pdf.");
            }
        }

        private void bunifuFlatButton8_Click_1(object sender, EventArgs e)
        {
            memberpanel.Visible = false;
            MealEntryPanel.Visible = false;
            dashpanel.Visible = false;
            AboutPanel.Visible = true;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox4.Text = "";
        }
    }
}
