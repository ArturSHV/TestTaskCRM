namespace WebSite.Models
{
    public class AddOrderFactory : IFactory
    {
        public IPageModel Create()
        {
            return new AddOrderPageModel();
        }
    }
}
