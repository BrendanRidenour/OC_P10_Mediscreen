using System.ComponentModel.DataAnnotations;

namespace Mediscreen
{
    public class PatientData
    {
        [Required]
        [Display(Name = "Given Name")]
        public virtual string GivenName { get; set; } = null!;

        [Required]
        [Display(Name = "Family Name")]
        public virtual string FamilyName { get; set; } = null!;

        [Required]
        [Display(Name = "Date of Birth")]
        public virtual Birthdate DateOfBirth { get; set; } = null!;

        [Required]
        [Display(Name = "Sex Assigned at Birth")]
        public virtual BiologicalSex BiologicalSex { get; set; }

        [Display(Name = "Home Address")]
        public virtual string? HomeAddress { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public virtual string? PhoneNumber { get; set; }

        public virtual string GetFullName() => $"{this.GivenName} {this.FamilyName}";
    }
}