using System;
using System.Net.Http;

namespace Mediscreen.Data.Mocks
{
    public class MockHttpClient : HttpClient
    {
        public MockHttpMessageHandler Handler { get; }

        public MockHttpClient(MockHttpMessageHandler handler)
            : base(handler)
        {
            BaseAddress = new Uri("https://example.com");
            Handler = handler;
        }

        public HttpRequestMessage SendAsync_ParamRequest
        {
            get => Handler.SendAsync_ParamRequest!;
            set => Handler.SendAsync_ParamRequest = value;
        }

        public HttpResponseMessage SendAsync_Return
        {
            get => Handler.SendAsync_Return!;
            set => Handler.SendAsync_Return = value;
        }
    }
}