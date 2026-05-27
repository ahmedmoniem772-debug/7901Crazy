using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ConquerOnline
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        private static bool Exists(string username)
        {
            try
            {
            
            }
            catch
            {
            }
            return false;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            
            if (textBox10.Text == "")
            {
                textBox10.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (textBox10.Text != "")
            {
                textBox10.BackColor = System.Drawing.Color.White;
            }
            if (textBox9.Text == "")
            {
                textBox9.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (textBox9.Text != "")
            {
                textBox9.BackColor = System.Drawing.Color.White;
            }
            if (!Exists(textBox10.Text))
            {
               
                label16.Visible = true;
            }
            else
            {
                label16.ForeColor = Color.Red;
                label16.Text = "Account Name Is Exit";
                label16.Visible = true;
            }
         
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
   
    }
}
