using System.Collections;
using System.Linq;

namespace WebSite.Models
{
    public class HomePageModel : IPageModel
    {
        public List<Order> orders = new List<Order>();
        public DataForFilter dataForFilter = new DataForFilter();
        public string date1 { get; set; }
        public string date2 { get; set; }

        public IPageModel InitialData(DataContext dataContext)
        {

            OrdersDataCreate(dataContext);
            DataForFilterCreate(dataContext);
            return new HomePageModel { orders = orders, dataForFilter = dataForFilter };
        }

        private void DataForFilterCreate(DataContext dataContext)
        {
            dataForFilter = new DataForFilter()
            {
                date = dataContext.Order.Select(x => x.Date.ToShortDateString()).ToList().DistinctBy(x => x).ToList(),
                number = dataContext.Order.Select(x => x.Number).ToList().DistinctBy(x => x).ToList(),
                providerId = dataContext.Order.Select(x => x.ProviderId).ToList().DistinctBy(x => x).ToList(),
            };

        }

        private void OrdersDataCreate(DataContext dataContext)
        {
            orders = dataContext.Order.ToList();
        }

    }
}
