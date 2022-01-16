using Mediscreen.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen.Controllers
{
    public class PatientNoteControllerTests
    {
        [Fact]
        public void InheritsController()
        {
            Assert.True(typeof(ControllerBase).IsAssignableFrom(typeof(PatientNotesController)));
        }

        [Fact]
        public void HasApiControllerAttribute()
        {
            var attribute = GetClassAttribute<PatientNotesController, ApiControllerAttribute>();

            Assert.NotNull(attribute);
        }

        [Fact]
        public void HasRouteAttribute()
        {
            var attribute = GetClassAttribute<PatientNotesController, RouteAttribute>();

            Assert.Equal("[controller]", attribute.Template);
        }

        [Fact]
        public void Create_PatientOverload_HasHttpPostAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpPostAttribute>("Create");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Create_PatientOverload_PatientHasFromBodyAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromBodyAttribute>("Create",
                "note");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Create_PatientOverload_WhenCalled_CallsCreateOnPatientService()
        {
            var service = NoteService();
            var controller = Controller(service);
            var note = NoteData();

            await controller.Create(note);

            Assert.Equal(service.Create_ParamNote, note);
        }

        [Fact]
        public async Task Create_PatientOverload_WhenCalled_ReturnsCreatedAtActionResult()
        {
            var service = NoteService();
            var controller = Controller(service);
            var note = NoteData();

            var createResult = await controller.Create(note);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(createResult.Result);
            Assert.Equal("Read", createdAtAction.ActionName);
            Assert.Equal(service.Create_Return.Id, ((PatientNoteEntity)createdAtAction.Value!).Id);
            Assert.Equal(service.Create_Return.PatientId, createdAtAction.RouteValues!["patientId"]!);
            Assert.Equal(service.Create_Return.Id, createdAtAction.RouteValues!["noteId"]!);
        }

        [Fact]
        public void Read_PatientIdOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpGetAttribute>("Read");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Read_PatientIdOverload_PatientIdHasHttpGetAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>("Read",
                "patientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Read_PatientIdOverload_PatientServiceReturnsNull_ReturnsNotFound()
        {
            var service = NoteService();
            service.ReadByPatientId_Return = null!;
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            var actionResult = await controller.Read(patientId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task Read_PatientIdOverload_WhenCalled_ReturnsReadOnPatientService()
        {
            var service = NoteService();
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            var actionResult = await controller.Read(patientId);

            Assert.Equal(patientId, service.ReadByPatientId_ParamPatientId);
            Assert.Equal(service.ReadByPatientId_Return, actionResult.Value);
        }

        [Fact]
        public void Read_PatientIdAndNoteIdOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpGetAttribute>("Read",
                methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Read_PatientIdAndNoteIdOverload_HasRouteAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, RouteAttribute>("Read",
                methodIndex: 1);

            Assert.Equal("{patientId}/{noteId}", attribute.Template);
        }

        [Fact]
        public void Read_PatientIdAndNoteIdOverload_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>("Read",
                "patientId", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Read_PatientIdAndNoteIdOverload_NoteIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>("Read",
                "noteId", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Read_PatientIdAndNoteIdOverload_WhenCalled_CallsReadOnPatientService()
        {
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var service = NoteService();
            service.ReadByNoteId_Return = null;
            var controller = Controller(service);

            await controller.Read(patientId, noteId);

            Assert.Equal(patientId, service.ReadByNoteId_ParamPatientId);
            Assert.Equal(noteId, service.ReadByNoteId_ParamNoteId);
        }

        [Fact]
        public async Task Read_PatientIdAndNoteIdOverload_PatientServiceReturnsNull_ReturnsNotFound()
        {
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var service = NoteService();
            service.ReadByNoteId_Return = null;
            var controller = Controller(service);

            var readResult = await controller.Read(patientId, noteId);

            Assert.IsType<NotFoundResult>(readResult.Result);
        }

        [Fact]
        public async Task Read_PatientIdAndNoteIdOverload_WhenCalled_ReturnsEntity()
        {
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var service = NoteService();
            service.ReadByNoteId_Return = null;
            var controller = Controller(service);

            var readResult = await controller.Read(patientId, noteId);

            Assert.Equal(service.ReadByNoteId_Return, readResult.Value);
        }

        [Fact]
        public void Update_NoteOverload_HasHttpPutAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpPutAttribute>("Update");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Update_NoteOverload_NoteHasFromBodyAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromBodyAttribute>("Update",
                "note");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Update_NoteOverload_WhenCalled_CallsReadOnPatientService()
        {
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var service = NoteService();
            service.ReadByNoteId_Return = null;
            var controller = Controller(service);
            string text = "note text";
            var note = new PatientNoteEntity()
            {
                PatientId = patientId,
                Id = noteId,
                Text = text,
            };

            await controller.Update(note);

            Assert.Equal(patientId, service.ReadByNoteId_ParamPatientId);
            Assert.Equal(noteId, service.ReadByNoteId_ParamNoteId);
        }

        [Fact]
        public async Task Update_NoteOverload_NotesServiceReturnsNull_ReturnsNotFound()
        {
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var service = NoteService();
            service.ReadByNoteId_Return = null;
            var controller = Controller(service);
            var text = "note text";
            var note = new PatientNoteEntity()
            {
                PatientId = patientId,
                Id = noteId,
                Text = text,
            };

            var updateResult = await controller.Update(note);

            Assert.IsType<NotFoundResult>(updateResult);
        }

        [Theory]
        [InlineData("note text1")]
        [InlineData("note text2")]
        public async Task Update_NoteOverload_WhenCalled_CallsUpdateOnNotesService(string text)
        {
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var service = NoteService();
            service.ReadByNoteId_Return = NoteEntity();
            var controller = Controller(service);
            var note = new PatientNoteEntity()
            {
                PatientId = patientId,
                Id = noteId,
                Text = text,
            };

            await controller.Update(note);

            Assert.Equal(noteId, service.Update_ParamNote!.Id);
            Assert.Equal(patientId, service.Update_ParamNote.PatientId);
            Assert.Equal(text, service.Update_ParamNote.Text);
        }

        [Fact]
        public async Task Update_NoteOverload_WhenCalled_ReturnsNoContentResult()
        {
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var service = NoteService();
            service.ReadByNoteId_Return = NoteEntity();
            var controller = Controller(service);
            var text = "note text";
            var note = new PatientNoteEntity()
            {
                PatientId = patientId,
                Id = noteId,
                Text = text,
            };

            var updateResult = await controller.Update(note);

            Assert.IsType<NoContentResult>(updateResult);
        }

        static MockPatientNoteService NoteService() => new();
        static PatientNotesController Controller(MockPatientNoteService notesService) => new(notesService);
        static PatientNoteData NoteData(string text = "note text") => new()
        {
            PatientId = Guid.NewGuid(),
            Text = text,
        };
        static PatientNoteEntity NoteEntity() => new()
        {
            Id = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            Text = "note text",
        };
    }
}