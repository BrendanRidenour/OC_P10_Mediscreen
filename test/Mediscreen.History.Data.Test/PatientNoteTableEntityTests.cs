using System;
using Xunit;

namespace Mediscreen.Data
{
    public class PatientNoteTableEntityTests
    {
        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public void Constructor_NoteIdAndNoteOverload_SetsProperties(string text)
        {
            var noteId = Guid.NewGuid();
            var note = new PatientNoteData()
            {
                PatientId = Guid.NewGuid(),
                Text = text,
            };

            var entity = new PatientNoteTableEntity(noteId, note);

            Assert.Equal($"Patient-{note.PatientId}", entity.PartitionKey);
            Assert.Equal($"Note-{noteId}", entity.RowKey);
            Assert.Equal(noteId, entity.Id);
            Assert.Equal(note.PatientId, entity.PatientId);
            Assert.Equal(text, entity.Text);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public void Constructor_NoteOverload_SetsProperties(string text)
        {
            var note = new PatientNoteEntity()
            {
                Id = Guid.NewGuid(),
                PatientId = Guid.NewGuid(),
                Text = text,
            };

            var entity = new PatientNoteTableEntity(note);

            Assert.Equal($"Patient-{note.PatientId}", entity.PartitionKey);
            Assert.Equal($"Note-{note.Id}", entity.RowKey);
            Assert.Equal(note.Id, entity.Id);
            Assert.Equal(note.PatientId, entity.PatientId);
            Assert.Equal(text, entity.Text);
        }

        [Fact]
        public void CreatePartitionKey_WhenCalled_ReturnsString()
        {
            var patientId = Guid.NewGuid();

            var partitionKey = PatientNoteTableEntity.CreatePartitionKey(patientId);

            Assert.Equal($"Patient-{patientId}", partitionKey);
        }

        [Fact]
        public void CreateRowKey_WhenCalled_ReturnsString()
        {
            var noteId = Guid.NewGuid();

            var rowKey = PatientNoteTableEntity.CreateRowKey(noteId);

            Assert.Equal($"Note-{noteId}", rowKey);
        }
    }
}