using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeklyNotification.App.DAL.Contracts;
using WeeklyNotification.App.Models;

namespace WeeklyNotification.App.Services
{
    public interface IFundInvestmentOrderService<T>
    {
        Task<IEnumerable<InvestmentOrderModel>> GetInvestmentOrderModels();
    }
    
    public class FundInvestmentOrderService<T>: IFundInvestmentOrderService<T> where T : class, IFundInvestmentOrder
    {
        private readonly IRepository<T> _orderRepository;
        

        public FundInvestmentOrderService(IRepository<T> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<InvestmentOrderModel>> GetInvestmentOrderModels()
        {
            var orders = await _orderRepository.GetAll()
                .Include(o => o.FundProduct)
                .Include(o => o.Customer)
                .Where(o => o.Status == "Complete")
                .ToListAsync();
            return orders.Select(o => new InvestmentOrderModel()
            {
                Amount = o.Amount,
                Customer = o.Customer,
                Id = o.Id,
                Status = o.Status,
                CreatedUtc = o.CreatedUtc,
                CustomerId = o.CustomerId,
                InterestRate = o.InterestRate,
                IsDeposit = o.IsDeposit,
                CurrencyId = o.FundProduct.Currency
            });
        }
        
    }
}