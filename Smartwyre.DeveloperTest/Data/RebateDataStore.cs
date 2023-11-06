using System.Xml.Linq;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    public Rebate GetRebate(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return rebateIdentifier switch
        {
            "fixed rate" => new Rebate { Identifier = "fixed rate", Incentive = IncentiveType.FixedRateRebate, Amount = 100m, Percentage = 0.1m },
            "fixed cash" => new Rebate { Identifier = "fixed cash", Incentive = IncentiveType.FixedCashAmount, Amount = 100m },
            _ => new Rebate { Identifier = "per uom", Incentive = IncentiveType.AmountPerUom, Amount = 10m }
        };
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
