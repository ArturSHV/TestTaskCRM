namespace WebSite.Models
{
    public class ViewOrderFactory : IFactory
    {
        private DataContext dataContext;
        public ViewOrderFactory(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public IPageModel Create()
        {
            IPageModel viewOrderPageModel = new ViewOrderPageModel(dataContext);
            viewOrderPageModel.InitialData();
            return viewOrderPageModel;
        }
    }
}
