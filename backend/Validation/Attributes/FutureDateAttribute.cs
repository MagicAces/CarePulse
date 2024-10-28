using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Validation.Attributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date && date <= DateTime.Now)
            {
                return new ValidationResult(ErrorMessage ?? "The date must be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}