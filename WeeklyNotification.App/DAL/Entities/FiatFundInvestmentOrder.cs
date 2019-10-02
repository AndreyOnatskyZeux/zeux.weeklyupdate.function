using System;
using System.ComponentModel.DataAnnotations.Schema;
using WeeklyNotification.App.DAL.Contracts;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("FiatFundInvestmentOrder")]
    public class FiatFundInvestmentOrder : InvestmentOrderBase, IInvestmentOrder
    {
        public int FundProductId { get; set; }
        public int? TransactionId { get; set; }
        public decimal CommissionRate { get; set; }
        public string TransactionTimestamp { get; set; }
    }
}