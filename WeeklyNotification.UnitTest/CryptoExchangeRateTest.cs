using WeeklyNotification.App.Services;
using CryptoExchangeRate.UnitTest.Logger;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CryptoExchangeRate.UnitTest
{
    public class CryptoExchangeRateTest
    {
//        [Fact]
//        public void RecalculateRate_Not_Call_For_Empty_CurrencyList()
//        {
//            // Arrange
//            var logger = (ListLogger)TestHelper.CreateLogger(LoggerTypes.List);
//            var coinbaseUtil = new Mock<WeeklyNotification.App.Contracts.ICoinbaseUtil>();
//            coinbaseUtil.Setup(x => x.GetCurrencyRate(It.IsAny<string>())).Returns(It.IsAny<decimal>);
//
//            var cryptoConverstionUtil = new Mock<WeeklyNotification.App.Contracts.ICryptoConverstionUtil>();
//            cryptoConverstionUtil.Setup(x => x.GetCurrencyRate(It.IsAny<string>())).Returns(It.IsAny<decimal>);
//
//	        var factoryUtil = new Mock<WeeklyNotification.App.Contracts.IFactoryUtil>();
//	        factoryUtil.Setup(x => x.GetCryptoConverstionUtil(It.IsAny<string>())).Returns(cryptoConverstionUtil.Object);
//
//			var currencyRepo = new Mock<WeeklyNotification.App.DAL.Contracts.ICurrencyRepo>();
//            currencyRepo.Setup(x => x.GetCurrencyByName(It.IsAny<string>())).Returns(new WeeklyNotification.App.DAL.Entities.Currency());
//            // Get empty list
//            currencyRepo.Setup(x => x.GetCurrenciesByType(It.IsAny<int>())).Returns(new List<WeeklyNotification.App.DAL.Entities.Currency>());
//
//            var cryptoExchangeRateRepo = new Mock<WeeklyNotification.App.DAL.Contracts.ICryptoExchangeRateRepo>();
//            cryptoExchangeRateRepo.Setup(x => x.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(It.IsAny<bool>());
//            cryptoExchangeRateRepo.Setup(x => x.UpdateRate(It.IsAny<int>(), It.IsAny<decimal>())).Returns(It.IsAny<bool>());
//            cryptoExchangeRateRepo.Setup(x => x.GetByCurrencyIds(It.IsAny<int>(), It.IsAny<int>())).Returns(new WeeklyNotification.App.DAL.Entities.CryptoExchangeRate());
//
//            // Act
//            var exchageService = new ExchangeService(coinbaseUtil.Object, factoryUtil.Object, currencyRepo.Object, cryptoExchangeRateRepo.Object);
//            exchageService.InitLog(logger);
//            exchageService.RecalculateRate();
//
//            // Assert
//            currencyRepo.Verify(x => x.GetCurrencyByName(It.IsAny<string>()), Times.Once);
//            currencyRepo.Verify(x => x.GetCurrenciesByType(It.IsAny<int>()), Times.Once);
//
//            coinbaseUtil.Verify(x => x.GetCurrencyRate(It.IsAny<string>()), Times.Never);
//            cryptoConverstionUtil.Verify(x => x.GetCurrencyRate(It.IsAny<string>()), Times.Never);
//            cryptoExchangeRateRepo.Verify(x => x.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
//            cryptoExchangeRateRepo.Verify(x => x.UpdateRate(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
//            cryptoExchangeRateRepo.Verify(x => x.GetByCurrencyIds(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
//        }
//
//        [Fact]
//        public void RecalculateRate_Call_With_Create_New_Data()
//        {
//            // Arrange
//            var logger = (ListLogger)TestHelper.CreateLogger(LoggerTypes.List);
//            var coinbaseUtil = new Mock<WeeklyNotification.App.Contracts.ICoinbaseUtil>();
//            coinbaseUtil.Setup(x => x.GetCurrencyRate(It.IsAny<string>())).Returns(2);
//
//            var cryptoConverstionUtil = new Mock<WeeklyNotification.App.Contracts.ICryptoConverstionUtil>();
//            cryptoConverstionUtil.Setup(x => x.GetCurrencyRate(It.IsAny<string>())).Returns(1);
//
//			var factoryUtil = new Mock<WeeklyNotification.App.Contracts.IFactoryUtil>();
//	        factoryUtil.Setup(x => x.GetCryptoConverstionUtil(It.IsAny<string>())).Returns(cryptoConverstionUtil.Object);
//
//			var currencyRepo = new Mock<WeeklyNotification.App.DAL.Contracts.ICurrencyRepo>();
//            currencyRepo.Setup(x => x.GetCurrencyByName(It.IsAny<string>())).Returns(new WeeklyNotification.App.DAL.Entities.Currency());
//            currencyRepo.Setup(x => x.GetCurrenciesByType(It.IsAny<int>()))
//                .Returns(new[]
//                {
//                    new WeeklyNotification.App.DAL.Entities.Currency{ Name="BTC" },
//                    new WeeklyNotification.App.DAL.Entities.Currency{ Name="TestCoin" }
//                });
//
//	        var expectedRates = new Dictionary<string, decimal>();
//			expectedRates.Add("BTC", 2 * 0.998m);
//	        expectedRates.Add("TestCoin", 2 * 1 * 0.998m);
//
//			var cryptoExchangeRateRepo = new Mock<WeeklyNotification.App.DAL.Contracts.ICryptoExchangeRateRepo>();
//            cryptoExchangeRateRepo.Setup(x => x.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(It.IsAny<bool>());
//            cryptoExchangeRateRepo.Setup(x => x.UpdateRate(It.IsAny<int>(), It.IsAny<decimal>())).Returns(It.IsAny<bool>());
//            cryptoExchangeRateRepo.Setup(x => x.GetByCurrencyIds(It.IsAny<int>(), It.IsAny<int>())).Returns((WeeklyNotification.App.DAL.Entities.CryptoExchangeRate)null);
//
//            // Act
//            var exchageService = new ExchangeService(coinbaseUtil.Object, factoryUtil.Object, currencyRepo.Object, cryptoExchangeRateRepo.Object);
//            exchageService.InitLog(logger);
//	        var actualRates = exchageService.RecalculateRate();
//
//            // Assert
//
//	        foreach (var actual in actualRates)
//	        {
//		        var expectedRate = expectedRates[actual.Key];
//				Assert.Equal(expectedRate, actual.Value);
//	        }
//
//            currencyRepo.Verify(x => x.GetCurrencyByName(It.IsAny<string>()), Times.Once);
//            currencyRepo.Verify(x => x.GetCurrenciesByType(It.IsAny<int>()), Times.Once);
//            coinbaseUtil.Verify(x => x.GetCurrencyRate(It.IsAny<string>()), Times.AtLeastOnce);
//            cryptoConverstionUtil.Verify(x => x.GetCurrencyRate(It.IsAny<string>()), Times.Once);
//            cryptoExchangeRateRepo.Verify(x => x.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.AtLeastOnce);            
//            cryptoExchangeRateRepo.Verify(x => x.GetByCurrencyIds(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
//
//            cryptoExchangeRateRepo.Verify(x => x.UpdateRate(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
//        }
//
//        [Fact]
//        public void RecalculateRate_Call_With_Update_Rate_Only()
//        {
//            // Arrange
//            var logger = (ListLogger)TestHelper.CreateLogger(LoggerTypes.List);
//            var coinbaseUtil = new Mock<WeeklyNotification.App.Contracts.ICoinbaseUtil>();
//            coinbaseUtil.Setup(x => x.GetCurrencyRate(It.IsAny<string>())).Returns(It.IsAny<decimal>);
//
//            var cryptoConverstionUtil = new Mock<WeeklyNotification.App.Contracts.ICryptoConverstionUtil>();
//            cryptoConverstionUtil.Setup(x => x.GetCurrencyRate(It.IsAny<string>())).Returns(It.IsAny<decimal>);
//
//	        var factoryUtil = new Mock<WeeklyNotification.App.Contracts.IFactoryUtil>();
//	        factoryUtil.Setup(x => x.GetCryptoConverstionUtil(It.IsAny<string>())).Returns(cryptoConverstionUtil.Object);
//
//			var currencyRepo = new Mock<WeeklyNotification.App.DAL.Contracts.ICurrencyRepo>();
//            currencyRepo.Setup(x => x.GetCurrencyByName(It.IsAny<string>())).Returns(new WeeklyNotification.App.DAL.Entities.Currency());
//            currencyRepo.Setup(x => x.GetCurrenciesByType(It.IsAny<int>()))
//                .Returns(new[]
//                {
//                    new WeeklyNotification.App.DAL.Entities.Currency{ Name="BTC" },
//                    new WeeklyNotification.App.DAL.Entities.Currency{ Name="TestCoin" },
//					new WeeklyNotification.App.DAL.Entities.Currency{ Name = "BCHSV"}
//                });
//
//            var cryptoExchangeRateRepo = new Mock<WeeklyNotification.App.DAL.Contracts.ICryptoExchangeRateRepo>();
//            cryptoExchangeRateRepo.Setup(x => x.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(It.IsAny<bool>());
//            cryptoExchangeRateRepo.Setup(x => x.UpdateRate(It.IsAny<int>(), It.IsAny<decimal>())).Returns(It.IsAny<bool>());
//            cryptoExchangeRateRepo.Setup(x => x.GetByCurrencyIds(It.IsAny<int>(), It.IsAny<int>())).Returns(new WeeklyNotification.App.DAL.Entities.CryptoExchangeRate());
//
//            // Act
//            var exchageService = new ExchangeService(coinbaseUtil.Object, factoryUtil.Object, currencyRepo.Object, cryptoExchangeRateRepo.Object);
//            exchageService.InitLog(logger);
//            exchageService.RecalculateRate();
//
//            // Assert
//            currencyRepo.Verify(x => x.GetCurrencyByName(It.IsAny<string>()), Times.Once);
//            currencyRepo.Verify(x => x.GetCurrenciesByType(It.IsAny<int>()), Times.Once);
//            coinbaseUtil.Verify(x => x.GetCurrencyRate(It.IsAny<string>()), Times.AtLeastOnce);
//            cryptoConverstionUtil.Verify(x => x.GetCurrencyRate(It.IsAny<string>()), Times.AtLeastOnce);
//            cryptoExchangeRateRepo.Verify(x => x.GetByCurrencyIds(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
//            cryptoExchangeRateRepo.Verify(x => x.UpdateRate(It.IsAny<int>(), It.IsAny<decimal>()), Times.Exactly(3));
//
//            cryptoExchangeRateRepo.Verify(x => x.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);

//        }

    }
}
