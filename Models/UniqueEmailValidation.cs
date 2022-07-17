using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ERPSystemTimologio.EF;

namespace ERPSystemTimologio.Models
{
    public class UniqueEmailValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                var db = new TimologioEntities();

                int isExists = db.Users.Where(u => u.Email.Equals(value.ToString())).Count();
                if (isExists > 0)
                {
                    return new ValidationResult("The email is already is registered");
                }
            }

            return ValidationResult.Success;
        }
    }
}