namespace WebSite.Models
{
    class HomeFactory : IFactory
    {
        public IPageModel Create()
        {
            return new HomePageModel();
        }
    }
}
