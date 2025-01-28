

using Ally.Domain.Entities;

namespace Ally.Application.Abstraction.Cart
{
    public interface ICreateNewCartEntityRepository
    {
        Task<CartEntity> CreateNewCartAsync(int userId);
    }
}
