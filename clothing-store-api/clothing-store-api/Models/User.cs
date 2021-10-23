using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Types;

#nullable enable

namespace clothing_store_api.Models
{
    
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime RegisteredAt { get;set; }

        public UserDTO HideSensitive() => new UserDTO
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone = Phone,
            LastLogin = LastLogin,
            RegisteredAt = RegisteredAt
        };
    }
}
