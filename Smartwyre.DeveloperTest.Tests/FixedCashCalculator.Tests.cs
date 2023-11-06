using System;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class FixedCashCalculatorTests
    {
        private readonly FixedCashCalculator _fixedCashCalculator;
        public FixedCashCalculatorTests()
        {
            _fixedCashCalculator = new FixedCashCalculator();
            // Reuse for all tests?
        }

        [Fact]
        public void ValidateAndCalculateRebate_UnsupportedRebateType_ReturnsFail()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
            };

            var result = _fixedCashCalculator.ValidateAndCalculateRebate(product, new Rebate(), 10m);

            Assert.False(result.Success);
            Assert.Equal("Product does not support rebate type", result.Reason);
        }

        [Fact]
        public void ValidateAndCalculateRebate_ZeroAmount_ReturnsFail()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
            };

            var rebate = new Rebate
            {
                Amount = 0m,
            };

            var result = _fixedCashCalculator.ValidateAndCalculateRebate(product, rebate, 100m);

            Assert.False(result.Success);
            Assert.Equal("Cannot have a fixed cash rebate with amount 0", result.Reason);
        }

        [Fact]
        public void ValidateAndCalculateRebate_ValidRebate_ReturnsCorrectAmount()
        {
            var product = new Product
            {
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
            };

            var rebate = new Rebate
            {
                Amount = 10m,
            };

            var result = _fixedCashCalculator.ValidateAndCalculateRebate(product, rebate, 100m);

            Assert.True(result.Success);
            Assert.Equal(10m, result.Amount);
        }
    }
}