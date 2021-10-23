using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace clothing_store_api.Types
{
    public class SignInDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class SignUpDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }

    public class AuthResult
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public UserDTO? User { get; set; }
        public string? Token { get; set; }
    }
}
