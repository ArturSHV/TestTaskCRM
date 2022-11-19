using System.ComponentModel.DataAnnotations;

namespace WebSite.Models
{
    public class OrdersDataGet 
    {
        public int orderId { get; set; }

        public List<string> number { get; set; }

        [Required]
        public List<DateTime> date { get; set; }
        public DateTime date1 { get; set; }
        public DateTime date2 { get; set; }
        public List<int> providerId { get; set; }

        public List<string> providerName { get; set; }

        public List<string> orderItemName { get; set; }

        public List<string> unit { get; set; }

        public List<decimal> quantity { get; set; }
    }
}
