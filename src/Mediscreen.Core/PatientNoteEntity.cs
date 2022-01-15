using System.ComponentModel.DataAnnotations;

namespace Mediscreen
{
    public class PatientNoteEntity : PatientNoteData
    {
        [Required]
        [Display(Name = "Note Id")]
        public virtual Guid Id { get; set; }

        public PatientNoteEntity(PatientNoteEntity note)
            : this(note.Id, note)
        { }

        public PatientNoteEntity(Guid noteId, PatientNoteData note)
            : base(note)
        {
            Id = noteId;
        }

        public PatientNoteEntity() : base() { }
    }
}