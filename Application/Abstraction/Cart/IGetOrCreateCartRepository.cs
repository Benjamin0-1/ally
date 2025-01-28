

using Ally.Domain.Entities;

namespace Ally.Application.Abstraction.Cart
{
    public interface IGetOrCreateCartRepository
    {
        Task<CartEntity> CreateOrGetCartAsync();
    }
}
