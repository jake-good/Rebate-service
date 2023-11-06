using System;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class AmountPerUOMCalculatorTests
    {
        private readonly AmountPerUOMCalculator _amountPerUOMCalculator;
        public AmountPerUOMCalculatorTests()
        {
            _amountPerUOMCalculator = new AmountPerUOMCalculator();
            // Reuse for all tests?
        }

        [Fact]
        public void ValidateAndCalculateRebate_UnsupportedRebateType_ReturnsFail()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
            };

            var result = _amountPerUOMCalculator.ValidateAndCalculateRebate(product, new Rebate(), 10m);

            Assert.False(result.Success);
            Assert.Equal("Product does not support rebate type", result.Reason);
        }

        [Fact]
        public void ValidateAndCalculateRebate_ZeroAmount_ReturnsFail()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.AmountPerUom,
            };

            var rebate = new Rebate
            {
                Amount = 0m,
            };

            var result = _amountPerUOMCalculator.ValidateAndCalculateRebate(product, rebate, 0m);

            Assert.False(result.Success);
            Assert.Equal("Amount per UOM rebate cannot have zero amount or volume", result.Reason);
        }

        [Fact]
        public void ValidateAndCalculateRebate_ValidRebate_ReturnsCorrectAmount()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.AmountPerUom,
            };

            var rebate = new Rebate
            {
                Amount = 10m,
            };

            var result = _amountPerUOMCalculator.ValidateAndCalculateRebate(product, rebate, 100m);

            Assert.True(result.Success);
            Assert.Equal(1000m, result.Amount);
        }
    }
}