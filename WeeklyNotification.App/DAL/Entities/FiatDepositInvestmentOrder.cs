using System;
using System.ComponentModel.DataAnnotations.Schema;
using WeeklyNotification.App.DAL.Contracts;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("FiatDepositInvestmentOrder")]
    public class FiatDepositInvestmentOrder : InvestmentOrderBase, IInvestmentOrder
    {
        public int DepositProductId { get; set; }
        public int? TransactionId { get; set; }
        public string TransactionTimestamp { get; set; }
    }
}