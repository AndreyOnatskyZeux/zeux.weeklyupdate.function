using System;

namespace WeeklyNotification.App.Contracts
{
    public interface ICryptoConverstionUtil
    {
		[Obsolete]
        decimal GetCurrencyRate(string currencyName);
	    decimal GetBidCurrencyRate(string currencyName, string baseCurrencyName);
	    decimal GetAskCurrencyRate(string currencyName, string baseCurrencyName);
	}
}
