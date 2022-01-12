using System.ComponentModel.DataAnnotations;

namespace Mediscreen.Validation
{
    public class DateAttribute : ValidationAttribute
    {
        public DateAttribute()
        {
            ErrorMessage = "Please enter a valid date.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not Date birthdate)
                return null;

            try
            {
                _ = new DateTime(year: birthdate.Year, month: birthdate.Month, day: birthdate.Day);

                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}