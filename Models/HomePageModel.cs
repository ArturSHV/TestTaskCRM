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
        public IPageModel pageModel { get; set; }
        private DataContext dataContext { get; set; }


        public HomePageModel(DataContext dataContext)
        {
            this.dataContext = dataContext;
            
        }

        public void InitialData()
        {
            OrdersDataCreate();
            DataForFilterCreate();
            pageModel = new HomePageModel(dataContext) { dataForFilter = dataForFilter, orders = orders };
        }

        private void DataForFilterCreate()
        {
            dataForFilter = new DataForFilter()
            {
                date = dataContext.Order.Select(x => x.Date.ToShortDateString()).ToList().DistinctBy(x => x).ToList(),
                number = dataContext.Order.Select(x => x.Number).ToList().DistinctBy(x => x).ToList(),
                providerId = dataContext.Order.Select(x => x.ProviderId).ToList().DistinctBy(x => x).ToList(),
            };

        }

        private void OrdersDataCreate()
        {
            orders = dataContext.Order.ToList();
        }

    }
}
