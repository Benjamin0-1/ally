using MediatR;

namespace Ally.Application.Core.Products.Command
{
    public class CreateProductVariantCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int ProductId { get; set; }
    }
}
