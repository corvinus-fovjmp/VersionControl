using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;
using System.IO;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
        public Form1()
        {
            InitializeComponent();
            lblLastName.Text = Resource1.FullName;
            
            
            button1.Text = Resource1.Add;
            btnWrite.Text = Resource1.Write;
            btnDel.Text = Resource1.Del;
            lblFirstName.Hide();
            txtFirstName.Hide();
            listBox1.DataSource = users;
            listBox1.ValueMember = "ID";
            listBox1.DisplayMember = "FullName";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = txtLastName.Text


            };
            users.Add(u);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "Vesszővel tagolt szöveg (*.csv) |*.csv";
            sfd.DefaultExt = "csv";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName,false, Encoding.Default);
                foreach (var u in users)
                {
                    sw.WriteLine($"{u.ID};{u.FullName}");
                }
                sw.Close();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {


            /*string id = (from u in users where u.FullName.Equals(txtLastName.Text) select u.ID).ToString();
            string name = (from o in users where o.ID.Equals(id) select o.FullName).ToString();
            var del = users.SingleOrDefault(s => s.FullName == id);
            if (del !=null)
            {
                users.Remove(del);
            }
            users.Remove(users.SingleOrDefault(s => s.ID.ToString() == id));*/
            var val = listBox1.SelectedValue.ToString();
            var id = from u in users where u.ID.ToString() == val.ToString() select u;
            users.Remove(id.FirstOrDefault());
                
            




            
        }
    }
}
