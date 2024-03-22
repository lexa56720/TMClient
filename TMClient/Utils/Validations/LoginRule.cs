using ApiTypes.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TMClient.Utils.Validations
{
    class LoginRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is not string name)
                return new ValidationResult(false, "wrong type");

            if (DataConstraints.IsLoginLegal(name))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Логин не соответсвует требованиям");
        }
    }
}
