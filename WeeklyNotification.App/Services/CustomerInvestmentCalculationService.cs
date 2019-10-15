using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeklyNotification.App.DAL.Contracts;
using WeeklyNotification.App.DAL.Entities;
using WeeklyNotification.App.Models;

namespace WeeklyNotification.App.Services
{
    public interface ICustomerInvestmentCalculationService

    {
        Task<List<CustomerInvestmentInfo>> GetInvestmentInfos(IEnumerable<InvestmentOrderModel> orders);
    }

    public class CustomerInvestmentCalculationCalculationService : ICustomerInvestmentCalculationService
    {
        private readonly IRepository<CryptoExchangeRate> _rateRepository;
        private readonly IRepository<Currency> _currencyRepository;


        public CustomerInvestmentCalculationCalculationService(IRepository<CryptoExchangeRate> rateRepository,
            IRepository<Currency> currencyRepository)
        {
            _rateRepository = rateRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CustomerInvestmentInfo>> GetInvestmentInfos(IEnumerable<InvestmentOrderModel> orders)
        {
            var rates = await _rateRepository.GetAll().ToListAsync();
            var currencies = await _currencyRepository.GetAll().ToListAsync();
            List<CustomerInvestmentInfo> investmentInfos = new List<CustomerInvestmentInfo>();
            var todayDate = DateTime.UtcNow;
            foreach (var customerInvestments in orders.GroupBy(g => g.Customer))
            {
                decimal interestEarnedForCustomerInGBP = 0;

                decimal npvForCustomerInGBP = 0;

                foreach (var investmentsByProduct in customerInvestments.GroupBy(ci => ci.ProductId))
                {
                    // Total deposits 
                    decimal depositOrdersNPVSummary = 0;
                    // Total withdraws
                    decimal withdrawalOrdersNPVSummary = 0;
                    // Total deposits NPV
                    decimal interestEarned = 0;

                    int currencyId = investmentsByProduct.First().CurrencyId;

                    foreach (var investment in investmentsByProduct)
                    {
                        decimal daysInvested = GetInvestmentDays(investment.CreatedUtc, todayDate);

                        decimal daysCoefficient = daysInvested / 365;

                        decimal rateCoefficient = 1 + investment.InterestRate;

                        double investmentCoefficient = Math.Pow(
                            x: (double) rateCoefficient,
                            y: (double) daysCoefficient);

                        decimal amountWithInterest = investment.Amount * (decimal) investmentCoefficient;

                        if (investment.IsDeposit)
                        {
                            depositOrdersNPVSummary += amountWithInterest;
                            interestEarned += amountWithInterest - investment.Amount;
                        }
                        else
                        {
                            withdrawalOrdersNPVSummary += amountWithInterest;
                            interestEarned -= amountWithInterest - investment.Amount;
                        }
                    }

                    interestEarnedForCustomerInGBP += ConvertToGBP(interestEarned, currencyId, true);
                    npvForCustomerInGBP += ConvertToGBP(depositOrdersNPVSummary - withdrawalOrdersNPVSummary,
                        currencyId, false);
                    
                }


                if (npvForCustomerInGBP > 0)
                {
                    investmentInfos.Add(new CustomerInvestmentInfo
                    {
                        Customer = customerInvestments.Key,
                        AmountNPV = npvForCustomerInGBP,
                        InterestEarned = interestEarnedForCustomerInGBP
                    });
                }
            }

            return investmentInfos;

            decimal ConvertToGBP(decimal value, int currencyId, bool isInterest)
            {
                var gbpExchangeRate = rates.FirstOrDefault(r => r.FromCurrencyId == currencyId);
                int decimalPlaces = 6;
                if (!isInterest)
                {
                    var currency = currencies.FirstOrDefault(c => c.Id == currencyId);
                    if (currency == null)
                    {
                        throw new Exception("Currency does not exist");
                    }

                    decimalPlaces = currency.DecimalPlaces;
                }


                if (gbpExchangeRate == null) // currency is GBP
                {
                    return decimal.Round(value, decimalPlaces);
                }

                return Math.Floor(decimal.Round(value, decimalPlaces) * gbpExchangeRate.Rate * 100) / 100;
            }
        }

        private static int GetInvestmentDays(DateTime firstDay, DateTime lastDay)
        {
            var duration = (lastDay.Date - firstDay.Date).Days - 1;
            return Math.Max(duration, 0);
        }
    }
}