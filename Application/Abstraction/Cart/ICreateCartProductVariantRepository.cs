
namespace Ally.Application.Abstraction.Cart
{
    partial interface ICreateCartProductVariantRepository
    {
        Task<bool> CreateCartProductVariant(int cartId, int productVariantId, int quantity);
    }
}
