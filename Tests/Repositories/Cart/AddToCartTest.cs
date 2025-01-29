using Xunit;
using Moq;
using Ally.Application.Abstraction.JWT;
using Ally.Infrastructure.Repositories.Cart;
using Ally.Infrastructure.Data;
using Ally.Domain.Entities;
using Ally.Application.Core.Cart.Command;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ally.Tests.Repositories.Cart
{
    public class AddToCartTest
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<IJwtRepository> _mockJwtRepository;
        private readonly Mock<ICreateCartProductVariantRepository> _mockCreateCartProductVariantRepository;
        private readonly AddToCartRepository _addToCartRepository;

        public AddToCartTest()
        {
            _mockJwtRepository = new Mock<IJwtRepository>();
            _mockContext = new Mock<ApplicationDbContext>();
            _mockCreateCartProductVariantRepository = new Mock<ICreateCartProductVariantRepository>();

            _addToCartRepository = new AddToCartRepository(
                _mockContext.Object,
                _mockJwtRepository.Object,
                _mockCreateCartProductVariantRepository.Object);
        }

        [Fact]
        public async Task AddToCartAsync_ShouldUpdateQuantity_WhenProductVariantExists()
        {
            // Arrange
            var existingProductVariant = new CartProductVariantEntity
            {
                CartId = 1,
                ProductVariantId = 1,
                Quantity = 1
            };

            var command = new AddToCartCommand
            {
                CartId = 1,
                ProductVariantId = 1,
                Quantity = 1
            };

            // Set up mock to simulate an existing cart product variant
            var mockDbSet = new Mock<DbSet<CartProductVariantEntity>>();
            var data = new[] { existingProductVariant }.AsQueryable();

            mockDbSet.As<IQueryable<CartProductVariantEntity>>()
                .Setup(m => m.Provider).Returns(data.Provider);

            mockDbSet.As<IQueryable<CartProductVariantEntity>>()
                .Setup(m => m.Expression).Returns(data.Expression);

            mockDbSet.As<IQueryable<CartProductVariantEntity>>()
                .Setup(m => m.ElementType).Returns(data.ElementType);

            mockDbSet.As<IQueryable<CartProductVariantEntity>>()
                .Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(x => x.CartProductVariants).Returns(mockDbSet.Object);
            _mockJwtRepository.Setup(x => x.GetUserIdFromJwt()).ReturnsAsync(1);

            // Act
            var result = await _addToCartRepository.AddToCartAsync(command);

            Assert.True(result);
            Assert.Equal(2, existingProductVariant.Quantity); 
        }

        [Fact]
        public async Task AddToCartAsync_ShouldCreateNewCart_WhenNoCartExistsForUser()
        {

            var command = new AddToCartCommand
            {
                ProductVariantId = 1,
                Quantity = 1
            };

            _mockJwtRepository.Setup(x => x.GetUserIdFromJwt()).ReturnsAsync(1);

            _mockContext.Setup(x => x.CartProductVariants)
                .Returns(MockDbSet(new CartProductVariantEntity[] { }.AsQueryable())); 

            _mockCreateCartProductVariantRepository.Setup(x =>
                x.CreateCartProductVariant(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _addToCartRepository.AddToCartAsync(command);

            Assert.True(result);
        }

        private static DbSet<T> MockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }
    }
}
