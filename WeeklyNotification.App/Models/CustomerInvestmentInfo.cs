using WeeklyNotification.App.DAL.Entities;

namespace WeeklyNotification.App.Models
{
    public class CustomerInvestmentInfo
    {
        public Customer Customer { get; set; }
        public decimal AmountNPV { get; set; }
        public decimal InterestEarned { get; set; }
    }
}