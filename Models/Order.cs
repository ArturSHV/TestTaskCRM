using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Number { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime Date { get; set; }
        public int ProviderId { get; set; }
    }
}
