using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class CalculatorFactory : ICalculatorFactory
    {
        private readonly Dictionary<IncentiveType, IRebateCalculator> calculatorDictionary;

        public CalculatorFactory()
        {
            calculatorDictionary = new Dictionary<IncentiveType, IRebateCalculator>
            {
                { IncentiveType.FixedRateRebate, new FixedRateCalculator() },
                { IncentiveType.AmountPerUom, new AmountPerUOMCalculator() },
                { IncentiveType.FixedCashAmount, new FixedCashCalculator() },
            };
        }

        public IRebateCalculator GetCalculator(IncentiveType incentiveType)
        {
            if (calculatorDictionary.TryGetValue(incentiveType, out var rebateCalculator))
            {
                return rebateCalculator;
            }

            return null;
        }
    }
}
