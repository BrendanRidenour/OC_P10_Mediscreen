using Xunit;

namespace Mediscreen
{
    public class DiabetesRiskLevelTests
    {
        [Fact]
        public void NoneIsIntZero()
        {
            Assert.Equal(0, (int)DiabetesRiskLevel.None);
        }

        [Fact]
        public void BorderlineIsIntOne()
        {
            Assert.Equal(1, (int)DiabetesRiskLevel.Borderline);
        }

        [Fact]
        public void InDangerIsIntTwo()
        {
            Assert.Equal(2, (int)DiabetesRiskLevel.InDanger);
        }

        [Fact]
        public void EarlyOnsetIsIntThree()
        {
            Assert.Equal(3, (int)DiabetesRiskLevel.EarlyOnset);
        }
    }
}