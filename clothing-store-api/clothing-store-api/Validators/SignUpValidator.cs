using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Types;

namespace clothing_store_api.Validators
{
    public class SignUpValidator
    {
        private SignUpDTO data;

        public SignUpValidator(SignUpDTO data)
        {
            this.data = data;
        }

        public bool Validate()
        {
            return UserDataValidator.ValidateRequired(data.FirstName) &&
                UserDataValidator.ValidateEmail(data.Email) &&
                UserDataValidator.ValidatePhone(data.Phone) &&
                UserDataValidator.ValidatePassword(data.Password);
        }
    }
}
