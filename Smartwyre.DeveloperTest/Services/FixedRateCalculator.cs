using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class FixedRateCalculator : IRebateCalculator
    {
        public CalculateRebateResult ValidateAndCalculateRebate(Product product, Rebate rebate, decimal volume)
        {
            {
                if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
                {
                    return new CalculateRebateResult { Success = false, Reason = "Product does not support rebate type" };
                }

                if (rebate.Amount == 0 || rebate.Percentage == 0 || volume == 0)
                {
                    return new CalculateRebateResult { Success = false, Reason = "Invalid values for fixed rate rebate" };
                }

                return new CalculateRebateResult { Amount = rebate.Amount * rebate.Percentage * volume, Success = true };
            }
        }
    }
}