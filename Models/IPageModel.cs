using System.Collections;

namespace WebSite.Models
{
    public interface IPageModel
    {
        public IPageModel pageModel { get; set; }
        void InitialData();
    }
}
