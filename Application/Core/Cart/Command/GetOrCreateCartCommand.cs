using Ally.Domain.Entities;
using MediatR;

namespace Ally.Application.Core.Cart.Command
{
    public class GetOrCreateCartCommand : IRequest<CartEntity> // <-- define well what you need in return;
    {
    }
}
