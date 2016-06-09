using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventClasses
{
    public class AppUser
    {
        private int userId;
        private string userName;
        private string password;

        public int UserId
        {
            get { return userId; }
        }
        public string UserName
        {
            get { return userName; }
        }
        public string Password
        {
            get { return password; }
        }

        public AppUser(int id, string name, string pwd)
        {
            userId = id;
            userName = name;
            password = pwd;
        }

        public static AppUser Login(string name, string pwd)
        {
            if (name == "valid" && pwd == "AppUser1")
                return new AppUser(1, name, pwd);
            else if (name == "valid" && pwd == "AppUser2")
                return new AppUser(2, name, pwd);
            else
                return new AppUser(-1, name, pwd);
        }


    }
}
