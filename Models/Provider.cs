using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class Provider 
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Name { get; set; }
    }
}
