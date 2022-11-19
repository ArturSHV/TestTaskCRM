namespace WebSite.Models
{
    public class AddOrderPageModel : IPageModel
    {
        public List<string> providers { get; set; } 
        public Order orders { get; set; }

        public IPageModel InitialData(DataContext dataContext)
        {
            providers = dataContext.Provider.Select(x=>x.Name).ToList();

            return new AddOrderPageModel() { providers = providers };
        }
    }
}
