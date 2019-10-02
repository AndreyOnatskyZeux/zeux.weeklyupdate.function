using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyNotification.App.Contracts
{
	public interface IFactoryUtil
	{
		ICryptoConverstionUtil GetCryptoConverstionUtil(string cryptoCurrency);
	}
}
