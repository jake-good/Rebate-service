using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class AmountPerUOMCalculator : IRebateCalculator
    {
        public CalculateRebateResult ValidateAndCalculateRebate(Product product, Rebate rebate, decimal volume)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
            {
                return new CalculateRebateResult { Success = false, Reason = "Product does not support rebate type" };
            }

            if (rebate.Amount == 0 || volume == 0) // Maybe separate these and have diff errors? or add vals to reason
            {
                return new CalculateRebateResult { Success = false, Reason = "Amount per UOM rebate cannot have zero amount or volume" };
            }

            return new CalculateRebateResult { Amount = rebate.Amount * volume, Success = true };
        }
    }
}