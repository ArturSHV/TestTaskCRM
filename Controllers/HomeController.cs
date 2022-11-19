using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Diagnostics;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        public HomePageModel model { get; set; }

        public HomeController([FromServices] DataContext dataContext)
		{
            IFactory homeFactory = new HomeFactory();

            IPageModel pageModel = homeFactory.Create();

            model = pageModel.InitialData(dataContext) as HomePageModel;
        }

        public IActionResult Index()
        {
            model.date1 = $"{DateTime.Now.AddMonths(-1).Year}-{DateTime.Now.AddMonths(-1).Month}-{DateTime.Now.AddMonths(-1).Day}";
            model.date2 = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(OrdersDataGet ordersDataGet) 
        {
            try
            {
                FilterDate(ordersDataGet.date);
                FilterDate1(ordersDataGet.date1);
                FilterDate2(ordersDataGet.date2);
                FilterNumber(ordersDataGet.number);
                FilterProviderId(ordersDataGet.providerId);
                ReturnDateFilter(ordersDataGet.date1, ordersDataGet.date2);

                return View(model);
            }
            catch
            {
                return Redirect("Error/Ошибка");
            }
		}

        /// <summary>
        /// Начальные данные фильтра периода
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        private void ReturnDateFilter(DateTime date1, DateTime date2)
        {
            model.date1 = $"{date1.Year}-{date1.Month}-{date1.Day}";
            model.date2 = $"{date2.Year}-{date2.Month}-{date2.Day}";
        }

        /// <summary>
        /// Фильтрация по дате
        /// </summary>
        /// <param name="date"></param>
        private void FilterDate(List<DateTime> date)
        {
            if (date != null)
            {
                var modelHelper = new HomePageModel();
                foreach (var item in date)
                {
                    var a = model.orders.Where(x => x.Date.ToShortDateString() == item.ToShortDateString()).ToList();
                    modelHelper.orders = modelHelper.orders.Union(a).ToList();
                }
                model.orders = modelHelper.orders;
            }
        }

        /// <summary>
        /// Фильтр периода с
        /// </summary>
        /// <param name="date"></param>
        private void FilterDate1(DateTime date)
        {
            if (date != DateTime.MinValue)
                model.orders = model.orders
                    .Where(x => Convert.ToDateTime(x.Date.ToShortDateString()) 
                    >= Convert.ToDateTime(date.ToShortDateString())).ToList();
        }

        /// <summary>
        /// Фильтр периода до
        /// </summary>
        /// <param name="date"></param>
        private void FilterDate2(DateTime date)
        {
            if (date != DateTime.MinValue)
                model.orders = model.orders
                    .Where(x => Convert.ToDateTime(x.Date.ToShortDateString()) 
                    <= Convert.ToDateTime(date.ToShortDateString())).ToList();
        }

        /// <summary>
        /// Фильтрация по номеру заказа
        /// </summary>
        /// <param name="number"></param>
        private void FilterNumber(List<string> number)
        {
            if (number != null)
            {
                var modelHelper = new HomePageModel();
                foreach (var item in number)
                {
                    var a = model.orders.Where(x => x.Number == item).ToList(); 
                    modelHelper.orders = modelHelper.orders.Union(a).ToList();
                }
                model.orders = modelHelper.orders;
            }
        }

        /// <summary>
        /// Фильтр по ид провайдера
        /// </summary>
        /// <param name="providerId"></param>
        private void FilterProviderId(List<int> providerId)
        {
            if (providerId != null)
            {
                var modelHelper = new HomePageModel();
                foreach (var item in providerId)
                {
                    var a = model.orders.Where(x => x.ProviderId == item).ToList();
                    modelHelper.orders = modelHelper.orders.Union(a).ToList();
                }
                model.orders = modelHelper.orders;
            }
        }

    }
}