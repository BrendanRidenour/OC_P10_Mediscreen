using Azure;
using Azure.Core;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Mediscreen.Data.Mocks
{
    public class MockResponse : Response<PatientNoteTableEntity>
    {
        public override PatientNoteTableEntity Value { get; }

        public MockResponse(PatientNoteTableEntity value) => Value = value;

        public override Response GetRawResponse() => new BaseResponse();

        class BaseResponse : Response
        {
            public override int Status => throw new System.NotImplementedException();

            public override string ReasonPhrase => throw new System.NotImplementedException();

            public override Stream? ContentStream { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
            public override string ClientRequestId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

            public override void Dispose()
            {
                throw new System.NotImplementedException();
            }

            protected override bool ContainsHeader(string name)
            {
                throw new System.NotImplementedException();
            }

            protected override IEnumerable<HttpHeader> EnumerateHeaders()
            {
                throw new System.NotImplementedException();
            }

            protected override bool TryGetHeader(string name, [NotNullWhen(true)] out string? value)
            {
                throw new System.NotImplementedException();
            }

            protected override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}