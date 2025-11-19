using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Validation
{
    public class NameValidation : ValidationAttribute
    {
        private readonly Regex _regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");

        public NameValidation()
        {
            ErrorMessage = "leopardName format is invalid";
        }

        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            return _regex.IsMatch(value.ToString()!);
        }
    }
}
