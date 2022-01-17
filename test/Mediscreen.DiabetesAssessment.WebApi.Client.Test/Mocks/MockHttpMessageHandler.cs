using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mediscreen.Data.Mocks
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage? SendAsync_ParamRequest;
        public HttpResponseMessage SendAsync_Return = new(HttpStatusCode.OK);
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SendAsync_ParamRequest = request;

            return Task.FromResult(SendAsync_Return);
        }
    }
}