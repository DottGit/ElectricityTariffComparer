namespace ElectricityTariffComparer.Models
{
    public class TariffProvider
    {
        public abstract class Tariff
        {
            public string Name { get; set; }
            public int Type { get; set; }
            public abstract decimal CalculateAnnualCost(int consumption);
        }

        public class BasicElectricityTariff : Tariff
        {
            public decimal BaseCostPerMonth { get; set; }
            public decimal AdditionalKwhCost { get; set; }
            public object BaseCost { get; internal set; }

            public override decimal CalculateAnnualCost(int consumption)
            {
                return (BaseCostPerMonth * 12) + (consumption * AdditionalKwhCost / 100);
            }
        }
        public class PackagedTariff : Tariff
        {
            public int IncludedKwh { get; set; }
            public decimal BaseCost { get; set; }
            public decimal AdditionalKwhCost { get; set; }

            public override decimal CalculateAnnualCost(int consumption)
            {
                if (consumption <= IncludedKwh)
                {
                    return BaseCost;
                }
                else
                {
                    return BaseCost + ((consumption - IncludedKwh) * AdditionalKwhCost / 100);
                }
            }
        }

        //Mocked Tariffs
        public List<Tariff> GetTariffs()
        {
            return new List<Tariff>
        {
            new BasicElectricityTariff
            {
                Name = "Product A",
                BaseCostPerMonth = 5,
                AdditionalKwhCost = 22
            },
            new PackagedTariff
            {
                Name = "Product B",
                IncludedKwh = 4000,
                BaseCost = 800,
                AdditionalKwhCost = 30
            }
        };
        }
    }
}
