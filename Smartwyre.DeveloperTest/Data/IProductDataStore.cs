using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    public interface IProductDataStore
    {
        public Product GetProduct(string productIdentifier);
    }
}