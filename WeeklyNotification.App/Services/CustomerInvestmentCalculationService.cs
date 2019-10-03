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
            List<CustomerInvestmentInfo> customerInterests = new List<CustomerInvestmentInfo>();

            foreach (var customerInvestments in orders.GroupBy(g => g.Customer))
            {
                // Total deposits 
                decimal depositOrdersSummary = 0;
                // Total withdraws
                decimal withdrawalOrdersSummary = 0;
                // Total deposits NPV
                decimal depositsNPV = 0;
                // Total withdraws NPV
                decimal withdrawsNPV = 0;

                foreach (var investment in customerInvestments)
                {
                    int investmentDuration = (DateTime.UtcNow - investment.CreatedUtc).Days;

                    if (investment.IsDeposit)
                    {
                        depositOrdersSummary += ConvertToGBP(investment.Amount, investment.CurrencyId);
                        depositsNPV += ConvertToGBP(GetNPV(
                            investment.Amount,
                            (double)investment.InterestRate,
                            investmentDuration),investment.CurrencyId);
                    }
                    else
                    {
                        withdrawalOrdersSummary += ConvertToGBP(investment.Amount, investment.CurrencyId);
                        withdrawsNPV += ConvertToGBP(GetNPV(
                            investment.Amount,
                            (double)investment.InterestRate,
                            investmentDuration), investment.CurrencyId);
                    }
                }


                if (depositsNPV - withdrawsNPV >= 0)
                {
                    customerInterests.Add(new CustomerInvestmentInfo
                    {
                        Customer = customerInvestments.Key,
                        Amount = depositOrdersSummary - withdrawalOrdersSummary,
                        InterestEarned = Math.Round(depositsNPV - withdrawsNPV -
                                                    (depositOrdersSummary - withdrawalOrdersSummary), 6)
                    });
                }
            }

            return customerInterests;

            decimal ConvertToGBP(decimal value, int currencyId)
            {
                var gbpExchangeRate = rates.FirstOrDefault(r => r.FromCurrencyId == currencyId);
                if (gbpExchangeRate == null)
                {
                    gbpExchangeRate = new CryptoExchangeRate() {Rate = 1};
                }

                return value * (decimal) gbpExchangeRate.Rate;
            }
        }

        private static decimal GetNPV(decimal amount, double interestRate, double investmentDuration)
        {
            return amount * (decimal)Math.Pow(1 + interestRate, (investmentDuration / 365));
        }
    }
}