namespace WebSite.Models
{
    public class AddOrderFactory : IFactory
    {
        private DataContext dataContext { get; set; }
        public AddOrderFactory(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public IPageModel Create()
        {
            IPageModel addOrderPageModel = new AddOrderPageModel(dataContext);
            addOrderPageModel.InitialData();
            return addOrderPageModel;
        }
    }
}
