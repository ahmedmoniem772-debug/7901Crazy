using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusX
{
    public class PayPalHandler
    {

        public const string ConnectionString = "Server=localhost;username=root;password=SwoC33MaHBaqGym8Nk7DuEa587JDE3Cmuwpt0UnyY6K07MpFJC;database=newcq;";
        public static Dictionary<float, float> getItems(string username)
        {
            Dictionary<float, float> items = new Dictionary<float, float>();
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    using (var cmd = new MySqlCommand("select itemid from payments where username=@joo and claimed=0"
                        , conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@joo", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                float r = float.Parse(reader.GetString("itemid"));
                                if (items.ContainsKey(r))
                                    items[r]++;
                                else 
                                    items.Add(r, 1);
                            }
                        }
                    }

                    using (var cmd = new MySqlCommand("update payments set claimed=1 where username=@joo"
                        , conn))
                    {
                        cmd.Parameters.AddWithValue("@joo", username);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return items;
        }
        public static void logDonation(string user, string name, string log)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    using (var cmd = new MySqlCommand("insert into log_payments (username,name,log) values (@user,@name,@log)"
                        , conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@log", log);
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }



        
    }
}
