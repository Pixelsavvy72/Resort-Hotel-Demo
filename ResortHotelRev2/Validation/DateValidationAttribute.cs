using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;


//TODO: Get this working. Partial view doesn't give errors but this still fails causing model / view to fail.
namespace ResortHotelRev2.Validation
{
    
    public class CheckInValidationAttribute : ValidationAttribute  
    {

        public CheckInValidationAttribute()
            :base("Check-in date must not have already passed.")
        {
            _minimumDate = DateTime.UtcNow;
        }

        protected override ValidationResult IsValid(object CheckIn, ValidationContext validationContext)
        {

           
            DateTime checkInConverted = DateTime.Parse(CheckIn.ToString());

            if (CheckIn != null ) 
            {
                if (checkInConverted < DateTime.UtcNow)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            

            return ValidationResult.Success;
        }
        private readonly DateTime _minimumDate;
    } 
}