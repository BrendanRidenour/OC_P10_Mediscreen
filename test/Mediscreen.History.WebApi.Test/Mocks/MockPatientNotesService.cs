using Mediscreen.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediscreen.Mocks
{
    public class MockPatientNotesService : IPatientNotesService
    {
        public PatientNoteData? Create_ParamNote;
        public PatientNoteEntity Create_Return = new()
        {
            Id = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            Text = "note text",
        };
        public Task<PatientNoteEntity> Create(PatientNoteData note)
        {
            Create_ParamNote = note;

            return Task.FromResult(Create_Return);
        }

        public Guid? ReadByPatientId_ParamPatientId;
        public List<PatientNoteEntity> ReadByPatientId_Return = new()
        {
            new PatientNoteEntity()
            {
                Id= Guid.NewGuid(),
                PatientId= Guid.NewGuid(),
                Text = "note text",
            },
            new PatientNoteEntity()
            {
                Id= Guid.NewGuid(),
                PatientId= Guid.NewGuid(),
                Text = "note text",
            },
        };
        public Task<IEnumerable<PatientNoteEntity>> Read(Guid patientId)
        {
            ReadByPatientId_ParamPatientId = patientId;

            return Task.FromResult<IEnumerable<PatientNoteEntity>>(ReadByPatientId_Return);
        }

        public Guid? ReadByNoteId_ParamPatientId;
        public Guid? ReadByNoteId_ParamNoteId;
        public PatientNoteEntity? ReadByNoteId_Return = new()
        {
            Id = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            Text = "note text",
        };
        public Task<PatientNoteEntity?> Read(Guid patientId, Guid noteId)
        {
            ReadByNoteId_ParamPatientId = patientId;
            ReadByNoteId_ParamNoteId = noteId;

            return Task.FromResult(ReadByNoteId_Return);
        }

        public PatientNoteEntity? Update_ParamNote;
        public Task Update(PatientNoteEntity note)
        {
            Update_ParamNote = note;

            return Task.CompletedTask;
        }
    }
}