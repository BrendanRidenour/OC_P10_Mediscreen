using Azure;
using Azure.Data.Tables;

namespace Mediscreen.Data
{
    public class PatientNoteTableEntity : PatientNoteEntity, ITableEntity
    {
        public string PartitionKey { get; set; } = null!;
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public PatientNoteTableEntity(Guid noteId, PatientNoteData note)
            : this(new PatientNoteEntity(noteId, note))
        { }

        public PatientNoteTableEntity(PatientNoteEntity note)
            : base(note)
        {
            PartitionKey = CreatePartitionKey(patientId: note.PatientId);
            RowKey = CreateRowKey(noteId: note.Id);
        }

        public PatientNoteTableEntity() { }

        public static string CreatePartitionKey(Guid patientId) => $"Patient-{patientId}";
        public static string CreateRowKey(Guid noteId) => $"Note-{noteId}";
    }
}