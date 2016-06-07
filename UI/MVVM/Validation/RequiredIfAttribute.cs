using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace UI.MVVM.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private String PropertyName { get; set; }
        private Object DesiredValue { get; set; }

        public RequiredIfAttribute(String propertyName, Object desiredvalue, String errormessage)
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            this.ErrorMessage = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue.ToString() == DesiredValue.ToString() && value == null)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MinLengthIfAttribute : ValidationAttribute
    {
        private String PropertyName { get; set; }
        private Object DesiredValue { get; set; }
        private uint MinLength { get; set; }

        public MinLengthIfAttribute(String propertyName, object desiredValue, uint minLength, Type errorMessageResourceType = null, string errorMessageResourceName = null, String errormessage = null)
        {
            PropertyName = propertyName;
            MinLength = minLength;
            DesiredValue = desiredValue;
            ErrorMessageResourceType = errorMessageResourceType;
            ErrorMessageResourceName = errorMessageResourceName;
            ErrorMessage = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue.ToString() == DesiredValue.ToString() && (value == null || value.ToString().Length < MinLength))
            {
                if (ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
                {
                    var field = ErrorMessageResourceType.GetProperty(ErrorMessageResourceName);
                    if (field != null)
                    {
                        var val = field.GetValue(ErrorMessageResourceType, null) as string;
                        return new ValidationResult(val);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
