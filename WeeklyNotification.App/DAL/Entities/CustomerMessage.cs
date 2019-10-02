using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("CustomerMessage")]
    public class CustomerMessage
    {
        public int Id { get; set; }
        public int Customer { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title { get; set; }
        public decimal? Amount { get; set; }
        public string Type { get; set; }
        public int? TopUp { get; set; }
        public int? Transfer { get; set; }
        public int? Currency { get; set; }
        public int? TransferToContact { get; set; }
        public int? RequestMoney { get; set; }
        public bool? DoNotNotify { get; set; }
        public int? AdminMessage { get; set; }
        public string Message { get; set; }
        public int? ZeuxSpendingCategoryId { get; set; }
        public int? OpenBankingTransaction { get; set; }
        public int? CryptoTransactionId { get; set; }
        public int? TransactionId { get; set; }
        public int? FiatDepositInvestmentOrder { get; set; }
        public int? FiatFundInvestmentOrder { get; set; }
    }
}