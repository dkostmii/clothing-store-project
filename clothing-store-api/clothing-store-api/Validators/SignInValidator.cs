using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Types;

namespace clothing_store_api.Validators
{
    public class SignInValidator
    {
        private SignInDTO data;
        public SignInValidator(SignInDTO data)
        {
            this.data = data;
        }
        public bool Validate()
        {
            return (UserDataValidator.ValidateEmail(data.Login) ||
                    UserDataValidator.ValidatePhone(data.Login)) &&
                    UserDataValidator.ValidatePassword(data.Password);
        }
    }
}
