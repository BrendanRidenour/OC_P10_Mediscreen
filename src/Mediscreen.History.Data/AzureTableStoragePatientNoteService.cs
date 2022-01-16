using Azure;
using Azure.Data.Tables;

namespace Mediscreen.Data
{
    public class AzureTableStoragePatientNoteService : IPatientNoteService
    {
        readonly string _connectionString;

        public AzureTableStoragePatientNoteService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or whitespace.", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        bool tableExists = false;
        protected virtual async Task<TableClient> CreateTableClient()
        {
            var client = new TableClient(_connectionString, tableName: "PatientNotes");

            if (!tableExists)
            {
                await client.CreateIfNotExistsAsync();
                tableExists = true;
            }

            return client;
        }

        public async Task<PatientNoteEntity> Create(PatientNoteData note)
        {
            var entity = new PatientNoteTableEntity(noteId: Guid.NewGuid(), note);

            var tables = await CreateTableClient();

            await tables.AddEntityAsync(entity);

            return new PatientNoteEntity(entity);
        }

        public async Task<IEnumerable<PatientNoteEntity>> Read(Guid patientId)
        {
            try
            {
                var partitionKey = PatientNoteTableEntity.CreatePartitionKey(patientId);

                var tables = await CreateTableClient();

                var results = tables.QueryAsync<PatientNoteTableEntity>(
                    filter: $"PartitionKey eq '{partitionKey}'");

                var notes = new List<PatientNoteEntity>();

                await foreach (var page in results.AsPages())
                    foreach (var entity in page.Values)
                        if (entity is not null)
                            notes.Add(new PatientNoteEntity(entity));

                return notes;
            }
            catch (RequestFailedException exception)
            when (exception.ErrorCode == "ResourceNotFound")
            {
                return null!;
            }
        }

        public async Task<PatientNoteEntity?> Read(Guid patientId, Guid noteId)
        {
            try
            {
                var entity = await ReadEntity(patientId, noteId);

                return new PatientNoteEntity(entity);
            }
            catch (RequestFailedException exception)
            when (exception.ErrorCode == "ResourceNotFound")
            {
                return null;
            }
        }

        protected async Task<PatientNoteTableEntity> ReadEntity(Guid patientId, Guid noteId)
        {
            var partitionKey = PatientNoteTableEntity.CreatePartitionKey(patientId);
            var rowKey = PatientNoteTableEntity.CreateRowKey(noteId);

            var tables = await CreateTableClient();

            var entity = await tables.GetEntityAsync<PatientNoteTableEntity>(partitionKey, rowKey);

            return entity;
        }

        public async Task Update(PatientNoteEntity note)
        {
            var existingNote = await ReadEntity(note.PatientId, note.Id);

            if (existingNote is null)
                throw new InvalidOperationException($"Cannot update note because none exist with the note id '{note.Id}' and patient id '{note.PatientId}'.");

            var updateEntity = new PatientNoteTableEntity(note)
            {
                Timestamp = existingNote.Timestamp,
                ETag = existingNote.ETag,
            };

            var tables = await CreateTableClient();

            await tables.UpdateEntityAsync(updateEntity, existingNote.ETag, TableUpdateMode.Replace);
        }
    }
}