using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;
using Moq;
using Smartwyre.DeveloperTest.Data;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private readonly RebateService _rebateService;
    private readonly Mock<IProductDataStore> _productDataStoreMock;
    private readonly Mock<IRebateDataStore> _rebateDataStoreMock;
    private readonly Mock<ICalculatorFactory> _calculatorFactoryMock;
    public RebateServiceTests()
    {
        _productDataStoreMock = new Mock<IProductDataStore>();
        _rebateDataStoreMock = new Mock<IRebateDataStore>();
        _calculatorFactoryMock = new Mock<ICalculatorFactory>();
        _rebateService = new RebateService(_productDataStoreMock.Object, _rebateDataStoreMock.Object, _calculatorFactoryMock.Object);
    }

    [Fact]
    public void Calculate_NoProductFound_DoesSomething()
    {
        _rebateDataStoreMock.Setup((m) => m.GetRebate(It.IsAny<string>())).Returns(new Rebate());
        _productDataStoreMock.Setup((m) => m.GetProduct(It.IsAny<string>())).Returns((Product)null);

        var result = _rebateService.Calculate(new CalculateRebateRequest { ProductIdentifier = "", });

        Assert.False(result.Success);
        Assert.Equal("No Product found for identifier", result.Reason);
        _calculatorFactoryMock.Verify((m) => m.GetCalculator(It.IsAny<IncentiveType>()), Times.Never);
        _rebateDataStoreMock.Verify((e) => e.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_NoRebateFound_ReturnsFailure()
    {
        _rebateDataStoreMock.Setup(d => d.GetRebate(It.IsAny<string>())).Returns((Rebate)null);

        var result = _rebateService.Calculate(new CalculateRebateRequest { RebateIdentifier = "", });

        Assert.False(result.Success);
        Assert.Equal("No Rebate found for identifier", result.Reason);
        _productDataStoreMock.Verify((e) => e.GetProduct(It.IsAny<string>()), Times.Never);
        _calculatorFactoryMock.Verify((e) => e.GetCalculator(It.IsAny<IncentiveType>()), Times.Never);
        _rebateDataStoreMock.Verify((e) => e.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void Calculate_SuccessfulRequest_StoresResult()
    {
        var rebate = new Rebate
        {
            Identifier = "rebate",
            Incentive = IncentiveType.FixedRateRebate
        };

        var product = new Product
        {
            Identifier = "product",
        };

        var expectedResult = new CalculateRebateResult
        {
            Success = true,
            Amount = 10m
        };

        var rebateRequest = new CalculateRebateRequest
        {
            ProductIdentifier = product.Identifier,
            RebateIdentifier = rebate.Identifier,
            Volume = 10m,
        };

        var calculator = new Mock<IRebateCalculator>();
        calculator.Setup(d => d.ValidateAndCalculateRebate(product, rebate, rebateRequest.Volume)).Returns(expectedResult).Verifiable();
        _productDataStoreMock.Setup(d => d.GetProduct(product.Identifier)).Returns(product).Verifiable();
        _rebateDataStoreMock.Setup(d => d.GetRebate(rebate.Identifier)).Returns(rebate).Verifiable();
        _calculatorFactoryMock.Setup(d => d.GetCalculator(rebate.Incentive)).Returns(calculator.Object).Verifiable();

        var actualResult = _rebateService.Calculate(rebateRequest);

        Mock.Verify();
        _rebateDataStoreMock.Verify((e) => e.StoreCalculationResult(rebate, expectedResult.Amount), Times.Once);
        Assert.Equal(actualResult, expectedResult);
    }

    [Fact]
    public void Calculate_BadRequest_ReturnsFailure()
    {
        var result = new CalculateRebateResult
        {
            Success = false,
            Reason = "test reason"
        };

        var calculator = new Mock<IRebateCalculator>();
        calculator
            .Setup((m) => m.ValidateAndCalculateRebate(It.IsAny<Product>(), It.IsAny<Rebate>(), It.IsAny<decimal>()))
            .Returns(result);

        _productDataStoreMock.Setup((m) => m.GetProduct(It.IsAny<string>())).Returns(new Product());
        _rebateDataStoreMock.Setup((m) => m.GetRebate(It.IsAny<string>())).Returns(new Rebate());
        _calculatorFactoryMock.Setup((m) => m.GetCalculator(It.IsAny<IncentiveType>())).Returns(calculator.Object);

        var actualResult = _rebateService.Calculate(new CalculateRebateRequest());
        Assert.Equal(result, actualResult);
        _rebateDataStoreMock.Verify((m) => m.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }
}
