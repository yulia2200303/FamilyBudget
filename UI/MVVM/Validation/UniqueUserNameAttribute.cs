using System;
using System.ComponentModel.DataAnnotations;
using DAL.Common;

namespace UI.MVVM.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UniqueUserNameAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var login = value.ToString();

            using (var uow = new UnitOfWork())
            {
                if(uow.UserRepository.IsUserExist(login)) return new ValidationResult(ErrorMessage);
            }

           return ValidationResult.Success;
        }
    }
}
