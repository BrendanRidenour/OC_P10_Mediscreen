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
            Assert.True(typeof(ControllerBase).IsAssignableFrom(typeof(PatientsController)));
        }

        [Fact]
        public void HasApiControllerAttribute()
        {
            var attribute = GetClassAttribute<PatientsController, ApiControllerAttribute>();

            Assert.NotNull(attribute);
        }

        [Fact]
        public void HasRouteAttribute()
        {
            var attribute = GetClassAttribute<PatientsController, RouteAttribute>();

            Assert.Equal("[controller]", attribute.Template);
        }

        [Fact]
        public void Create_PatientOverload_HasHttpPostAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpPostAttribute>("Create");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Create_PatientOverload_PatientHasFromBodyAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromBodyAttribute>("Create",
                "patient");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Create_PatientOverload_WhenCalled_CallsCreateOnPatientService()
        {
            var patientService = PatientService();
            var controller = Controller(patientService);
            var patient = PatientData();

            await controller.Create(patient);

            Assert.Equal(patientService.Create_ParamPatient, patient);
        }

        [Fact]
        public async Task Create_PatientOverload_WhenCalled_ReturnsCreatedAtActionResult()
        {
            var patientService = PatientService();
            var controller = Controller(patientService);
            var patient = PatientData();

            var createResult = await controller.Create(patient);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(createResult.Result);
            Assert.Equal("Read", createdAtAction.ActionName);
            Assert.Equal(patientService.Create_Return, createdAtAction.Value);
            Assert.Equal(patientService.Create_Return!.Id, createdAtAction.RouteValues!["id"]!);
        }

        [Fact]
        public void Read_EmptyOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpGetAttribute>("Read");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Read_EmptyOverload_WhenCalled_ReturnsReadOnPatientService()
        {
            var patientService = PatientService();
            var controller = Controller(patientService);

            var entities = await controller.Read();

            Assert.Equal(patientService.Read_Return, entities);
        }

        [Fact]
        public void Read_IdOverload_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpGetAttribute>("Read",
                methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Read_IdOverload_HasRouteAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, RouteAttribute>("Read",
                methodIndex: 1);

            Assert.Equal("{id}", attribute.Template);
        }

        [Fact]
        public void Read_IdOverload_IdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromRouteAttribute>("Read",
                "id", methodIndex: 1);

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Read_IdOverload_WhenCalled_CallsReadOnPatientService()
        {
            var id = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = null;
            var controller = Controller(patientService);

            await controller.Read(id);

            Assert.Equal(id, patientService.ReadById_ParamId);
        }

        [Fact]
        public async Task Read_IdOverload_PatientServiceReturnsNull_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = null;
            var controller = Controller(patientService);

            var readResult = await controller.Read(id);

            Assert.IsType<NotFoundResult>(readResult.Result);
        }

        [Fact]
        public async Task Read_IdOverload_WhenCalled_ReturnsEntity()
        {
            var id = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = PatientEntity();
            var controller = Controller(patientService);

            var readResult = await controller.Read(id);

            Assert.Equal(patientService.ReadById_Return, readResult.Value);
        }

        [Fact]
        public void Update_IdAndPatientOverload_HasHttpPutAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, HttpPutAttribute>("Update");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Update_IdAndPatientOverload_HasRouteAttribute()
        {
            var attribute = GetMethodAttribute<PatientsController, RouteAttribute>("Update");

            Assert.Equal("{id}", attribute.Template);
        }

        [Fact]
        public void Update_IdAndPatientOverload_IdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromRouteAttribute>("Update",
                "id");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void Update_IdAndPatientOverload_PatientHasFromBodyAttribute()
        {
            var attribute = GetParameterAttribute<PatientsController, FromBodyAttribute>("Update",
                "patient");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task Update_IdAndPatientOverload_WhenCalled_CallsReadOnPatientService()
        {
            var id = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = null;
            var controller = Controller(patientService);
            var patient = PatientData();

            await controller.Update(id, patient);

            Assert.Equal(id, patientService.ReadById_ParamId);
        }

        [Fact]
        public async Task Update_IdAndPatientOverload_PatientServiceReturnsNull_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = null;
            var controller = Controller(patientService);
            var patient = PatientData();

            var updateResult = await controller.Update(id, patient);

            Assert.IsType<NotFoundResult>(updateResult);
        }

        [Fact]
        public async Task Update_IdAndPatientOverload_WhenCalled_CallsUpdateOnPatientService()
        {
            var id = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = PatientEntity();
            var controller = Controller(patientService);
            var patient = PatientData();

            await controller.Update(id, patient);

            Assert.Equal(patientService.Update_ParamEntity, patientService.ReadById_Return);
        }

        [Fact]
        public async Task Update_IdAndPatientOverload_WhenCalled_ReturnsNoContentResult()
        {
            var id = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = PatientEntity();
            var controller = Controller(patientService);
            var patient = PatientData();

            var updateResult = await controller.Update(id, patient);

            Assert.IsType<NoContentResult>(updateResult);
        }

        static MockPatientService PatientService() => new();
        static PatientsController Controller(MockPatientService? patientService = null) =>
            new(patientService ?? PatientService());
        static PatientData PatientData() => new();
        static PatientEntity PatientEntity() => new();
    }
}