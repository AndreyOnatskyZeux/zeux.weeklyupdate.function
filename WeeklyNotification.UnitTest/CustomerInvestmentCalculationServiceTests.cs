﻿using System;
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
            //converting
            decimal Convert(decimal value, decimal? rate)
            {
                if (!rate.HasValue)
                {
                    return value;
                }

                return Math.Floor(rate.Value * value * 100) / 100;
            }

            // Arrange 
            var investmentDuration = 10;
            var customerId = 1;
            var interestRate = 0.06M;
            var amount = 100.0;
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
                    CreatedUtc = DateTime.UtcNow.AddDays(-investmentDuration - 1),
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
            decimal interestEarned = (decimal) Math.Round((npv - amount), 6);

            var ratesRepository = new Mock<IRepository<WeeklyNotification.App.DAL.Entities.CryptoExchangeRate>>();
            var currencyRepository = new Mock<IRepository<Currency>>();

            var expected = new CustomerInvestmentInfo()
            {
                AmountNPV = Convert((decimal) npv, 2),
                Customer = customer,
                InterestEarned = Convert((decimal) interestEarned, 2)
            };

            var ratesMock = new List<WeeklyNotification.App.DAL.Entities.CryptoExchangeRate>()
            {
                new WeeklyNotification.App.DAL.Entities.CryptoExchangeRate()
                {
                    FromCurrencyId = 1,
                    Rate = 2
                }
            }.AsQueryable().BuildMock();
            
            var currenciesMock = new List<Currency>()
            {
                new Currency()
                {
                    Id = 1,
                    DecimalPlaces = 2
                }
            }.AsQueryable().BuildMock();
            
            ratesRepository.Setup(r => r.GetAll()).Returns(ratesMock.Object);
            currencyRepository.Setup(r => r.GetAll()).Returns(currenciesMock.Object);
            var service = new CustomerInvestmentCalculationCalculationService(ratesRepository.Object,currencyRepository.Object);
            // Act 
            var result = await service.GetInvestmentInfos(orders);

            // Assert
            Assert.True(result.Any());
            Assert.Equal(expected.AmountNPV, result.First().AmountNPV);
            Assert.Equal(expected.InterestEarned, result.First().InterestEarned);
        }
    }
}