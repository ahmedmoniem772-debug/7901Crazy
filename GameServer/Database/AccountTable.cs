using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusX.Database
{
    public class AccountTable
    {
       
        public class AccountRegister
        {
            public string Username = "";
            public string Password = "";
            public string Email = "";
            public AccountRegister()
            {
                Username = Password = Email = "";
            }
        }
        public class ChangePassword
        {
            public string Username = "";
            public string Password = "";
            public string Email = "";
            public ChangePassword()
            {
                Username = Password = "";
            }
        }
    }
}
