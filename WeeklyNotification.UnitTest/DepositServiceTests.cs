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
    public class DepositServiceTests
    {
        [Fact]
        public async Task Calling_GetFiatDepositInvestments_returns_correct_CryptoDepositInvestmentCollection()
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
            
            var fakeItems = new List<FiatDepositInvestmentOrder>
            {
                new FiatDepositInvestmentOrder
                {
                    CreatedUtc = DateTime.UtcNow.AddDays(-investmentDuration),
                    Status = "Complete",
                    TransactionId = transactionId,
                    CustomerId = customerId,
                    Customer = customer,
                    Amount = (decimal) amount,
                    IsDeposit = true,
                    DepositProductId = 1,
                    InterestRate = interestRate
                }
            };

            double npv = amount * Math.Pow((1 + (double) interestRate), (double) investmentDuration / 365);
            decimal interestEarned = (decimal)Math.Round(npv - amount, 6);

            var repository = new Mock<IRepository<FiatDepositInvestmentOrder>>();

            var expected = new CustomerInvestmentInfo()
            {
                Amount = (decimal) amount,
                Customer = customer,
                InterestEarned = interestEarned
            };
            var mock = fakeItems.AsQueryable().BuildMock();
            repository.Setup(r => r.GetAll()).Returns(mock.Object);
            var service = new InvestmentOrderService<FiatDepositInvestmentOrder>(repository.Object);
            // Act 
            var result = await service.GetInvestmentInfos();

            // Assert
            Assert.True(result.Any());
            Assert.Equal(expected.Amount,result.First().Amount);
            Assert.Equal(expected.InterestEarned,result.First().InterestEarned);
        }
    }
}