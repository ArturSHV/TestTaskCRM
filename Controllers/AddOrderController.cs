using Microsoft.AspNetCore.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class AddOrderController : Controller
    {
        public AddOrderPageModel model { get; set; }

        public AddOrderController([FromServices] DataContext dataContext)
        {
            IFactory viewOrderFactory = new AddOrderFactory();

            IPageModel pageModel = viewOrderFactory.Create();

            model = pageModel.InitialData(dataContext) as AddOrderPageModel;
        }

        public IActionResult Index()
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult Index([FromServices] DataContext dataContext, string number, string providerName)
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
