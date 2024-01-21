using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_MAG
{
    public static class UserSession
    {
        public static string Email { get; private set; }

        public static void SetUserEmail(string email)
        {
            Email = email;
        }
    }
}