namespace WebSite.Models
{
    class HomeFactory : IFactory
    {
        private DataContext dataContext { get; set; }
        public HomeFactory(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public IPageModel Create()
        {
            IPageModel homePageModel = new HomePageModel(dataContext);
            homePageModel.InitialData();
            return homePageModel;
        }
    }
}
