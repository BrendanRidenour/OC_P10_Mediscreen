using Mediscreen.Mocks;
using Mediscreen.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen.Controllers
{
    public class PatientDiabetesAssessmentControllerTests
    {
        [Fact]
        public void InheritsController()
        {
            Assert.True(typeof(Controller).IsAssignableFrom(typeof(PatientDiabetesAssessmentController)));
        }

        [Fact]
        public void HasRouteAttribute()
        {
            var attribute = GetClassAttribute<PatientDiabetesAssessmentController, RouteAttribute>();

            Assert.Equal("[controller]/{patientId}", attribute.Template);
        }

        [Fact]
        public void ReadReport_HasHttpGetAttribute()
        {
            var attribute = GetMethodAttribute<PatientDiabetesAssessmentController, HttpGetAttribute>("ReadReport");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void ReadReport_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<PatientDiabetesAssessmentController, FromRouteAttribute>(
                "ReadReport", "patientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task ReadReport_WhenCalled_CallsReadOnPatientService()
        {
            var service = PatientService();
            var controller = Controller(patientService: service);
            var patientId = Guid.NewGuid();

            await controller.ReadReport(patientId);

            Assert.Equal(service.ReadById_ParamId, patientId);
        }

        [Fact]
        public async Task ReadReport_ReadOnPatientServiceReturnsNull_ReturnsNotFound()
        {
            var service = PatientService();
            service.ReadById_Return = null;
            var controller = Controller(patientService: service);
            var patientId = Guid.NewGuid();

            var readResult = await controller.ReadReport(patientId);

            Assert.IsType<NotFoundResult>(readResult);
        }

        [Fact]
        public async Task ReadReport_WhenCalled_CallsGenerateDiabetesReportOnAssessmentService()
        {
            var service = DiabetesService();
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            await controller.ReadReport(patientId);

            Assert.Equal(service.GenerateDiabetesReport_ParamPatientId, patientId);
        }

        [Fact]
        public async Task ReadReport_WhenCalled_ReturnsView()
        {
            var patientService = PatientService();
            var diabetesService = DiabetesService();
            var controller = Controller(diabetesService, patientService);
            var patientId = Guid.NewGuid();

            var readResult = await controller.ReadReport(patientId);

            var view = Assert.IsType<ViewResult>(readResult);
            var viewModel = Assert.IsType<PatientViewModel<string>>(view.Model);
            Assert.Equal(patientService.ReadById_Return, viewModel.Patient);
            Assert.Equal(diabetesService.GenerateDiabetesReport_Return, viewModel.Value);
        }

        static MockPatientService PatientService() => new()
        {
            ReadById_Return = new(),
        };
        static MockPatientDiabetesAssessmentService DiabetesService() => new();
        static PatientDiabetesAssessmentController Controller(
            MockPatientDiabetesAssessmentService? diabetesService = null,
            MockPatientService? patientService = null) =>
            new(patientService ?? PatientService(), diabetesService ?? DiabetesService());
    }
}