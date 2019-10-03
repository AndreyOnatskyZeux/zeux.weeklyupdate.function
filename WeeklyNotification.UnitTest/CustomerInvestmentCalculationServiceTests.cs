using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable.Moq;
using Moq;
using WeeklyNotification.App.DAL.Contracts;
using WeeklyNotification.App.DAL.Entities;
using WeeklyNotification.App.Models;
using WeeklyNotification.App.Services;
using Xunit;

namespace CryptoExchangeRate.UnitTest
{
    public class CustomerInvestmentCalculationServiceTests
    {
        [Fact]
        public async Task Calling_GetInvestmentInfos_returns_correct_Collection()
        {
            // Arrange 
            var investmentDuration = 10;
            var transactionId = 1;
            var customerId = 1;
            var interestRate = 0.06M;
            var amount = 100.0;
            var provider = "Wecashe";
            var customer = new Customer()
            {
                Id = customerId,
                DeviceToken = "token",
                DeviceType = "android"
            };
            
            var orders = new List<InvestmentOrderModel>
            {
                new InvestmentOrderModel()
                {
                    CreatedUtc = DateTime.UtcNow.AddDays(-investmentDuration),
                    Status = "Complete",
                    CustomerId = customerId,
                    Customer = customer,
                    Amount = (decimal) amount,
                    IsDeposit = true,
                    InterestRate = interestRate,
                    CurrencyId = 1
                }
            };

            double npv = amount * Math.Pow((1 + (double) interestRate), (double) investmentDuration / 365);
            decimal interestEarned = (decimal)Math.Round(2*(npv - amount), 6);

            var repository = new Mock<IRepository<WeeklyNotification.App.DAL.Entities.CryptoExchangeRate>>();

            var expected = new CustomerInvestmentInfo()
            {
                Amount = (decimal) (2 * amount),
                Customer = customer,
                InterestEarned = interestEarned
            };
           
            var mock = new List<WeeklyNotification.App.DAL.Entities.CryptoExchangeRate>()
            {
                new WeeklyNotification.App.DAL.Entities.CryptoExchangeRate()
                {
                    FromCurrencyId = 1,
                    Rate = 2
                }
            }.AsQueryable().BuildMock();
            repository.Setup(r => r.GetAll()).Returns(mock.Object);
            var service = new CustomerInvestmentCalculationCalculationService(repository.Object);
            // Act 
            var result = await service.GetInvestmentInfos(orders);

            // Assert
            Assert.True(result.Any());
            Assert.Equal(expected.Amount,result.First().Amount);
            Assert.Equal(expected.InterestEarned,result.First().InterestEarned);
        }
    }
}