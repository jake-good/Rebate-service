using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    readonly IProductDataStore _productDataStore;
    readonly IRebateDataStore _rebateDataStore;
    private readonly ICalculatorFactory _calculatorFactory;

    public RebateService(IProductDataStore productDataStore, IRebateDataStore rebateDataStore, ICalculatorFactory calculatorFactory)
    {
        _productDataStore = productDataStore;
        _rebateDataStore = rebateDataStore;
        _calculatorFactory = calculatorFactory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        if (rebate == null)
        {
            return new CalculateRebateResult
            {
                Success = false,
                Reason = "No Rebate found for identifier"
            };
        }

        Product product = _productDataStore.GetProduct(request.ProductIdentifier);
        if (product == null)
        {
            return new CalculateRebateResult
            {
                Success = false,
                Reason = "No Product found for identifier"
            };
        }

        var rebateCalculator = _calculatorFactory.GetCalculator(rebate.Incentive);
        if (rebateCalculator == null)
        {
            return new CalculateRebateResult
            {
                Success = false,
                Reason = $"No calulator implemented for Incentive type {rebate.Incentive}",
            };
        }

        var result = rebateCalculator.ValidateAndCalculateRebate(product, rebate, request.Volume);
        if (result.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, result.Amount);
        }

        return result;
    }
}

// todo ~ incentive types -> supportedincentivetypes?
// ~ amount is used for two different things?? ~ should type be diff
// ~ write CLI 

