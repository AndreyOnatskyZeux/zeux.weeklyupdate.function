using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeklyNotification.App.DAL.Contracts;
using WeeklyNotification.App.Models;

namespace WeeklyNotification.App.Services
{
    public interface IInvestmentOrderService<T> where T : class, IInvestmentOrder
    {
        Task<List<CustomerInvestmentInfo>> GetInvestmentInfos();
    }

    public class InvestmentOrderService<T> : IInvestmentOrderService<T> where T : class, IInvestmentOrder
    {
        private readonly IRepository<T> _repository;

        public InvestmentOrderService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<List<CustomerInvestmentInfo>> GetInvestmentInfos()
        {
            var orders = await _repository.GetAll().Include(o => o.Customer).Where(x => x.Status == "Complete")
                .ToArrayAsync();
            List<CustomerInvestmentInfo> customerInterests = new List<CustomerInvestmentInfo>();

            foreach (var customerInvestments in orders.GroupBy(g => g.Customer))
            {
                // Total deposits 
                decimal depositOrdersSummary = 0;
                // Total withdraws
                decimal withdrawalOrdersSummary = 0;
                // Total deposits NPV
                double depositsNPV = 0;
                // Total withdraws NPV
                double withdrawsNPV = 0;

                foreach (var investment in customerInvestments)
                {
                    int investmentDuration = (DateTime.UtcNow - investment.CreatedUtc).Days;

                    if (investment.IsDeposit)
                    {
                        depositOrdersSummary += investment.Amount;
                        depositsNPV += GetNPV(
                            (double) investment.Amount,
                            (double) investment.InterestRate,
                            (double) investmentDuration);
                    }
                    else
                    {
                        withdrawalOrdersSummary += investment.Amount;
                        withdrawsNPV += GetNPV(
                            (double) investment.Amount,
                            (double) investment.InterestRate,
                            (double) investmentDuration);
                    }
                }

                if (depositsNPV - withdrawsNPV >= 0)
                {
                    customerInterests.Add(new CustomerInvestmentInfo
                    {
                        Customer = customerInvestments.Key,
                        Amount = depositOrdersSummary - withdrawalOrdersSummary,
                        InterestEarned = Math.Round(Convert.ToDecimal(depositsNPV - withdrawsNPV) -
                                         (depositOrdersSummary - withdrawalOrdersSummary),6)
                    });
                }
            }

            return customerInterests;
        }

        private static double GetNPV(double amount, double interestRate, double investmentDuration)
        {
            return amount * Math.Pow((1 + interestRate), investmentDuration / 365);
        }
    }
}