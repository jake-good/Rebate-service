using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class FixedCashCalculator : IRebateCalculator
    {
        public CalculateRebateResult ValidateAndCalculateRebate(Product product, Rebate rebate, decimal volume)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
            {
                return new CalculateRebateResult { Success = false, Reason = "Product does not support rebate type" };
            }

            if (rebate.Amount == 0)
            {
                return new CalculateRebateResult { Success = false, Reason = "Cannot have a fixed cash rebate with amount 0" };
            }

            return new CalculateRebateResult { Amount = rebate.Amount, Success = true };
        }
    }
}