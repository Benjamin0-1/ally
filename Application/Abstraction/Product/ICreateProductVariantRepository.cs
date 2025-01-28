

using Ally.Application.Core.Products.Command;

namespace Ally.Application.Abstraction.Product
{
    public interface ICreateProductVariantRepository
    {
        Task<bool> CreateProductVariantAsync(CreateProductVariantCommand request);
    }
}
