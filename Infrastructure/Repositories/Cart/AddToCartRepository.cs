using Ally.Application.Abstraction.Cart;
using Ally.Application.Abstraction.JWT;
using Ally.Application.Core.Cart.Command;
using Ally.Domain.Entities;
using Ally.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ally.Infrastructure.Repositories.Cart
{
    public class AddToCartRepository : IAddToCartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtRepository _jwtRepository;
        private readonly ICreateCartProductVariantRepository _createCartProductVariantRepository;

        public AddToCartRepository(ApplicationDbContext context, IJwtRepository jwtRepository, ICreateCartProductVariantRepository createCartProductVariantRepository)
        {
            _context = context;
            _jwtRepository = jwtRepository;
            _createCartProductVariantRepository = createCartProductVariantRepository;
        }
        /**
         * Should the user's cart be brought in via the relationship instead of manually?
         */
        public async Task<bool> AddToCartAsync(AddToCartCommand request)
        {
            try
            {
                int UserId = await _jwtRepository.GetUserIdFromJwt();

                var existingCartProductVariant = await _context.CartProductVariants
                    .FirstOrDefaultAsync(x => x.CartId == request.CartId && x.ProductVariantId == request.ProductVariantId);

                if (existingCartProductVariant != null)
                {
                    existingCartProductVariant.Quantity += request.Quantity;
                }
                else
                {
                    bool success = await _createCartProductVariantRepository.CreateCartProductVariant(request.CartId, request.ProductVariantId, request.Quantity);

                    if (!success)
                    {
                        return false;
                    }
                }

                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to cart: {ex.Message}");
                return false;
            }
        }

    }
}
