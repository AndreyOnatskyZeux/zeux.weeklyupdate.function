using System;
using System.ComponentModel.DataAnnotations.Schema;
using WeeklyNotification.App.DAL.Contracts;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("CryptoFundInvestmentOrder")]
    public class CryptoFundInvestmentOrder: InvestmentOrderBase, IFundInvestmentOrder
    {
        public int FundProductId { get; set; }
        public int? CryptoTransactionId { get; set; }
        public decimal CommissionRate { get; set; }
        public FundProduct FundProduct { get; set; }
    }
}