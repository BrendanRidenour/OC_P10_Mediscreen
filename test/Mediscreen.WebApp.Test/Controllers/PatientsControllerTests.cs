using Mediscreen.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen.Controllers
{
    public class PatientsControllerTests
    {
        [Fact]
        public void InheritsController()
        {
            Assert.True(typeof(Controller).IsAssignableFrom(typeof(PatientsController)));
        }

        [Fact]
        public void HasRouteAttribute()
        {
            var attribute = GetClassAttribute<PatientsController, RouteAttribute>();

            Assert.Equal("[controller]", attribute.Template);
        }

        [Fact]
        public void CreatePatient_EmptyOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpGetAttribute>("CreatePatient");

            Assert.Equal("create", attribute.Template);
        }

        [Fact]
        public void CreatePatient_EmptyOverload_ReturnsView()
        {
            var controller = Controller();

            var result = controller.CreatePatient();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreatePatient_PatientOverload_HasHttpPostAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpPostAttribute>("CreatePatient",
                methodIndex: 1);

            Assert.Equal("create", attribute.Template);
        }

        [Fact]
        public void CreatePatient_PatientOverload_PatientHasFromFormAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromFormAttribute>(
                "CreatePatient", "patient", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task CreatePatient_PatientOverload_ModelStateIsNotValid_ReturnsView()
        {
            var patientService = PatientService();
            var controller = Controller(patientService);
            controller.ModelState.AddModelError(string.Empty, "Model Error");
            var patient = PatientData();

            var result = await controller.CreatePatient(patient);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(patient, view.Model);
        }

        [Fact]
        public async Task CreatePatient_PatientOverload_CallsCreateOnPatientService()
        {
            var patientService = PatientService();
            var controller = Controller(patientService);
            var patient = PatientData();

            await controller.CreatePatient(patient);

            Assert.Equal(patient, patientService.Create_ParamPatient);
        }

        [Fact]
        public async Task CreatePatient_PatientOverload_ReturnsRedirectToActionResult()
        {
            var patientService = PatientService();
            var controller = Controller(patientService);
            var patient = PatientData();

            var createResult = await controller.CreatePatient(patient);

            var createdAtAction = Assert.IsType<RedirectToActionResult>(createResult);
            Assert.Equal("ReadPatient", createdAtAction.ActionName);
            Assert.Equal(patientService.Create_Return!.Id, (Guid)createdAtAction.RouteValues!["id"]!);
        }

        [Fact]
        public void ReadPatients_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpGetAttribute>("ReadPatients");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task ReadPatients_WhenCalled_ReturnsView()
        {
            var patientService = PatientService();
            var controller = Controller(patientService);

            var readResult = await controller.ReadPatients();

            var view = Assert.IsType<ViewResult>(readResult);
            Assert.Equal(patientService.Read_Return, view.Model);
        }

        [Fact]
        public void ReadPatient_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpGetAttribute>("ReadPatient");

            Assert.Equal("{id}", attribute.Template);
        }

        [Fact]
        public void ReadPatient_IdOverload_IdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromRouteAttribute>(
                "ReadPatient", "id");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task ReadPatient_IdOverload_PatientServiceReturnsNull_ReturnsNotFound()
        {
            var patientService = PatientService();
            patientService.ReadById_Return = null;
            var controller = Controller(patientService);

            var readResult = await controller.ReadPatient(id: Guid.NewGuid());

            Assert.IsType<NotFoundResult>(readResult);
        }

        [Fact]
        public async Task ReadPatient_IdOverload_WhenCalled_ReturnsView()
        {
            var patientService = PatientService();
            patientService.ReadById_Return = PatientEntity();
            var controller = Controller(patientService);

            var readResult = await controller.ReadPatient(id: Guid.NewGuid());

            var view = Assert.IsType<ViewResult>(readResult);
            Assert.Equal(patientService.ReadById_Return, view.Model);
        }

        [Fact]
        public void UpdatePatient_IdOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpGetAttribute>("UpdatePatient");

            Assert.Equal("{id}/update", attribute.Template);
        }

        [Fact]
        public void UpdatePatient_IdOverload_IdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromRouteAttribute>(
                "UpdatePatient", "id");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task UpdatePatient_IdOverload_PatientServiceReturnsNull_ReturnsNotFound()
        {
            var patientService = PatientService();
            patientService.ReadById_Return = null;
            var controller = Controller(patientService);

            var updateResult = await controller.UpdatePatient(id: Guid.NewGuid());

            Assert.IsType<NotFoundResult>(updateResult);
        }

        [Fact]
        public async Task UpdatePatient_IdOverload_WhenCalled_ReturnsView()
        {
            var patientService = PatientService();
            patientService.ReadById_Return = PatientEntity();
            var controller = Controller(patientService);

            var updateResult = await controller.UpdatePatient(id: Guid.NewGuid());

            var view = Assert.IsType<ViewResult>(updateResult);
            Assert.Equal(patientService.ReadById_Return, view.Model);
        }

        [Fact]
        public void UpdatePatient_IdAndPatientOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpPostAttribute>("UpdatePatient",
                methodIndex: 1);

            Assert.Equal("{id}/update", attribute.Template);
        }

        [Fact]
        public void UpdatePatient_IdAndPatientOverload_IdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromRouteAttribute>(
                "UpdatePatient", "id", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void UpdatePatient_IdAndPatientOverload_PatientHasFromFormAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromFormAttribute>(
                "UpdatePatient", "patient", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task UpdatePatient_IdAndPatientOverload_ModelStateNotValid_ReturnsView()
        {
            var controller = Controller();
            controller.ModelState.AddModelError(string.Empty, "Error message");
            var patient = PatientData();

            var updateResult = await controller.UpdatePatient(id: Guid.NewGuid(), patient);

            var view = Assert.IsType<ViewResult>(updateResult);
            Assert.Equal(patient, view.Model);
        }

        [Theory]
        [InlineData("GN1")]
        [InlineData("GN2")]
        public async Task UpdatePatient_IdAndPatientOverload_WhenCalled_CallsUpdateOnPatientService(
            string givenName)
        {
            var patientService = PatientService();
            var controller = Controller(patientService);
            var id = Guid.NewGuid();
            var patient = PatientData();
            patient.GivenName = givenName;

            await controller.UpdatePatient(id, patient);

            Assert.Equal(id, patientService.Update_ParamEntity!.Id);
            Assert.Equal(givenName, patientService.Update_ParamEntity.GivenName);
        }

        [Theory]
        [InlineData("GN1")]
        [InlineData("GN2")]
        public async Task UpdatePatient_IdAndPatientOverload_WhenCalled_ReturnsRedirectToAction(
            string givenName)
        {
            var patientService = PatientService();
            var controller = Controller(patientService);
            var id = Guid.NewGuid();
            var patient = PatientData();
            patient.GivenName = givenName;

            var updateResult = await controller.UpdatePatient(id, patient);

            var redirectToAction = Assert.IsType<RedirectToActionResult>(updateResult);
            Assert.Equal("ReadPatient", redirectToAction.ActionName);
            Assert.Equal(id, (Guid)redirectToAction.RouteValues!["id"]!);
        }

        static MockPatientService PatientService() => new();
        static PatientsController Controller(MockPatientService? patientService = null) =>
            new(patientService ?? PatientService());
        static PatientData PatientData() => new();
        static PatientEntity PatientEntity() => new();
    }
}