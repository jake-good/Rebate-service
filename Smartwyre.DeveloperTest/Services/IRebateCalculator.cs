
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public interface IRebateCalculator
    {
        public CalculateRebateResult ValidateAndCalculateRebate(Product product, Rebate rebate, decimal volume);
    }
}