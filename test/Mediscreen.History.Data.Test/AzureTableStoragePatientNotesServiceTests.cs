using Azure.Data.Tables;
using Mediscreen.Data.Mocks;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mediscreen.Data
{
    public class AzureTableStoragePatientNotesServiceTests
    {
        [Fact]
        public void ImplementsIPatientNotesService()
        {
            Assert.True(typeof(IPatientNotesService)
                .IsAssignableFrom(typeof(AzureTableStoragePatientNotesService)));
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task Create_WhenCalled_CallsTableClientAddEntityAsync(string text)
        {
            var note = PatientNoteData(text);
            var service = Service();

            var entity = await service.Create(note);

            Assert.True(entity.Id != Guid.Empty);
            Assert.Equal(service.TableClient.AddEntityAsync_ParamEntity!.Id, entity.Id);
            Assert.Equal(note.PatientId, entity.PatientId);
            Assert.Equal(note.Text, entity.Text);
        }

        [Fact]
        public async Task ReadByPatientId_WhenCalled_CallsTableClientQueryAsync()
        {
            var patientId = Guid.NewGuid();
            var service = Service();
            var partitionKey = PatientNoteTableEntity.CreatePartitionKey(patientId);

            var entities = await service.Read(patientId);

            Assert.Equal($"PartitionKey eq '{partitionKey}'", service.TableClient.QueryAsync_ParamFilter);
            Assert.Equal(2, entities.Count());
            Assert.Equal(service.TableClient.QueryAsync_Return.AsPages_Result!.ElementAt(0).Id,
                entities.ElementAt(0).Id);
            Assert.Equal(service.TableClient.QueryAsync_Return.AsPages_Result!.ElementAt(1).Id,
                entities.ElementAt(1).Id);
        }

        [Fact]
        public async Task ReadByPatientIdAndNoteId_WhenCalled_CallsTableGetEntityAsync()
        {
            var patientId = Guid.NewGuid();
            var noteId = Guid.NewGuid();
            var service = Service();

            var entity = await service.Read(patientId, noteId);

            Assert.Equal(PatientNoteTableEntity.CreatePartitionKey(patientId),
                service.TableClient.GetEntityAsync_PartitionKey);
            Assert.Equal(PatientNoteTableEntity.CreateRowKey(noteId),
                service.TableClient.GetEntityAsync_RowKey);
            Assert.Equal(service.TableClient.GetEntityAsync_Return!.Id, entity!.Id);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task Update_WhenCalled_CallsTableClientGetEntityAsync(string text)
        {
            var entity = PatientNoteEntity(text);
            var service = Service();
            var partitionKey = PatientNoteTableEntity.CreatePartitionKey(entity.PatientId);
            var rowKey = PatientNoteTableEntity.CreateRowKey(entity.Id);

            await service.Update(entity);

            Assert.Equal(partitionKey, service.TableClient.GetEntityAsync_PartitionKey);
            Assert.Equal(rowKey, service.TableClient.GetEntityAsync_RowKey);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public async Task Update_WhenCalled_CallsTableClientUpdateEntityAsync(string text)
        {
            var entity = PatientNoteEntity(text);
            var service = Service();
            service.TableClient.GetEntityAsync_Return = new PatientNoteTableEntity(entity);

            await service.Update(entity);

            Assert.Equal(service.TableClient.GetEntityAsync_Return!.PartitionKey,
                service.TableClient.UpdateEntityAsync_ParamEntity!.PartitionKey);
            Assert.Equal(service.TableClient.GetEntityAsync_Return!.RowKey,
                service.TableClient.UpdateEntityAsync_ParamEntity!.RowKey);
            Assert.Equal(service.TableClient.GetEntityAsync_Return!.Timestamp,
                service.TableClient.UpdateEntityAsync_ParamEntity!.Timestamp);
            Assert.Equal(service.TableClient.GetEntityAsync_Return!.ETag,
                service.TableClient.UpdateEntityAsync_ParamEntity!.ETag);
            Assert.Equal(service.TableClient.GetEntityAsync_Return!.PatientId,
                service.TableClient.UpdateEntityAsync_ParamEntity!.PatientId);
            Assert.Equal(service.TableClient.GetEntityAsync_Return!.Id,
                service.TableClient.UpdateEntityAsync_ParamEntity!.Id);
            Assert.Equal(service.TableClient.GetEntityAsync_Return!.Text,
                service.TableClient.UpdateEntityAsync_ParamEntity!.Text);
            Assert.Equal(service.TableClient.GetEntityAsync_Return.ETag,
                service.TableClient.UpdateEntityAsync_ParamIfMatch);
        }

        static PatientNoteData PatientNoteData(string text) => new()
        {
            PatientId = Guid.NewGuid(),
            Text = text,
        };
        static PatientNoteEntity PatientNoteEntity(string text) => new()
        {
            Id = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            Text = text,
        };
        static TestAzureTableStoragePatientNotesService Service(MockTableClient? tableClient = null) =>
            new(tableClient ?? new MockTableClient());
        class TestAzureTableStoragePatientNotesService : AzureTableStoragePatientNotesService
        {
            public MockTableClient TableClient { get; }

            public TestAzureTableStoragePatientNotesService(MockTableClient tableClient)
                : base("connection-string")
            {
                TableClient = tableClient;
            }

            protected override Task<TableClient> CreateTableClient() =>
                Task.FromResult<TableClient>(TableClient);
        }
    }
}