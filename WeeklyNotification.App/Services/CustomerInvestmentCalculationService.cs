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


        public CustomerInvestmentCalculationCalculationService(IRepository<CryptoExchangeRate> rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task<List<CustomerInvestmentInfo>> GetInvestmentInfos(IEnumerable<InvestmentOrderModel> orders)
        {
            var rates = await _rateRepository.GetAll().ToListAsync();
            List<CustomerInvestmentInfo> investmentInfos = new List<CustomerInvestmentInfo>();
            var todayDate = DateTime.UtcNow;

            foreach (var customerInvestments in orders.GroupBy(g => g.Customer))
            {
                // Total deposits 
                decimal depositOrdersNPVSummary = 0;
                // Total withdraws
                decimal withdrawalOrdersNPVSummary = 0;
                // Total deposits NPV
                decimal interestEarned = 0;


                foreach (var investment in customerInvestments)
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
                        depositOrdersNPVSummary += ConvertToGBP(amountWithInterest, investment.CurrencyId);
                        interestEarned += ConvertToGBP(amountWithInterest - investment.Amount, investment.CurrencyId);
                    }
                    else
                    {
                        withdrawalOrdersNPVSummary += ConvertToGBP(amountWithInterest, investment.CurrencyId);
                        interestEarned -= ConvertToGBP(amountWithInterest - investment.Amount, investment.CurrencyId);
                    }
                }


                if (depositOrdersNPVSummary - withdrawalOrdersNPVSummary > 0)
                {
                    investmentInfos.Add(new CustomerInvestmentInfo
                    {
                        Customer = customerInvestments.Key,
                        AmountNPV = depositOrdersNPVSummary - withdrawalOrdersNPVSummary,
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

        private static int GetInvestmentDays(DateTime firstDay, DateTime lastDay)
        {
            var duration = (lastDay.Date - firstDay.Date).Days - 1;
            return Math.Max(duration, 0);
        }
    }
}