using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using POS;
using POS.Models;

namespace POS.Models
{
    public static class MyFunctions
    {
        private static UPOSEntities db = new UPOSEntities();

        public static bool CheckUserRole(string email, string role)
        {
            bool result = false;
            UserRole uRole = db.UserRoles.FirstOrDefault(x => x.EmailAddress == email.Trim() && x.AssignedRole == role.Trim());
            if (uRole != null)
            {
                result = true;
            }
            return result;
        }

    }
}