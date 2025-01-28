using Ally.Application.Abstraction.JWT;
using Ally.Application.Abstraction.Product;
using Ally.Application.Core.Products.Command;
using Ally.Domain.Entities;
using Ally.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Ally.Infrastructure.Repositories.Products
{
    public class CreateProductVariantRepository : ICreateProductVariantRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtRepository _jwtRepository;

        // Constructor should be inside the class
        public CreateProductVariantRepository(ApplicationDbContext context, IJwtRepository jwtRepository)
        {
            _context = context;
            _jwtRepository = jwtRepository;
        }

        // The method to create a product variant
        public async Task<bool> CreateProductVariantAsync(CreateProductVariantCommand request)
        {
            try
            {
                var newProductVariant = new ProductVariantEntity
                {
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock,
                    ProductId = request.ProductId,
                    UserId = await _jwtRepository.GetUserIdFromJwt()
                };

                await _context.ProductVariants.AddAsync(newProductVariant);
                await _context.SaveChangesAsync();

                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false; 
            }
        }
    }
}
