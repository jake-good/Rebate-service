using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public interface ICalculatorFactory
    {
        public IRebateCalculator GetCalculator(IncentiveType incentiveType);
    }
}