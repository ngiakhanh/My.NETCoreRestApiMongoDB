using System.Collections.Generic;

namespace WebApplication1.Resources
{
    public class CategoryResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductResource> Products { get; set; }
    }
}
