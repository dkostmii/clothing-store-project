using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace clothing_store_api.Types
{
    public class Role
    {
        private Role(string value) { Value = value; }

        public static Role GetRole(string value)
        {
            switch (value)
            {
                case "customer":
                    return new Role("customer");
                case "manager":
                    return new Role("manager");

                default:
                    throw new Exception($"Role '{value}' does not exist.");
            }
        }

        public string Value { get; private set; }

        public static Role Customer { get { return new Role("customer"); } }
        public static Role Manager { get { return new Role("manager"); } }
    }
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime RegisteredAt { get; set; }
    }

    public class OptionalUserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }

    public class UserRole
    {
        public Role Role { get; set; }
        public UserDTO User { get; set; }
    }

    public class SignInRole
    {
        public Role Role { get; set; }
        public SignInDTO User { get; set; }
    }

    public class SignUpRole
    {
        public Role Role { get; set; }
        public SignUpDTO User { get; set; }
    }
}

