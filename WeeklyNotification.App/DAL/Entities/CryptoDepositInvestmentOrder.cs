
using System.ComponentModel.DataAnnotations.Schema;
using WeeklyNotification.App.DAL.Contracts;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("CryptoDepositInvestmentOrder")]
    public class CryptoDepositInvestmentOrder : InvestmentOrderBase, IInvestmentOrder
    {
        public int DepositProductId { get; set; }
        public int? CryptoTransactionId { get; set; }
    }
}