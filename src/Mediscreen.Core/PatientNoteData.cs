using System.ComponentModel.DataAnnotations;

namespace Mediscreen
{
    public class PatientNoteData
    {
        [Required]
        [Display(Name = "Patient Id")]
        public virtual Guid PatientId { get; set; }

        [Required]
        public virtual string Text { get; set; } = null!;

        public PatientNoteData(PatientNoteData note)
            : this()
        {
            PatientId = note.PatientId;
            Text = note.Text;
        }

        public PatientNoteData() { }
    }
}