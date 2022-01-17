using Mediscreen.Mocks;
using Mediscreen.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public void CreateNote_PatientIdOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpGetAttribute>("CreateNote");

            Assert.Equal("create", attribute.Template);
        }

        [Fact]
        public void CreateNote_PatientIdOverload_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>("CreateNote",
                "patientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task CreateNote_PatientIdOverload_PatientServiceReturnsNull_ReturnsNotFoundResult()
        {
            var service = PatientService();
            service.ReadById_Return = null;
            var controller = Controller(patientService: service);
            var patientId = Guid.NewGuid();

            var result = await controller.CreateNote(patientId);

            Assert.Equal(patientId, service.ReadById_ParamId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateNote_PatientIdOverload_ReturnsView()
        {
            var service = PatientService();
            var controller = Controller(patientService: service);
            var patientId = Guid.NewGuid();

            var result = await controller.CreateNote(patientId);

            var view = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PatientViewModel<string>>(view.Model);
            Assert.Equal(service.ReadById_Return, viewModel.Patient);
            Assert.Equal(string.Empty, viewModel.Value);
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

        [Fact]
        public async Task CreateNote_PatientIdAndTextOverload_PatientServiceReturnsNull_ReturnsNotFound()
        {
            var patientService = PatientService();
            patientService.ReadById_Return = null;
            var controller = Controller(patientService: patientService);
            var patientId = Guid.NewGuid();

            var result = await controller.CreateNote(patientId, "note text");

            Assert.Equal(patientId, patientService.ReadById_ParamId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task CreateNote_PatientIdAndTextOverload_ModelStateIsNotValid_ReturnsView(string text)
        {
            var patientService = PatientService();
            var noteService = NoteService();
            var controller = Controller(noteService, patientService);
            controller.ModelState.AddModelError(string.Empty, "Model Error");
            var patientId = Guid.NewGuid();

            var result = await controller.CreateNote(patientId, text);

            var view = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PatientViewModel<string>>(view.Model);
            Assert.Equal(patientService.ReadById_Return, viewModel.Patient);
            Assert.Equal(text, viewModel.Value);
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
            Assert.Equal("ReadNotes", createdAtAction.ActionName);
            Assert.Equal(service.Create_Return!.PatientId, (Guid)createdAtAction.RouteValues!["patientId"]!);
        }

        [Fact]
        public void ReadNotes_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientNotesController, HttpGetAttribute>("ReadNotes");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void ReadNotes_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientNotesController, FromRouteAttribute>("ReadNotes",
                "patientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task ReadNotes_WhenCalled_CallsReadOnPatientService()
        {
            var service = PatientService();
            var controller = Controller(patientService: service);
            var patientId = Guid.NewGuid();

            await controller.ReadNotes(patientId);

            Assert.Equal(service.ReadById_ParamId, patientId);
        }

        [Fact]
        public async Task ReadNotes_ReadOnPatientServiceReturnsNull_ReturnsNotFound()
        {
            var service = PatientService();
            service.ReadById_Return = null;
            var controller = Controller(patientService: service);
            var patientId = Guid.NewGuid();

            var readResult = await controller.ReadNotes(patientId);

            Assert.IsType<NotFoundResult>(readResult);
        }

        [Fact]
        public async Task ReadNotes_WhenCalled_CallsReadOnNoteService()
        {
            var service = NoteService();
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            await controller.ReadNotes(patientId);

            Assert.Equal(service.ReadByPatientId_ParamPatientId, patientId);
        }

        [Fact]
        public async Task ReadNotes_WhenCalled_ReturnsView()
        {
            var patientService = PatientService();
            var noteService = NoteService();
            var controller = Controller(noteService, patientService);
            var patientId = Guid.NewGuid();

            var readResult = await controller.ReadNotes(patientId);

            var view = Assert.IsType<ViewResult>(readResult);
            var viewModel = Assert.IsType<PatientViewModel<IEnumerable<PatientNoteEntity>>>(view.Model);
            Assert.Equal(patientService.ReadById_Return, viewModel.Patient);
            Assert.Equal(noteService.ReadByPatientId_Return, viewModel.Value);
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
        public async Task UpdateNote_PatientIdAndNoteIdOverload_PatientServiceReturnsNull_ReturnsNotFound()
        {
            var service = PatientService();
            service.ReadById_Return = null;
            var controller = Controller(patientService: service);
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var updateResult = await controller.UpdateNote(patientId, noteId);

            Assert.Equal(patientId, service.ReadById_ParamId);
            Assert.IsType<NotFoundResult>(updateResult);
        }

        [Fact]
        public async Task UpdateNote_PatientIdAndNoteIdOverload_NoteServiceReturnsNull_ReturnsNotFound()
        {
            var service = NoteService();
            service.ReadByNoteId_Return = null;
            var controller = Controller(service);
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var updateResult = await controller.UpdateNote(patientId, noteId);

            Assert.Equal(patientId, service.ReadByNoteId_ParamPatientId);
            Assert.Equal(noteId, service.ReadByNoteId_ParamNoteId);
            Assert.IsType<NotFoundResult>(updateResult);
        }

        [Fact]
        public async Task UpdateNote_PatientIdAndNoteIdOverload_WhenCalled_ReturnsView()
        {
            var patientService = PatientService();
            var noteService = NoteService();
            noteService.ReadByNoteId_Return = NoteEntity();
            var controller = Controller(noteService, patientService);

            var updateResult = await controller.UpdateNote(patientId: Guid.NewGuid(), noteId: Guid.NewGuid());

            var view = Assert.IsType<ViewResult>(updateResult);
            var viewModel = Assert.IsType<PatientViewModel<string>>(view.Model);
            Assert.Equal(patientService.ReadById_Return, viewModel.Patient);
            Assert.Equal(noteService.ReadByNoteId_Return.Text, viewModel.Value);
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
        public async Task UpdateNote_PatientIdAndNoteIdAndTextOverload_PatientServiceReturnsNull_ReturnsNotFound(
            string text)
        {
            var service = PatientService();
            service.ReadById_Return = null;
            var controller = Controller(patientService: service);
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var updateResult = await controller.UpdateNote(patientId, noteId, text);

            Assert.Equal(patientId, service.ReadById_ParamId);
            Assert.IsType<NotFoundResult>(updateResult);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task UpdateNote_PatientIdAndNoteIdAndTextOverload_ModelStateNotValid_ReturnsView(
            string text)
        {
            var service = PatientService();
            var controller = Controller(patientService: service);
            controller.ModelState.AddModelError(string.Empty, "Error message");
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            var updateResult = await controller.UpdateNote(patientId, noteId, text);

            var view = Assert.IsType<ViewResult>(updateResult);
            var viewModel = Assert.IsType<PatientViewModel<string>>(view.Model);
            Assert.Equal(service.ReadById_Return, viewModel.Patient);
            Assert.Equal(text, viewModel.Value);
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
            Assert.Equal("ReadNotes", redirectToAction.ActionName);
            Assert.Equal(patientId, (Guid)redirectToAction.RouteValues!["patientId"]!);
        }

        static MockPatientService PatientService() => new()
        {
            ReadById_Return = new(),
        };
        static MockPatientNoteService NoteService() => new();
        static PatientNotesController Controller(MockPatientNoteService? noteService = null,
            MockPatientService? patientService = null) =>
            new(patientService ?? PatientService(), noteService ?? NoteService());
        static PatientNoteEntity NoteEntity(string text = "note text") => new()
        {
            PatientId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Text = text,
        };
    }
}