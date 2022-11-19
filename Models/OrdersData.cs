namespace WebSite.Models
{
    public class OrdersData
    {
        public int orderId { get; set; }
        public string number { get; set; }
        public DateTime date { get; set; }
        public int providerId { get; set; }
        public string providerName { get; set; }
        public int orderItemId { get; set; }
        public string orderItemName { get; set; }
        public decimal quantity { get; set; }
        public string unit { get; set; }

    }
}
