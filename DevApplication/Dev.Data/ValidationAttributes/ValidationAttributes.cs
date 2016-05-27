using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MappingSystem.BusinessModel.ValidationAttributes
{
    public class EmailValidationAttribute : RegularExpressionAttribute
    {
        public EmailValidationAttribute()
            : base(
                @"^([\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+\.)*[\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$"
                )
        {
        }
    }

    public class NumbericValidationAttribute : RegularExpressionAttribute
    {
        public NumbericValidationAttribute()
            : base(@"^-[0-9]+$|^[0-9]+$")
        {
        }
    }

    public class NumbericDecimalValidationAttribute : RegularExpressionAttribute
    {
        public NumbericDecimalValidationAttribute()
            : base(@"^[.][0-9]+$|[0-9]*[.]*[0-9]+$")
        {
        }
    }

    public class DateRangeValidationAttribute : ValidationAttribute
    {
        private readonly DateTime? MinDate;

        public DateRangeValidationAttribute()
        {
            MinDate = DateTime.Now;
        }

        public override bool IsValid(object value)
        {
            DateTime dtValue = Convert.ToDateTime(value);
            return dtValue > MinDate;
        }
    }

    public sealed class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _basePropertyName;

        public DateGreaterThanAttribute(string basePropertyName)
        {
            _basePropertyName = basePropertyName;
        }

        //Override IsValid   
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Get PropertyInfo Object   
            PropertyInfo basePropertyInfo = validationContext.ObjectType.GetProperty(_basePropertyName);

            //Get Value of the property   
            var startDate = (DateTime?) basePropertyInfo.GetValue(validationContext.ObjectInstance, null);


            var thisDate = (DateTime?) value;

            //Actual comparision   
            if (thisDate != null && startDate != null)
            {
                if (thisDate < startDate)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            //Default return - This means there were no validation error   
            return null;
        }
    }
}