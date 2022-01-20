using Mediscreen.Data.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Mediscreen.Data
{
    public class DemographicsPatientServiceTests
    {
        [Theory]
        [InlineData("GN1")]
        [InlineData("GN2")]
        public async Task Create_PatientOverload_WhenCalled_CallsPostOnHttp(string givenName)
        {
            var http = HttpClient();
            var patientData = PatientData(givenName);
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.Created, PatientEntity(patientData));
            var patientService = PatientService(http);

            await patientService.Create(patientData);

            Assert.Equal(HttpMethod.Post, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri("https://example.com/patients"),
                http.SendAsync_ParamRequest.RequestUri);

            var postedData = await http.SendAsync_ParamRequest.Content!.ReadFromJsonAsync<PatientData>();
            Assert.Equal(patientData.GivenName, postedData!.GivenName);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Create_PatientOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = PatientService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Create(PatientData());
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Read_EmptyOverload_WhenCalled_CallsGetOnHttp(int count)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, PatientEntityList(count));
            var patientService = PatientService(http);

            await patientService.Read();

            Assert.Equal(HttpMethod.Get, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri("https://example.com/patients"),
                http.SendAsync_ParamRequest.RequestUri);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Read_EmptyOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = PatientService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Read();
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Read_EmptyOverload_WhenCalled_ReturnsEntities(int count)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, PatientEntityList(count));
            var patientService = PatientService(http);

            var entities = await patientService.Read();

            Assert.Equal(count, entities.Count());
        }

        [Fact]
        public async Task Read_IdOverload_WhenCalled_CallsGetOnHttp()
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, PatientEntity());
            var patientService = PatientService(http);
            var id = Guid.NewGuid();

            await patientService.Read(id);

            Assert.Equal(HttpMethod.Get, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri($"https://example.com/patients/{id}"),
                http.SendAsync_ParamRequest.RequestUri);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Read_IdOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = PatientService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Read(Guid.NewGuid());
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        [Fact]
        public async Task Read_IdOverload_404SuccessStatusCode_ReturnsNull()
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.NotFound);
            var patientService = PatientService(http);

            var result = await patientService.Read(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task Read_IdOverload_WhenCalled_ReturnsEntity()
        {
            var http = HttpClient();
            var id = Guid.NewGuid();
            var entity = PatientEntity();
            entity.Id = id;
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, entity);
            var patientService = PatientService(http);

            var result = await patientService.Read(id);

            Assert.Equal(id, result!.Id);
        }

        [Theory]
        [InlineData("famname1")]
        [InlineData("famname2")]
        public async Task Read_FamilyNameOverload_WhenCalled_CallsGetOnHttp(string familyName)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, PatientEntity());
            var patientService = PatientService(http);

            await patientService.Read(familyName);

            Assert.Equal(HttpMethod.Get, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri($"https://example.com/patients/{familyName}"),
                http.SendAsync_ParamRequest.RequestUri);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Read_FamilyNameOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = PatientService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Read(familyName: "famname");
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        [Fact]
        public async Task Read_FamilyNameOverload_404SuccessStatusCode_ReturnsNull()
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.NotFound);
            var patientService = PatientService(http);

            var result = await patientService.Read(familyName: "famname");

            Assert.Null(result);
        }

        [Theory]
        [InlineData("famname1")]
        [InlineData("famname2")]
        public async Task Read_FamilyNameOverload_WhenCalled_ReturnsEntity(string familyName)
        {
            var http = HttpClient();
            var entity = PatientEntity();
            entity.FamilyName = familyName;
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, entity);
            var patientService = PatientService(http);

            var result = await patientService.Read(familyName);

            Assert.Equal(familyName, result!.FamilyName);
        }

        [Theory]
        [InlineData("GN1")]
        [InlineData("GN2")]
        public async Task Update_PatientOverload_WhenCalled_CallsPutOnHttp(string givenName)
        {
            var http = HttpClient();
            var patientEntity = PatientEntity(PatientData(givenName));
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.NoContent);
            var patientService = PatientService(http);

            await patientService.Update(patientEntity);

            Assert.Equal(HttpMethod.Put, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri($"https://example.com/patients/{patientEntity.Id}"),
                http.SendAsync_ParamRequest.RequestUri);

            var putData = await http.SendAsync_ParamRequest.Content!.ReadFromJsonAsync<PatientData>();
            Assert.Equal(patientEntity.GivenName, putData!.GivenName);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Update_PatientOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = PatientService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Update(PatientEntity());
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        static HttpResponseMessage ResponseMessage(HttpStatusCode code) => new(code);
        static HttpResponseMessage ResponseMessage<T>(HttpStatusCode code, T content)
        {
            var response = ResponseMessage(code);

            if (content is not null)
            {
                var json = JsonSerializer.Serialize(content);

                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return response;
        }
        static MockHttpClient HttpClient() => new(new MockHttpMessageHandler());
        static DemographicsPatientService PatientService(MockHttpClient http) => new(http);
        static PatientData PatientData(string givenName = "GN") => new()
        {
            GivenName = givenName,
            FamilyName = "FN",
            DateOfBirth = new Date(2000, 1, 1),
            BiologicalSex = BiologicalSex.Male,
        };
        static PatientEntity PatientEntity(PatientData? data = null) =>
            new(Guid.NewGuid(), data ?? PatientData());
        static List<PatientEntity> PatientEntityList(int count = 0)
        {
            var result = new List<PatientEntity>();

            for (var i = 0; i < count; i++)
                result.Add(PatientEntity());

            return result;
        }
    }
}