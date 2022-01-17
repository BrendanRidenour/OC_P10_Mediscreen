using Mediscreen.Data.Mocks;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Mediscreen.Data
{
    public class PatientDiabetesAssessmentServiceTests
    {
        [Theory]
        [InlineData("report1")]
        [InlineData("report2")]
        public async Task GenerateDiabetesReport_PatientIdOverload_WhenCalled_CallsGetOnHttp(string report)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, report);
            var patientService = DiabetesService(http);
            var patientId = Guid.NewGuid();

            await patientService.GenerateDiabetesReport(patientId);

            Assert.Equal(HttpMethod.Get, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri($"https://example.com/diabetesassessment/{patientId}"),
                http.SendAsync_ParamRequest.RequestUri);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task GenerateDiabetesReport_PatientIdOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = DiabetesService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.GenerateDiabetesReport(patientId: Guid.NewGuid());
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        [Theory]
        [InlineData("report1")]
        [InlineData("report2")]
        public async Task Read_PatientIdOverload_WhenCalled_ReturnsEntities(string report)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, report);
            var patientService = DiabetesService(http);

            var result = await patientService.GenerateDiabetesReport(patientId: Guid.NewGuid());

            Assert.Equal(report, result);
        }

        static HttpResponseMessage ResponseMessage(HttpStatusCode code) => new(code);
        static HttpResponseMessage ResponseMessage(HttpStatusCode code, string content)
        {
            var response = ResponseMessage(code);

            if (content is not null)
            {
                response.Content = new StringContent(content, Encoding.UTF8, "text/plain");
            }

            return response;
        }
        static MockHttpClient HttpClient() => new(new MockHttpMessageHandler());
        static PatientDiabetesAssessmentService DiabetesService(MockHttpClient http) => new(http);
    }
}