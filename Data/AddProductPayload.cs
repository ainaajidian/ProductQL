using ProductQL.Models;

namespace ProductQL.Data
{
    public class AddProductPayload
    {
        public AddProductPayload(Product product)
        {
            Product = product;
        }

        public Product Product { get; }
    }
}
