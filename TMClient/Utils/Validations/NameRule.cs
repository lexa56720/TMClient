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
    class NameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is not string name)
                return new ValidationResult(false, "wrong type");

            if (DataConstraints.IsNameLegal(name))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Имя не соответсвует требованиям");
        }
    }
}
