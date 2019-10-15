using System;
using WeeklyNotification.App.DAL.Entities;

namespace WeeklyNotification.App.Models
{
    public class InvestmentOrderModel
    {
       public int Id { get; set; }
       public  decimal Amount { get; set; }
       public bool IsDeposit { get; set; }
       public string Status { get; set; }
       public DateTime CreatedUtc { get; set; }
       public int CustomerId { get; set; }
       public decimal InterestRate { get; set; }
       public Customer Customer { get; set; }
       public int CurrencyId { get; set; }
       public string ProductId { get; set; }
    }
}