using MediatR;

namespace Ally.Application.Core.Products.Command
{
    public class CreateProductCommand : IRequest<bool> // <-- whether created or not
    {
        public string Name { get; set; } 
    }
}
