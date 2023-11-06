using System;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class FixedRateCalculatorTests
    {
        private readonly FixedRateCalculator _fixedRateCalculator;
        public FixedRateCalculatorTests()
        {
            _fixedRateCalculator = new FixedRateCalculator();
            // Reuse for all tests?
        }

        [Fact]
        public void ValidateAndCalculateRebate_UnsupportedRebateType_ReturnsFail()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
            };

            var result = _fixedRateCalculator.ValidateAndCalculateRebate(product, new Rebate(), 10m);

            Assert.False(result.Success);
            Assert.Equal("Product does not support rebate type", result.Reason);
        }

        [Fact]
        public void ValidateAndCalculateRebate_ZeroPercentage_ReturnsFail()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
            };

            var rebate = new Rebate
            {
                Amount = 10m,
            };

            var result = _fixedRateCalculator.ValidateAndCalculateRebate(product, rebate, 10m);

            Assert.False(result.Success);
            Assert.Equal("Invalid values for fixed rate rebate", result.Reason);
        }

        [Fact]
        public void ValidateAndCalculateRebate_ValidRebate_ReturnsCorrectAmount()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
            };

            var rebate = new Rebate
            {
                Amount = 10m,
                Percentage = 0.5m
            };

            var result = _fixedRateCalculator.ValidateAndCalculateRebate(product, rebate, 100m);

            Assert.True(result.Success);
            Assert.Equal(500m, result.Amount);
        }
    }
}