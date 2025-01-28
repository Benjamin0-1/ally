using Ally.Application.Abstraction.Authentication;
using Ally.Application.Abstraction.JWT;
using Ally.Application.Abstraction.Product;
using Ally.Application.Core.Products.Command;
using Ally.Domain.Entities;
using Ally.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ally.Infrastructure.Repositories.Products
{
    public class CreateProductRepository : ICreateProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtRepository _jwtRepository;

        public CreateProductRepository(ApplicationDbContext context, IJwtRepository jwtRepository)
        {
            _context = context;
            _jwtRepository = jwtRepository;
        }

        public async Task<bool> CreateProductAsync(CreateProductCommand request)
        {
            try
            {
                var newProduct = new ProductEntity
                {
                    Name = request.Name,
                    UserId = await _jwtRepository.GetUserIdFromJwt()
                };

                await _context.Products.AddAsync(newProduct);

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
