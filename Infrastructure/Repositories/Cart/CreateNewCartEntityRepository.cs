

using Ally.Application.Abstraction.Cart;
using Ally.Domain.Entities;
using Ally.Infrastructure.Data;

namespace Ally.Infrastructure.Repositories.Cart
{
    public class CreateNewCartEntityRepository : ICreateNewCartEntityRepository
    {
        private readonly ApplicationDbContext _context;

        public CreateNewCartEntityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartEntity> CreateNewCartAsync(int userId)
        {
            var cart = new CartEntity
            {
                UserId = userId
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
    }
}
