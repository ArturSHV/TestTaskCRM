namespace WebSite.Models
{
    public class ViewOrderFactory : IFactory
    {
        public IPageModel Create()
        {
            return new ViewOrderPageModel();
        }
    }
}
