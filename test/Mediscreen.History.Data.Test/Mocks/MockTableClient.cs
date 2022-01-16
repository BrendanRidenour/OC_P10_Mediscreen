using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mediscreen.Data.Mocks
{
    public class MockTableClient : TableClient
    {
        public bool CreateIfNotExistsAsync_Called = false;
        public override Task<Response<TableItem>> CreateIfNotExistsAsync(CancellationToken cancellationToken = default)
        {
            CreateIfNotExistsAsync_Called = true;

            return Task.FromResult<Response<TableItem>>(null!);
        }

        public PatientNoteTableEntity? AddEntityAsync_ParamEntity;
        public override Task<Response> AddEntityAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            AddEntityAsync_ParamEntity = entity as PatientNoteTableEntity;

            return Task.FromResult<Response>(null!);
        }

        public string? QueryAsync_ParamFilter;
        public MockAsyncPageable QueryAsync_Return = new();
        public bool QueryAsync_ThrowResourceNotFound = false;
        public override AsyncPageable<T> QueryAsync<T>(string filter, int? maxPerPage = null, IEnumerable<string> select = null!, CancellationToken cancellationToken = default)
        {
            QueryAsync_ParamFilter = filter;

            if (QueryAsync_ThrowResourceNotFound)
                throw new RequestFailedException(404, "Not Found", errorCode: "ResourceNotFound", innerException: null);

            return (QueryAsync_Return as AsyncPageable<T>)!;
        }

        public string? GetEntityAsync_PartitionKey;
        public string? GetEntityAsync_RowKey;
        public bool GetEntityAsync_ThrowResourceNotFound = false;
        public PatientNoteTableEntity? GetEntityAsync_Return = new(new PatientNoteEntity(
            noteId: Guid.NewGuid(),
            note: new PatientNoteData()
            {
                PatientId = Guid.NewGuid(),
                Text = "note text",
            }))
        {
            Timestamp = DateTimeOffset.UtcNow,
            ETag = new ETag("123"),
        };
        public override Task<Response<T>> GetEntityAsync<T>(string partitionKey, string rowKey, IEnumerable<string> select = null!, CancellationToken cancellationToken = default)
        {
            GetEntityAsync_PartitionKey = partitionKey;
            GetEntityAsync_RowKey = rowKey;

            if (GetEntityAsync_ThrowResourceNotFound)
                throw new RequestFailedException(404, "Not Found", errorCode: "ResourceNotFound", innerException: null);

            return Task.FromResult((new MockResponse(GetEntityAsync_Return!) as Response<T>)!);
        }

        public PatientNoteTableEntity? UpdateEntityAsync_ParamEntity;
        public ETag? UpdateEntityAsync_ParamIfMatch;
        public TableUpdateMode? UpdateEntityAsync_ParamMode;
        public override Task<Response> UpdateEntityAsync<T>(T entity, ETag ifMatch, TableUpdateMode mode = TableUpdateMode.Merge, CancellationToken cancellationToken = default)
        {
            UpdateEntityAsync_ParamEntity = entity as PatientNoteTableEntity;
            UpdateEntityAsync_ParamIfMatch = ifMatch;
            UpdateEntityAsync_ParamMode = mode;

            return Task.FromResult(new MockResponse((entity as PatientNoteTableEntity)!).GetRawResponse());
        }
    }
}