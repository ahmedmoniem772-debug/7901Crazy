using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MYSQLCOMMAND = MySql.Data.MySqlClient.MySqlCommand;
using MYSQLREADER = MySql.Data.MySqlClient.MySqlDataReader;
using MYSQLCONNECTION = MySql.Data.MySqlClient.MySqlConnection;

namespace VirusX.DBFunctionality
{
    public unsafe static class DataHolder
    {
        public static String ConnectionString;
        public static Boolean CreateConnection(String database, String password)
        {
            ConnectionString = "Server=localhost;Port=3306;Database=" + database + ";Uid=root;Password=" + password + ";Persist Security Info=True;Pooling=true; Min Pool Size = 32;  Max Pool Size = 300;"; return true;
        }
        public static MYSQLCONNECTION MySqlConnection
        {
            get
            {
                MYSQLCONNECTION conn = new MYSQLCONNECTION();
                conn.ConnectionString = ConnectionString;
                return conn;
            }
        }
    }
}
