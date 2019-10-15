using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyNotification.App.DAL.Entities
{
    [Table("Currency")]
    public class Currency
    {
        public int Id { get; set; }
        public int DecimalPlaces { get; set; }
    }
}