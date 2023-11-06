using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class CalculatorFactoryTests
    {
        private readonly CalculatorFactory _calculatorFactory;

        public CalculatorFactoryTests()
        {
            _calculatorFactory = new CalculatorFactory();
        }

        [Fact]
        public void GetCalculator_ValidType_ReturnsCorrectObject()
        {
            var result = _calculatorFactory.GetCalculator(IncentiveType.FixedCashAmount);
            Assert.IsType<FixedCashCalculator>(result);
        }

        [Fact]
        public void GetCalculator_RebateTypeNotValid_ReturnsNull()
        {
            var result = _calculatorFactory.GetCalculator((IncentiveType)100);
            Assert.Null(result);
        }
    }
}