using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Infrastructure.Validation
{
    /// <summary>
    /// <c>FileExtensionAttribute</c> Validates files extensions.
    /// </summary>
    public class FileExtensionAttribute : ValidationAttribute
    {
        string[] extensions = { "jpg", "png" };

        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                bool result = extensions.Any(ex => extension.EndsWith(ex));

                if(result)
                {
                    return ValidationResult.Success;
                } else
                {
                    return new ValidationResult("Allowed Exxtensions : 'jpg', 'png' .");
                }
            }

            return ValidationResult.Success;
 
        }
    }
}
