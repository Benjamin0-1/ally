namespace Ally.Infrastructure.Repositories.Cart
{
    public interface ICreateCartProductVariantRepository
    {
        Task<bool> CreateCartProductVariant(int cartId, int productVariantId, int quantity);
    }
}
