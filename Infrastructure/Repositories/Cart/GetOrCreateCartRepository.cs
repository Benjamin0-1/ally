using Ally.Application.Abstraction.Cart;
using Ally.Application.Abstraction.JWT;
using Ally.Domain.Entities;
using Ally.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ally.Infrastructure.Repositories.Cart
{
    public class GetOrCreateCartRepository : IGetOrCreateCartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtRepository _jwtRepository;
        private readonly ICreateNewCartEntityRepository _createNewCartRepository;

        public GetOrCreateCartRepository(ApplicationDbContext context, IJwtRepository jwtRepository, ICreateNewCartEntityRepository createNewCartRepository)
        {
            _context = context;
            _jwtRepository = jwtRepository;
            _createNewCartRepository = createNewCartRepository;
        }

        // Method to get or create a cart
        public async Task<CartEntity> CreateOrGetCartAsync()
        {
            try
            {
                int userId = await _jwtRepository.GetUserIdFromJwt();

                var existingCart = await _context.Carts
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (existingCart != null) {
                    return existingCart;
                }

                var newCart = await _createNewCartRepository.CreateNewCartAsync(userId);

                await _context.Carts.AddAsync(newCart);
                await _context.SaveChangesAsync();
                return newCart;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating or retrieving cart: {ex.Message}");
                throw; 
            }
        }
    }
}
