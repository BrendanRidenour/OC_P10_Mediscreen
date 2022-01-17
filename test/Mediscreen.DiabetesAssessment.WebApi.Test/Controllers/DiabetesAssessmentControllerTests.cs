using Mediscreen.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen.Controllers
{
    public class DiabetesAssessmentControllerTests
    {
        [Fact]
        public void InheritsController()
        {
            Assert.True(typeof(ControllerBase).IsAssignableFrom(typeof(DiabetesAssessmentController)));
        }

        [Fact]
        public void HasApiControllerAttribute()
        {
            var attribute = GetClassAttribute<DiabetesAssessmentController, ApiControllerAttribute>();

            Assert.NotNull(attribute);
        }

        [Fact]
        public void HasRouteAttribute()
        {
            var attribute = GetClassAttribute<DiabetesAssessmentController, RouteAttribute>();

            Assert.Equal("[controller]", attribute.Template);
        }

        [Fact]
        public void GenerateDiabetesReport_PatientIdOverload_HasHttpPostAttribute()
        {
            var attribute = GetMethodAttribute<DiabetesAssessmentController, HttpPostAttribute>(
                "GenerateDiabetesReport");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void GenerateDiabetesReport_PatientIdOverload_HasRouteAttribute()
        {
            var attribute = GetMethodAttribute<DiabetesAssessmentController, RouteAttribute>(
                "GenerateDiabetesReport");

            Assert.Equal("{patientId}", attribute.Template);
        }

        [Fact]
        public void GenerateDiabetesReport_PatientIdOverload_PatientIdHasFromRouteAttribute()
        {
            var attribute = GetParameterAttribute<DiabetesAssessmentController, FromRouteAttribute>(
                "GenerateDiabetesReport", "patientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task GenerateDiabetesReport_PatientIdOverload_WhenCalled_CallsGenerateDiabetesReportOnAssessmentService()
        {
            var service = AssessmentService();
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            await controller.GenerateDiabetesReport(patientId);

            Assert.Equal(service.GenerateDiabetesReport_ParamPatientId, patientId);
        }

        [Fact]
        public async Task GenerateDiabetesReport_PatientIdOverload_AssessmentServiceGenerateDiabetesReportReturnsNull_ReturnsNotFound()
        {
            var service = AssessmentService();
            service.GenerateDiabetesReport_Return = null;
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            var actionResult = await controller.GenerateDiabetesReport(patientId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Theory]
        [InlineData("report1")]
        [InlineData("report2")]
        public async Task GenerateDiabetesReport_PatientIdOverload_WhenCalled_ReturnsAssessmentServiceReport(
            string report)
        {
            var service = AssessmentService();
            service.GenerateDiabetesReport_Return = report;
            var controller = Controller(service);
            var patientId = Guid.NewGuid();

            var actionResult = await controller.GenerateDiabetesReport(patientId);

            var result = Assert.IsType<string>(actionResult.Value);
            Assert.Equal(report, result);
        }

        static MockPatientDiabetesAssessmentService AssessmentService() => new();
        static DiabetesAssessmentController Controller(MockPatientDiabetesAssessmentService? service = null) =>
            new(service ?? AssessmentService());
    }
}