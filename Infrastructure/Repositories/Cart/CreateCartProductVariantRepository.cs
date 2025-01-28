

using Ally.Domain.Entities;
using Ally.Infrastructure.Data;

namespace Ally.Infrastructure.Repositories.Cart
{
    partial class CreateCartProductVariantRepository : ICreateCartProductVariantRepository
    {
        private readonly ApplicationDbContext _context;

        public CreateCartProductVariantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCartProductVariant(int cartId, int productVariantId, int quantity)
        {
            try
            {
                var newCartProductVariant = new CartProductVariantEntity
                {
                    CartId = cartId,
                    ProductVariantId = productVariantId,
                    Quantity = quantity,
                };

                _context.CartProductVariants.Add(newCartProductVariant);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
