using clothing_store_api.Types;
using clothing_store_api.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clothing_store_api.Middlewares
{
    public class Validate
    {
        public bool IsColor(ColorDTO data)
        {
            return UserDataValidator.ValidateRequired(data.Name) &&
                UserDataValidator.ValidateRequired(data.HexCode)
                && UserDataValidator.ValidateHexColor(data.HexCode);
        }

        public bool IsMaterialRaw(MaterialRawDTO data)
        {
            return data.RawId != null && data.Amount != null;
        }

        public bool IsMaterial(MaterialDTO data)
        {
            bool flag = true;

            if (data.Color != null)
            {
                flag = flag && IsColor(data.Color);
            }

            if (data.MaterialRaws != null)
            {
                foreach (var materialRaw in data.MaterialRaws)
                {
                    flag = flag && IsMaterialRaw(materialRaw);
                }
            }

            return UserDataValidator.ValidateRequired(data.Title) &&
                UserDataValidator.ValidateRequired(data.Description) && flag;
        }
    }
}
