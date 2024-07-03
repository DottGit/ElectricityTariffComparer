using AutoMapper;
using ElectricityTariffComparer.Models;
using Microsoft.AspNetCore.Mvc;
using static ElectricityTariffComparer.Models.Dto;
using static ElectricityTariffComparer.Models.TariffProvider;

namespace ElectricityTariffComparer.Controllers
{
    public class TariffComparerController : ControllerBase
    {
        private readonly TariffProvider _tariffProvider;
        private readonly IMapper _mapper;

        public TariffComparerController(IMapper mapper)
        {
            _tariffProvider = new TariffProvider();
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTariffComparison([FromQuery] int consumption)
        {
            var tariffs = _tariffProvider.GetTariffs();
            var results = tariffs.Select(t => new
            {
                TariffName = t.Name,
                AnnualCost = t.CalculateAnnualCost(consumption)
            })
            .OrderBy(r => r.AnnualCost)
            .ToList();

            return Ok(results);
        }

        [HttpPost]
        public IActionResult CalculateAnnualCosts([FromBody] List<TariffDto> tariffs, [FromQuery] int consumption)
        {
            var parsedTariffs = new List<Tariff>();

            foreach (var tariffDto in tariffs)
            {
                try
                {
                    var tariff = ParseTariff(tariffDto);
                    parsedTariffs.Add(tariff);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var results = parsedTariffs.Select(t => new TariffResult
            {
                TariffName = t.Name,
                AnnualCost = t.CalculateAnnualCost(consumption)
            })
            .OrderBy(r => r.AnnualCost)
            .ToList();

            return Ok(results);
        }

        //FYI: its a bit overkill but you can manually parse them,
        //i just used automapper because iam used to it
        private Tariff ParseTariff(TariffDto tariffDto)
        {
            return tariffDto.Type switch
            {
                1 => _mapper.Map<BasicElectricityTariff>(tariffDto),
                2 => _mapper.Map<PackagedTariff>(tariffDto),
                _ => throw new ArgumentException($"Unknown tariff type: {tariffDto.Type}"),
            };
        }

    }
}
