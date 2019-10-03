using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeklyNotification.App.DAL.Contracts;
using WeeklyNotification.App.Models;

namespace WeeklyNotification.App.Services
{
    public interface IDepositInvestmentOrderService<T>
    {
        Task<IEnumerable<InvestmentOrderModel>> GetInvestmentOrderModels();
    }
    
    public class DepositInvestmentOrderService<T> : IDepositInvestmentOrderService<T> where T : class, IDepositInvestmentOrder  
    {
        private readonly IRepository<T> _orderRepository;
        

        public DepositInvestmentOrderService(IRepository<T> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<InvestmentOrderModel>> GetInvestmentOrderModels()
        {
            var orders = await _orderRepository.GetAll()
                .Include(o => o.DepositProduct)
                .Include(o => o.Customer)
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
                CurrencyId = o.DepositProduct.Currency
            });
        }
        
    }
}