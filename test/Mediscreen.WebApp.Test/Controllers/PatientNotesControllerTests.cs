using Mediscreen.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen.Controllers
{
    public class PatientNotesControllerTests
    {
        [Fact]
        public void InheritsController()
        {
            Assert.True(typeof(Controller).IsAssignableFrom(typeof(PatientNotesController)));
        }

        [Fact]
        public void HasRouteAttribute()
        {
            var attribute = GetClassAttribute<PatientNotesController, RouteAttribute>();

            Assert.Equal("[controller]/{patientId}", attribute.Template);
        }

        [Fact]
        public void CreateNote_EmptyOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpGetAttribute>("CreateNote");

            Assert.Equal("create", attribute.Template);
        }

        [Fact]
        public void CreateNote_EmptyOverload_ReturnsView()
        {
            var controller = Controller();

            var result = controller.CreateNote();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreateNote_PatientIdAndTextOverload_HasHttpPostAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpPostAttribute>("CreateNote",
                methodIndex: 1);

            Assert.Equal("create", attribute.Template);
        }

        [Fact]
        public void CreateNote_PatientIdAndTextOverload_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>("CreateNote",
                "patientId", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void CreateNote_PatientIdAndTextOverload_TextHasFromFormAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromFormAttribute>(
                "CreateNote", "text", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void CreateNote_PatientIdAndTextOverload_TextHasRequiredAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, RequiredAttribute>(
                "CreateNote", "text", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task CreateNote_PatientIdAndTextOverload_ModelStateIsNotValid_ReturnsView(string text)
        {
            var service = NoteService();
            var controller = Controller(service);
            controller.ModelState.AddModelError(string.Empty, "Model Error");
            var patientId = Guid.NewGuid();

            var result = await controller.CreateNote(patientId, text);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(text, view.Model);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task CreateNote_PatientOverload_CallsCreateOnNotesService(string text)
        {
            var notesService = NoteService();
            var controller = Controller(notesService);
            var patientId = Guid.NewGuid();

            await controller.CreateNote(patientId, text);

            Assert.Equal(patientId, notesService.Create_ParamNote!.PatientId);
            Assert.Equal(text, notesService.Create_ParamNote.Text);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task CreateNote_PatientIdAndTextOverload_ReturnsRedirectToActionResult(string text)
        {
            var service = NoteService();
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            var createResult = await controller.CreateNote(patientId, text);

            var createdAtAction = Assert.IsType<RedirectToActionResult>(createResult);
            Assert.Equal("ReadNote", createdAtAction.ActionName);
            Assert.Equal(service.Create_Return!.PatientId, (Guid)createdAtAction.RouteValues!["patientId"]!);
            Assert.Equal(service.Create_Return!.Id, (Guid)createdAtAction.RouteValues!["noteId"]!);
        }

        [Fact]
        public void ReadNotes_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpGetAttribute>("ReadNotes");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task ReadNotes_WhenCalled_ReturnsView()
        {
            var service = NoteService();
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            var readResult = await controller.ReadNotes(patientId);

            var view = Assert.IsType<ViewResult>(readResult);
            Assert.Equal(service.ReadByPatientId_Return, view.Model);
        }

        [Fact]
        public void ReadNote_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpGetAttribute>("ReadNote");

            Assert.Equal("{noteId}", attribute.Template);
        }

        [Fact]
        public void ReadNote_PatientIdAndNoteIdOverload_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>(
                "ReadNote", "patientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void ReadNote_PatientIdAndNoteIdOverload_NoteIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>(
                "ReadNote", "noteId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task ReadNote_PatientIdAndNoteIdOverload_NoteServiceReturnsNull_ReturnsNotFound()
        {
            var service = NoteService();
            service.ReadByNoteId_Return = null!;
            var controller = Controller(service);

            var readResult = await controller.ReadNote(patientId: Guid.NewGuid(), noteId: Guid.NewGuid());

            Assert.IsType<NotFoundResult>(readResult);
        }

        [Fact]
        public async Task ReadNote_PatientIdAndNoteIdOverload_WhenCalled_ReturnsView()
        {
            var service = NoteService();
            service.ReadByNoteId_Return = NoteEntity();
            var controller = Controller(service);

            var readResult = await controller.ReadNote(patientId: Guid.NewGuid(), noteId: Guid.NewGuid());

            var view = Assert.IsType<ViewResult>(readResult);
            Assert.Equal(service.ReadByNoteId_Return, view.Model);
        }

        [Fact]
        public void UpdateNote_PatientIdAndNoteIdOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpGetAttribute>("UpdateNote");

            Assert.Equal("{noteId}/update", attribute.Template);
        }

        [Fact]
        public void UpdateNote_PatientIdAndNoteIdOverload_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>(
                "UpdateNote", "patientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void UpdateNote_PatientIdAndNoteIdOverload_NoteIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>(
                "UpdateNote", "noteId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task UpdateNote_PatientIdAndNoteIdOverload_NoteServiceReturnsNull_ReturnsNotFound()
        {
            var service = NoteService();
            service.ReadByNoteId_Return = null;
            var controller = Controller(service);

            var updateResult = await controller.UpdateNote(patientId: Guid.NewGuid(), noteId: Guid.NewGuid());

            Assert.IsType<NotFoundResult>(updateResult);
        }

        [Fact]
        public async Task UpdateNote_PatientIdAndNoteIdOverload_WhenCalled_ReturnsView()
        {
            var service = NoteService();
            service.ReadByNoteId_Return = NoteEntity();
            var controller = Controller(service);

            var updateResult = await controller.UpdateNote(patientId: Guid.NewGuid(), noteId: Guid.NewGuid());

            var view = Assert.IsType<ViewResult>(updateResult);
            Assert.Equal(service.ReadByNoteId_Return, view.Model);
        }

        [Fact]
        public void UpdateNote_PatientIdAndNoteIdAndTextOverload_HasHttpGetPostAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpPostAttribute>("UpdateNote",
                methodIndex: 1);

            Assert.Equal("{noteId}/update", attribute.Template);
        }

        [Fact]
        public void UpdateNote_PatientIdAndNoteIdAndTextOverload_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>(
                "UpdateNote", "patientId", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void UpdateNote_PatientIdAndNoteIdAndTextOverload_NoteIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>(
                "UpdateNote", "noteId", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void UpdateNote_PatientIdAndNoteIdAndTextOverload_TextHasFromFormAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromFormAttribute>(
                "UpdateNote", "text", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void UpdateNote_PatientIdAndNoteIdAndTextOverload_TextHasRequiredAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, RequiredAttribute>(
                "UpdateNote", "text", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task UpdateNote_PatientIdAndNoteIdAndTextOverload_ModelStateNotValid_ReturnsView(
            string text)
        {
            var controller = Controller();
            controller.ModelState.AddModelError(string.Empty, "Error message");
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var updateResult = await controller.UpdateNote(patientId, noteId, text);

            var view = Assert.IsType<ViewResult>(updateResult);
            Assert.Equal(text, view.Model);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task UpdateNote_PatientIdAndNoteIdAndTextOverload_WhenCalled_CallsUpdateOnPatientService(
            string text)
        {
            var service = NoteService();
            var controller = Controller(service);
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            await controller.UpdateNote(patientId, noteId, text);

            Assert.Equal(patientId, service.Update_ParamNote!.PatientId);
            Assert.Equal(noteId, service.Update_ParamNote.Id);
            Assert.Equal(text, service.Update_ParamNote.Text);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task UpdateNote_PatientIdAndNoteIdAndTextOverload_WhenCalled_ReturnsRedirectToAction(
            string text)
        {
            var service = NoteService();
            var controller = Controller(service);
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var updateResult = await controller.UpdateNote(patientId, noteId, text);

            var redirectToAction = Assert.IsType<RedirectToActionResult>(updateResult);
            Assert.Equal("ReadNote", redirectToAction.ActionName);
            Assert.Equal(patientId, (Guid)redirectToAction.RouteValues!["patientId"]!);
            Assert.Equal(noteId, (Guid)redirectToAction.RouteValues!["noteId"]!);
        }

        static MockPatientNoteService NoteService() => new();
        static PatientNotesController Controller(MockPatientNoteService? noteService = null) =>
            new(noteService ?? NoteService());
        static PatientNoteEntity NoteEntity(string text = "note text") => new()
        {
            PatientId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Text = text,
        };
    }
}