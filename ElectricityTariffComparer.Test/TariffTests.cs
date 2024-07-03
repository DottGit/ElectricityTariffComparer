using NUnit.Framework;
using NUnit.Framework.Legacy;
using static ElectricityTariffComparer.Models.TariffProvider;

namespace ElectricityTariffComparer.Test
{
    [TestFixture]
    public class TariffTests
    {
        [TestCase(3500, 830)]
        [TestCase(4500, 1050)]
        public void BasicElectricityTariff_CalculateAnnualCost_ReturnsCorrectCost(
            int consumption, 
            decimal expectedCost)
        {
            var tariff = new BasicElectricityTariff
            {
                BaseCostPerMonth = 5,
                AdditionalKwhCost = 22
            };

            var annualCost = tariff.CalculateAnnualCost(consumption);

            ClassicAssert.AreEqual(expectedCost, annualCost);
        }

        [TestCase(3500, 800)]
        [TestCase(4500, 950)]
        public void PackagedTariff_CalculateAnnualCost_ReturnsCorrectCost(
            int consumption, 
            decimal expectedCost)
        {
            var tariff = new PackagedTariff
            {
                IncludedKwh = 4000,
                BaseCost = 800,
                AdditionalKwhCost = 30
            };

            var annualCost = tariff.CalculateAnnualCost(consumption);

            ClassicAssert.AreEqual(expectedCost, annualCost);
        }
    }
}