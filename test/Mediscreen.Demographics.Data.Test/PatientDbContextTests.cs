using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Mediscreen.Data
{
    public class PatientDbContextTests
    {
        [Fact]
        public void InheritsDbContext()
        {
            Assert.True(typeof(DbContext).IsAssignableFrom(typeof(PatientDbContext)));
        }
    }
}