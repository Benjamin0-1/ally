
using Ally.Application.Core.Products.Command;

namespace Ally.Application.Abstraction.Product
{
    /*
     * Creates a simple base product
     */
    public interface ICreateProductRepository
    {
        Task<bool> CreateProductAsync(CreateProductCommand request);
    }
}
