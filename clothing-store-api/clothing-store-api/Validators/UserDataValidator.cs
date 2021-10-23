using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace clothing_store_api.Validators
{
    class UserDataValidator
    {
        public static bool ValidateEmail(string email)
        {
            return new Regex(
                @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")
                .IsMatch(email);
        }

        public static bool ValidatePhone(string phone)
        {
            return new Regex(@"^\+(?:[0-9] ?){6,14}[0-9]$").IsMatch(phone);
        }

        public static bool ValidatePassword(string password)
        {
            return new Regex(@"(?=.*[A-Z])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}")
                .IsMatch(password);
        }

        public static bool ValidateRequired(string data)
        {
            return data.Length > 0;
        }

        public static bool ValidateHexColor(string hexCode)
        {
            return new Regex(@"^#(?:[0-9a-fA-F]{3}){1,2}$")
                .IsMatch(hexCode);
        }
    }
}
