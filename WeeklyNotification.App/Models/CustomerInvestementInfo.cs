using WeeklyNotification.App.DAL.Entities;

namespace WeeklyNotification.App.Models
{
    public class CustomerInvestementInfo
    {
        public Customer Customer { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestEarned { get; set; }
    }
}