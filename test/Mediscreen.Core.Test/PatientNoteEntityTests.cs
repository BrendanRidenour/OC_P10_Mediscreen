using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen
{
    public class PatientNoteEntityTests
    {
        [Fact]
        public void PatientId_HasRequiredAttribute()
        {
            var attribute = GetPropertyAttribute<PatientNoteEntity, RequiredAttribute>("PatientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void PatientId_HasDisplayAttribute()
        {
            var attribute = GetPropertyAttribute<PatientNoteEntity, DisplayAttribute>("Id");

            Assert.Equal("Note Id", attribute.Name);
        }

        [Fact]
        public void PatientId_HasJsonPropertyName()
        {
            var attribute = GetPropertyAttribute<PatientNoteEntity, JsonPropertyNameAttribute>("Id");

            Assert.Equal("noteId", attribute.Name);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public void Constructor_NoteIdAndNoteOverload_SetsProperties(string text)
        {
            var noteId = Guid.NewGuid();
            var data = new PatientNoteData()
            {
                PatientId = Guid.NewGuid(),
                Text = text,
            };

            var entity = new PatientNoteEntity(noteId, data);

            Assert.Equal(noteId, entity.Id);
            Assert.Equal(data.PatientId, entity.PatientId);
            Assert.Equal(text, entity.Text);
        }
    }
}