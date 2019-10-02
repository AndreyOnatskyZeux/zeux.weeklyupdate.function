using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyNotification.App.DAL.Entities
{
    public abstract class InvestmentOrderBase
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeposit { get; set; }
        public string Status { get; set; }
        public DateTime CreatedUtc { get; set; }
        public int CustomerId { get; set; }
        public decimal InterestRate { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        
    }
}