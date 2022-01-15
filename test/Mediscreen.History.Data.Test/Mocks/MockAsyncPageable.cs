using Azure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediscreen.Data.Mocks
{
    public class MockAsyncPageable : AsyncPageable<PatientNoteTableEntity>
    {
        public IReadOnlyList<PatientNoteTableEntity>? AsPages_Result = new List<PatientNoteTableEntity>()
        {
            new PatientNoteTableEntity(new PatientNoteEntity(
                noteId: Guid.NewGuid(),
                note: new PatientNoteData()
                {
                    PatientId = Guid.NewGuid(),
                    Text = "note text",
                })),
            new PatientNoteTableEntity(new PatientNoteEntity(
                noteId: Guid.NewGuid(),
                note: new PatientNoteData()
                {
                    PatientId = Guid.NewGuid(),
                    Text = "note text",
                })),
        };
        public override async IAsyncEnumerable<Page<PatientNoteTableEntity>> AsPages(string? continuationToken = null, int? pageSizeHint = null)
        {
            await Task.CompletedTask;

            yield return new MockPage(AsPages_Result!);
        }

        class MockPage : Page<PatientNoteTableEntity>
        {
            public override IReadOnlyList<PatientNoteTableEntity> Values { get; }

            public MockPage(IReadOnlyList<PatientNoteTableEntity> values)
            {
                Values = values;
            }

            public override string? ContinuationToken => null;

            public override Response GetRawResponse() => null!;
        }
    }
}