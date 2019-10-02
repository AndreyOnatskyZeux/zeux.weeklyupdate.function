using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace WeeklyNotification.App.Contracts
{
    public interface IExchangeService
    {
        void InitLog(ILogger log);
	    IDictionary<string, decimal> RecalculateRate();
    }
}
