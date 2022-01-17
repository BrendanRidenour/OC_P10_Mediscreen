using Microsoft.Extensions.Internal;
using System;

namespace Mediscreen.Mocks
{
    public class MockSystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow { get; }

        public MockSystemClock(DateTimeOffset utcNow)
        {
            UtcNow = utcNow;
        }
    }
}