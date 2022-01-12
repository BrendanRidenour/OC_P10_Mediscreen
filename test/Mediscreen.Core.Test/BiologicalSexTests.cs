using Xunit;

namespace Mediscreen
{
    public class BiologicalSexTests
    {
        [Fact]
        public void Female_Returns0()
        {
            Assert.Equal(0, (int)BiologicalSex.Female);
        }

        [Fact]
        public void Male_Returns1()
        {
            Assert.Equal(1, (int)BiologicalSex.Male);
        }
    }
}