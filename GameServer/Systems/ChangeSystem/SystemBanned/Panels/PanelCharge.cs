using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // تأكد من إضافة هذا السطر


namespace VirusX.Systems.ChangeSystem.SystemBanned.Panels
{
    public partial class PanelCharge : Form
    {
        public PanelCharge()
        {
            InitializeComponent();
        }

        public string ConnectionString = "Server=localhost;username=root;password=SwoC33MaHBaqGym8Nk7DuEa587JDE3Cmuwpt0UnyY6K07MpFJC;database=newcq;";

        private void button1_Click(object sender, EventArgs e)
        {
            string username = User.Text;
            string itemId = USD.Text;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string query = "INSERT INTO payments (username, itemid) VALUES (@username, @itemid)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@itemid", itemId);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("تم إضافةاالشحن بنجاح!");
                        User.Clear();
                        USD.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("حدث خطأ: " + ex.Message);
                    }
                }
            }
        }

      


    }
}
