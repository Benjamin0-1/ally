using MediatR;

namespace Ally.Application.Core.Cart.Command
{
    public class AddToCartCommand
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }

        public int CartId { get; set; } // <-- should be gotten from user directly?
    }
}
