using Mediscreen.Validation;
using System.ComponentModel.DataAnnotations;

namespace Mediscreen
{
    public class PatientData
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Given Name")]
        public virtual string GivenName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Display(Name = "Family Name")]
        public virtual string FamilyName { get; set; } = null!;

        [Required]
        [Date]
        [Display(Name = "Date of Birth")]
        public virtual Date DateOfBirth { get; set; } = null!;

        [Required]
        [Display(Name = "Sex Assigned at Birth")]
        public virtual BiologicalSex BiologicalSex { get; set; }

        [MaxLength(255)]
        [Display(Name = "Home Address")]
        public virtual string? HomeAddress { get; set; }

        [Phone]
        [MaxLength(100)]
        [Display(Name = "Phone Number")]
        public virtual string? PhoneNumber { get; set; }

        public virtual string GetFullName() => $"{this.GivenName} {this.FamilyName}";

        public int GetAge(DateTimeOffset today)
        {
            today = today.ToUniversalTime();
            var dob = new DateTimeOffset(DateOfBirth.Year, DateOfBirth.Month, DateOfBirth.Day,
                hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);

            var age = 0;
            var counter = dob.AddYears(1);

            while (counter <= today)
            {
                age++;

                counter = counter.AddYears(1);
            }

            return age;
        }
    }
}