using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("Customer")]
    public class Customer
    {
        public int Id { get; set; }
        public string DeviceType { get; set; }
        public string DeviceToken { get; set; }
    }
}