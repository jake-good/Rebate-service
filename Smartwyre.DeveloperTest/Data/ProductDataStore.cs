using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        Product product = productIdentifier switch
        {
            "fixed cash" => new Product { Id = 1, Identifier = "", SupportedIncentives = SupportedIncentiveType.FixedCashAmount },
            "fixed rate" => new Product { Id = 2, Identifier = "fixed rate", SupportedIncentives = SupportedIncentiveType.FixedRateRebate },
            _ => new Product { Id = 3, Identifier = "per uom", SupportedIncentives = SupportedIncentiveType.AmountPerUom }
        };

        return product;
    }
}
