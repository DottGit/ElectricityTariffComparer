using AutoMapper;
using ElectricityTariffComparer.Controllers;
using ElectricityTariffComparer.Mapper;
using ElectricityTariffComparer.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElectricityTariffComparer.Models.Dto;

namespace ElectricityTariffComparer.Test
{
    [TestFixture]
    public class TariffControllerTests
    {
        private IMapper _mapper;
        private TariffComparerController _controller;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
            _controller = new TariffComparerController(_mapper);
        }

        [Test]
        public void CalculateAnnualCosts_ValidInput_ReturnsExpectedResults()
        {
            // Arrange
            var tariffs = new List<TariffDto>
            {
                new TariffDto { Name = "Product A", Type = 1, BaseCost = 5, AdditionalKwhCost = 22 },
                new TariffDto { Name = "Product B", Type = 2, IncludedKwh = 4000, BaseCost = 800, AdditionalKwhCost = 30 }
            };
            int consumption = 4500;

            // Act
            var result = _controller.CalculateAnnualCosts(tariffs, consumption) as OkObjectResult;
            var results = result.Value as List<TariffResult>;

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            ClassicAssert.AreEqual(2, results.Count);

            dynamic firstResult = results[0];
            dynamic secondResult = results[1];

            ClassicAssert.AreEqual("Product B", firstResult.TariffName);
            ClassicAssert.AreEqual(950m, firstResult.AnnualCost);
            ClassicAssert.AreEqual("Product A", secondResult.TariffName);
            ClassicAssert.AreEqual(990m, secondResult.AnnualCost);
        }

        [Test]
        public void CalculateAnnualCosts_InvalidTariffType_ReturnsBadRequest()
        {
            // Arrange
            var tariffs = new List<TariffDto>
            {
                new TariffDto { Name = "Invalid Tariff", Type = 3, BaseCost = 10, AdditionalKwhCost = 20 }
            };
            int consumption = 5000;

            // Act
            var result = _controller.CalculateAnnualCosts(tariffs, consumption);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.AreEqual("Unknown tariff type: 3", badRequestResult.Value);
        }
    }
}
