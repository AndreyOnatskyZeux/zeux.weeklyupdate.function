using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("CryptoExchangeRate")]
    public class CryptoExchangeRate
    {
        public int Id { get; set; }
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public string Symbol { get; set; }
        public decimal Rate { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}