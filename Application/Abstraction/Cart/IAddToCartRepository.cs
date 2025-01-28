

using Ally.Application.Core.Cart.Command;

namespace Ally.Application.Abstraction.Cart
{
    public interface IAddToCartRepository
    {
        Task<bool> AddToCartAsync(AddToCartCommand request);
    }
}
