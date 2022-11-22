using Microsoft.AspNetCore.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class AddOrderController : Controller
    {
        public AddOrderPageModel model { get; set; }
        private DataContext dataContext { get; set; }
        private AddOrderPageModel modelHelper { get; set; }

        public AddOrderController([FromServices] DataContext dataContext)
        {
            this.dataContext = dataContext;

            modelHelper = new AddOrderPageModel(dataContext);

            IFactory viewOrderFactory = new AddOrderFactory(dataContext);

            IPageModel pageModel = viewOrderFactory.Create();

            model = pageModel.pageModel as AddOrderPageModel;
        }

        public IActionResult Index()
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string number, string providerName)
        {
            var providerId = dataContext.Provider.FirstOrDefault(x=>x.Name==providerName);
            if (providerId != null)
            {
                var order = dataContext.Order.FirstOrDefault(x => (x.Number == number) && (x.ProviderId == providerId.Id));

                if (order == null)
                {
                    var newOrder = new Order
                    {
                        Date = DateTime.Now,
                        Number = number,
                        ProviderId = providerId.Id
                    };

                    dataContext.Order.Add(newOrder);
                    dataContext.SaveChanges();

                    var orderId = dataContext.Order.FirstOrDefault(x => (x.Number == number) && (x.ProviderId == providerId.Id))?.Id;

                    return Redirect($"ViewOrder/{orderId}");
                }
                else
                    return Redirect("Error/Ошибка предметной области");
            }
            
            return View(model);
        }
    }
}
