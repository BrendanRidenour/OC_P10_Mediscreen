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
    public class HistoryPatientNoteServiceTests
    {
        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task Create_NoteOverload_WhenCalled_CallsPostOnHttp(string text)
        {
            var http = HttpClient();
            var patientData = NoteData(text);
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.Created, NoteEntity(patientData));
            var patientService = NoteService(http);

            await patientService.Create(patientData);

            Assert.Equal(HttpMethod.Post, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri("https://example.com/patientnotes"),
                http.SendAsync_ParamRequest.RequestUri);

            var postedData = await http.SendAsync_ParamRequest.Content!.ReadFromJsonAsync<PatientNoteData>();
            Assert.Equal(patientData.Text, postedData!.Text);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Create_NoteOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = NoteService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Create(NoteData());
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Read_PatientIdOverload_WhenCalled_CallsGetOnHttp(int count)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, NoteEntityList(count));
            var patientService = NoteService(http);
            var patientId = Guid.NewGuid();

            await patientService.Read(patientId);

            Assert.Equal(HttpMethod.Get, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri($"https://example.com/patientnotes/{patientId}"),
                http.SendAsync_ParamRequest.RequestUri);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Read_PatientIdOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = NoteService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Read(patientId: Guid.NewGuid());
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Read_PatientIdOverload_WhenCalled_ReturnsEntities(int count)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, NoteEntityList(count));
            var patientService =  NoteService(http);

            var entities = await patientService.Read(patientId: Guid.NewGuid());

            Assert.Equal(count, entities.Count());
        }

        [Fact]
        public async Task Read_PatientIdAndNoteIdOverload_WhenCalled_CallsGetOnHttp()
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, NoteEntity());
            var patientService = NoteService(http);
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();

            await patientService.Read(patientId, noteId);

            Assert.Equal(HttpMethod.Get, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri($"https://example.com/patientnotes/{patientId}/{noteId}"),
                http.SendAsync_ParamRequest.RequestUri);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Read_PatientIdAndNoteIdOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = NoteService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Read(patientId: Guid.NewGuid(), noteId: Guid.NewGuid());
            });

            Assert.Equal((HttpStatusCode)code, exception.StatusCode);
        }

        [Fact]
        public async Task Read_PatientIdAndNoteIdOverload_WhenCalled_ReturnsEntity()
        {
            var http = HttpClient();
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var entity = NoteEntity();
            entity.PatientId = patientId;
            entity.Id = noteId;
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.OK, entity);
            var patientService = NoteService(http);

            var result = await patientService.Read(patientId, noteId);

            Assert.Equal(patientId, result!.PatientId);
            Assert.Equal(noteId, result!.Id);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task Update_NoteOverload_WhenCalled_CallsPutOnHttp(string text)
        {
            var http = HttpClient();
            var updateEntity = NoteEntity(NoteData(text));
            http.SendAsync_Return = ResponseMessage(HttpStatusCode.NoContent);
            var service = NoteService(http);

            await service.Update(updateEntity);

            Assert.Equal(HttpMethod.Put, http.SendAsync_ParamRequest.Method);
            Assert.Equal(new Uri($"https://example.com/patientnotes"),
                http.SendAsync_ParamRequest.RequestUri);

            var putEntity = await http.SendAsync_ParamRequest.Content!.ReadFromJsonAsync<PatientNoteEntity>();
            Assert.Equal(updateEntity.PatientId, putEntity!.PatientId);
            Assert.Equal(updateEntity.Id, putEntity.Id);
            Assert.Equal(updateEntity.Text, putEntity.Text);
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(500)]
        public async Task Update_NoteOverload_NotSuccessStatusCode_Throws(int code)
        {
            var http = HttpClient();
            http.SendAsync_Return = ResponseMessage((HttpStatusCode)code);
            var patientService = NoteService(http);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await patientService.Update(NoteEntity());
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
        static HistoryPatientNoteService NoteService(MockHttpClient http) => new(http);
        static PatientNoteData NoteData(string text = "note text") => new()
        {
            PatientId = Guid.NewGuid(),
            Text = text,
        };
        static PatientNoteEntity NoteEntity(PatientNoteData? data = null) =>
            new(Guid.NewGuid(), data ?? NoteData());
        static List<PatientNoteEntity> NoteEntityList(int count = 0)
        {
            var result = new List<PatientNoteEntity>();

            for (var i = 0; i < count; i++)
                result.Add(NoteEntity());

            return result;
        }
    }
}