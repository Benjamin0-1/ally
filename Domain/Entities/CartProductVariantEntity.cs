using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*
 * This will be the many-to-many relationship between the Cart and the ProductVariant
 * Having the custom "Quantity" attribute, hence the manual creation of it.
 */

namespace Ally.Domain.Entities
{
    public class CartProductVariantEntity
    {
        [Key]
        [Column(Order = 0)] 
        public int CartId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ProductVariantId { get; set; }

        [ForeignKey("CartId")]
        public CartEntity Cart { get; set; }

        [ForeignKey("ProductVariantId")]
        public ProductVariantEntity ProductVariant { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
