namespace WebSite.Models
{
    public class AddOrderPageModel : IPageModel
    {
        public List<string> providers { get; set; } 
        public Order orders { get; set; }
        public IPageModel pageModel { get; set; }
        private DataContext dataContext { get; set; }

        public AddOrderPageModel(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void InitialData()
        {
            providers = dataContext.Provider.Select(x=>x.Name).ToList();

            pageModel = new AddOrderPageModel(dataContext) { providers = providers };
        }
    }
}
