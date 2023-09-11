
using System.ComponentModel.DataAnnotations;

namespace ProductAPI
{
    public class Product
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
}
