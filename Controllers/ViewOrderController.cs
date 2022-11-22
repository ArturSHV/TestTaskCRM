using Microsoft.AspNetCore.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class ViewOrderController : Controller
    {
        public ViewOrderPageModel model { get; set; }
        private DataContext dataContext { get; set; }
        private ViewOrderPageModel modelHelper { get; set; }

        public ViewOrderController([FromServices] DataContext dataContext)
        {
            this.dataContext = dataContext;

            modelHelper = new ViewOrderPageModel(dataContext);

            IFactory viewOrderFactory = new ViewOrderFactory(dataContext);

            IPageModel pageModel = viewOrderFactory.Create();

            model = pageModel as ViewOrderPageModel;
        }

        [Route("{controller}/{id}")]
        public IActionResult Index(int id)
        {
            try
            {
                SelectItemsById(id);
                return View(model);
            }
            catch 
            {
                return Redirect("Error/Ошибка");
            }
        }

        /// <summary>
        /// Метод загрузки данных в форму заказа и в фильтр
        /// </summary>
        /// <param name="ordersDataGet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{controller}/{id}")]
        [HttpPost]
        public IActionResult Index(OrdersDataGet ordersDataGet, int id)
        {
            try
            {
                SelectItemsById(id);
                FilterNumber(ordersDataGet.number);
                FilterProviderId(ordersDataGet.providerId);
                FilterOrderitemName(ordersDataGet.orderItemName);
                FilterOrderitemUnit(ordersDataGet.unit);
                return View(model);
            }
            catch
            {
                return Redirect("Error/Ошибка");
            }
        }

        [HttpPost]
        [Route("{controller}/{action}")]
        public IActionResult AddOrderItem(OrdersData ordersData)
        {
            var orderItem = new OrderItem()
            {
                Name = ordersData.orderItemName,
                OrderId = ordersData.orderId,
                Quantity = ordersData.quantity,
                Unit = ordersData.unit
            };

            try
            {
                dataContext.OrderItem.Add(orderItem);
                dataContext.SaveChanges();

                return Redirect($"/ViewOrder/{ordersData.orderId}");
            }
            catch 
            {
                return Redirect("Error/Ошибка");
            }
            
        }

        /// <summary>
        /// Метод удаления заказа
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{controller}/{action}/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = dataContext.Order.FirstOrDefault(x=>x.Id==id);
            if (order != null)
            {
                try
                {
                    dataContext.Order.Remove(order);

                    var orderItem = dataContext.OrderItem.FirstOrDefault(x => x.OrderId == id);

                    if (orderItem != null)
                        dataContext.OrderItem.Remove(orderItem);

                    dataContext.SaveChanges();
                    return Redirect("/");
                }
                catch
                {
                    return Redirect("Error/Ошибка");
                }
            }
            else
                return Redirect("Error/Заказа не существует");
        }

        /// <summary>
        /// Удалить строку из заказа
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{controller}/{action}/{orderId}")]
        public IActionResult DeleteOrderItem(int orderId, int id)
        {
            if (id == 0) return Redirect($"/Error/Выберите строку");

            var req = dataContext.OrderItem.FirstOrDefault(x => x.Id == id);
            if (req != null)
            {
                try
                {
                    dataContext.OrderItem.Remove(req);
                    dataContext.SaveChanges();
                    return Redirect($"/ViewOrder/{orderId}");
                }
                catch 
                {
                    return Redirect("Error/Ошибка");
                }
            }
            
            return Redirect($"/Error/Данных в БД не существует");
        }

        /// <summary>
        /// Обработчик редактирования заказа
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="ordersDataGet"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{controller}/{action}")]
        public IActionResult EditOrderItems(OrdersDataGet ordersDataGet)
        {
            try
            {
                var message = SendEditData(ordersDataGet);
                if (message == "Успешно")
                {
                    dataContext.SaveChanges();
                    return Redirect($"/ViewOrder/{ordersDataGet.orderId}");
                }

                return Redirect($"/Error/{message}");
            }
            catch 
            {
                return Redirect("Error/Ошибка");
            }
        }

        /// <summary>
        /// Проверка наличия ордера в БД, если не существует id, то возвращает false
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private bool EditOrderTable(Order order)
        {
            var oldOrder = dataContext.Order.FirstOrDefault(x => x.Id == order.Id);
            if (oldOrder != null)
            {
                oldOrder.Date = order.Date;
                oldOrder.Number = order.Number;
                oldOrder.ProviderId = order.ProviderId;
                return true;
            }
            return false;
        }

        /// <summary>
        /// проверка наличия данных в таблице OrderItem. Если данных не найдено, то создаем item
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="orderItem"></param>
        /// <returns></returns>
        private void EditOrderItemTable(OrderItem orderItem)
        {
            var oldOrderItem = dataContext.OrderItem.FirstOrDefault(x => x.Id == orderItem.Id);
            if (oldOrderItem != null)
            {
                oldOrderItem.Name = orderItem.Name;
                oldOrderItem.Quantity = orderItem.Quantity;
                oldOrderItem.Unit = orderItem.Unit;
            }
            else
            {
                dataContext.OrderItem.Add(orderItem);
            }
        }

        /// <summary>
        /// Формирует сообщение успешной операции, либо ошибки
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="ordersDataGet"></param>
        /// <returns></returns>
        private string SendEditData(OrdersDataGet ordersDataGet)
        {
            int a = 0;
            for (int i = 0; i < ordersDataGet.orderItemName.Count; i++)
            {
                
                var newProviderId = ordersDataGet.providerId.First();

                Order order = new Order()
                {
                    Id = ordersDataGet.orderId,
                    Date = ordersDataGet.date.FirstOrDefault(),
                    Number = ordersDataGet.number?.FirstOrDefault(),
                    ProviderId = newProviderId
                };
                var isOrderTrue = EditOrderTable(order);

                var orderItemReq = dataContext.OrderItem.FirstOrDefault(x => x.Name == ordersDataGet.orderItemName[i]);
                OrderItem orderItem = new OrderItem()
                {
                    Id = (orderItemReq==null) ? 0 : orderItemReq.Id,
                    Name = ordersDataGet.orderItemName[i],
                    Quantity = ordersDataGet.quantity[i],
                    Unit = ordersDataGet.unit[i]
                };
                EditOrderItemTable(orderItem);

                var check = dataContext.Order.Where(x=>(x.Number== ordersDataGet.number.FirstOrDefault()) 
                && (x.ProviderId==newProviderId) && (x.Id != ordersDataGet.orderId)).ToList();

                var count = check.Count;

                if ((check.Count >= 1) || 
                    (ordersDataGet?.number.FirstOrDefault() == ordersDataGet?.orderItemName[i]) || 
                    (isOrderTrue == false))
                {
                    a++;
                }
            }

            if (a==0)
            {
                return "Успешно";
            }

            return "Ограничение предметной области";

        }

        /// <summary>
        /// получение данных для выбранного orderId
        /// </summary>
        /// <param name="id"></param>
        private void SelectItemsById(int id)
        {
            model.ordersData = model.ordersData.Where(x=>x.orderId == id).ToList();
            model.orderId = id;
            model.number = model.ordersData.FirstOrDefault()?.number;
            model.date = model.ordersData.FirstOrDefault()?.date;
            
            if (model.ordersData.Count==0)
            {
                var a = model.orders.FirstOrDefault(x => x.Id == id);
                if (a!=null)
                {
                    model.ordersData.Add(new OrdersData
                    {
                        orderId = a.Id,
                        number = a.Number,
                        date = a.Date,
                        providerId = a.ProviderId
                    });
                    model.number = a.Number;
                    model.date = a.Date;
                }
            }

            MainFilterData();
        }

        /// <summary>
        /// Заполнение фильтра данными из заказа
        /// </summary>
        private void MainFilterData()
        {
            model.dataForFilter.providerId = model.ordersData.Select(x => x.providerId).ToList().DistinctBy(x => x).ToList();
            model.dataForFilter.providerName = model.ordersData.Select(x => x.providerName).ToList().DistinctBy(x => x).ToList();
            model.dataForFilter.orderItemName = model.ordersData.Select(x => x.orderItemName).ToList().DistinctBy(x => x).ToList();
            model.dataForFilter.unit = model.ordersData.Select(x => x.unit).ToList().DistinctBy(x => x).ToList();
        }

        /// <summary>
        /// фильтрация по number
        /// </summary>
        /// <param name="number"></param>
        private void FilterNumber(List<string> number)
        {
            if (number != null)
            {
                foreach (var item in number)
                {
                    var a = model.ordersData.Where(x => x.number == item).ToList();
                    modelHelper.ordersData = modelHelper.ordersData.Union(a).ToList();
                }
                model.ordersData = modelHelper.ordersData;
            }
        }

        /// <summary>
        /// фильтрация по providerId
        /// </summary>
        /// <param name="providerId"></param>
        private void FilterProviderId(List<int> providerId)
        {
            
            if (providerId != null)
            {
                foreach (var item in providerId)
                {
                    var a = model.ordersData.Where(x => x.providerId == item).ToList();
                    modelHelper.ordersData = modelHelper.ordersData.Union(a).ToList();
                }
                model.ordersData = modelHelper.ordersData;
            }
        }

        /// <summary>
        /// фильтрация по orderItemName
        /// </summary>
        /// <param name="orderitemName"></param>
        private void FilterOrderitemName(List<string> orderitemName)
        {
            if (orderitemName != null)
            {
                foreach (var item in orderitemName)
                {
                    var a = model.ordersData.Where(x => x.orderItemName == item).ToList();
                    modelHelper.ordersData = modelHelper.ordersData.Union(a).ToList();
                }
                model.ordersData = modelHelper.ordersData;
            }
        }

        /// <summary>
        /// фильтрация по unit
        /// </summary>
        /// <param name="orderitemUnit"></param>
        private void FilterOrderitemUnit(List<string> orderitemUnit)
        {
            if (orderitemUnit != null)
            {
                foreach (var item in orderitemUnit)
                {
                    var a = model.ordersData.Where(x => x.unit == item).ToList();
                    modelHelper.ordersData = modelHelper.ordersData.Union(a).ToList();
                }
                model.ordersData = modelHelper.ordersData;
            }
        }
    }
}
