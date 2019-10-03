using System;
using System.Collections;
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


        public CustomerInvestmentCalculationCalculationService(IRepository<CryptoExchangeRate> rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task<List<CustomerInvestmentInfo>> GetInvestmentInfos(IEnumerable<InvestmentOrderModel> orders)
        {
            var rates = await _rateRepository.GetAll().ToListAsync();
            List<CustomerInvestmentInfo> investmentInfos = new List<CustomerInvestmentInfo>();

            foreach (var customerInvestments in orders.GroupBy(g => g.Customer))
            {
                // Total deposits 
                decimal depositOrdersSummary = 0;
                // Total withdraws
                decimal withdrawalOrdersSummary = 0;
                // Total deposits NPV
                decimal interestEarned = 0;


                foreach (var investment in customerInvestments)
                {
                    decimal daysInvested = (DateTime.UtcNow - investment.CreatedUtc).Days;

                    decimal daysCoefficient = daysInvested / 365;

                    decimal rateCoefficient = 1 + investment.InterestRate;

                    double investmentCoefficient = Math.Pow(
                        x: (double) rateCoefficient,
                        y: (double) daysCoefficient);

                    decimal amountWithInterest = investment.Amount * (decimal) investmentCoefficient;

                    if (investment.IsDeposit)
                    {
                        depositOrdersSummary += ConvertToGBP(amountWithInterest, investment.CurrencyId);
                        interestEarned += ConvertToGBP(amountWithInterest - investment.Amount, investment.CurrencyId);
                    }
                    else
                    {
                        withdrawalOrdersSummary += ConvertToGBP(amountWithInterest, investment.CurrencyId);
                        interestEarned -= ConvertToGBP(amountWithInterest - investment.Amount, investment.CurrencyId);
                    }
                }


                if (depositOrdersSummary - withdrawalOrdersSummary >= 0)
                {
                    investmentInfos.Add(new CustomerInvestmentInfo
                    {
                        Customer = customerInvestments.Key,
                        Amount = depositOrdersSummary - withdrawalOrdersSummary,
                        InterestEarned = interestEarned
                    });
                }
            }

            return investmentInfos;

            decimal ConvertToGBP(decimal value, int currencyId)
            {
                var gbpExchangeRate = rates.FirstOrDefault(r => r.FromCurrencyId == currencyId);
                if (gbpExchangeRate == null)
                {
                    return value;
                }
                return Math.Floor(value * gbpExchangeRate.Rate * 100) / 100;
            }
        }
    }
}