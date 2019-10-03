using System.ComponentModel.DataAnnotations.Schema;
using WeeklyNotification.App.DAL.Contracts;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("FiatDepositInvestmentOrder")]
    public class FiatDepositInvestmentOrder : InvestmentOrderBase, IDepositInvestmentOrder
    {
        public int DepositProductId { get; set; }

        public DepositProduct DepositProduct { get; set; }
        public int? TransactionId { get; set; }
        public string TransactionTimestamp { get; set; }
    }
}