using System;
using System.ComponentModel.DataAnnotations;
using DAL.Common;

namespace UI.MVVM.Validation
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UniqueAssetAttibute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value.ToString();

            using (var uow = new UnitOfWork())
            {
                if (uow.AssetRepository.IsAssetExists(name)) return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
