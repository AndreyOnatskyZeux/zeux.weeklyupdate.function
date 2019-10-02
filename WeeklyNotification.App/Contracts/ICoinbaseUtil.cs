
namespace WeeklyNotification.App.Contracts
{
    public interface ICoinbaseUtil
    {
        decimal GetCurrencyRate(string currencyName);
    }
}
