using System;
using System.ComponentModel.DataAnnotations.Schema;
using WeeklyNotification.App.DAL.Contracts;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("FiatFundInvestmentOrder")]
    public class FiatFundInvestmentOrder : InvestmentOrderBase, IFundInvestmentOrder
    {
        public int? TransactionId { get; set; }
        public decimal CommissionRate { get; set; }
        public string TransactionTimestamp { get; set; }
        public int FundProductId { get; set; }
        public FundProduct FundProduct { get; set; }
    }
}