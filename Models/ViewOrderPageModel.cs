namespace WebSite.Models
{
    public class ViewOrderPageModel : IPageModel
    {
        public int orderId { get; set; }
        public string number { get; set; }
        public DateTime? date { get; set; }

        public List<Provider> providers = new List<Provider>();

        public List<Order> orders = new List<Order>();

        public List<OrderItem> orderItems = new List<OrderItem>();
        
        public DataForFilter dataForFilter = new DataForFilter();

        public List<OrdersData> ordersData = new List<OrdersData>();
        public IPageModel pageModel { get; set; }
        private DataContext dataContext { get; set; }


        public ViewOrderPageModel(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void InitialData()
        {
            orders = dataContext.Order.ToList();
            orderItems = dataContext.OrderItem.ToList();
            providers = dataContext.Provider.ToList();
            OrdersDataCreate();

            pageModel = new ViewOrderPageModel(dataContext) { orders = orders, orderItems = orderItems, 
                providers = providers, ordersData = ordersData };
        }

        private void OrdersDataCreate()
        {
            ordersData = orders.Join(
               orderItems,
               order => order.Id,
               orderItems => orderItems.OrderId,
               (order, orderItems) => new
               {
                   orderItems.OrderId,
                   order.Date,
                   order.Number,
                   order.ProviderId,
                   orderItems.Id,
                   orderItems.Unit,
                   orderItems.Quantity,
                   orderItems.Name
               }).Join(
               providers,
               orders => orders.ProviderId,
               providers => providers.Id,
               (orders, providers) => new OrdersData()
               {
                   orderId = orders.OrderId,
                   date = orders.Date,
                   number = orders.Number,
                   providerId = orders.ProviderId,
                   orderItemId = orders.Id,
                   unit = orders.Unit,
                   quantity = orders.Quantity,
                   orderItemName = orders.Name,
                   providerName = providers.Name
               }).ToList();
        }

    }
}
