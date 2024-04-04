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
    internal class PasswordRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is not string password)
                return new ValidationResult(false, "wrong type");

            if (DataConstraints.IsPasswordLegal(password))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Пароль не соответсвует требованиям");
        }
    }
}
