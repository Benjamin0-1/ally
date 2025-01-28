using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ally.Domain.Entities;

public class ProductEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } // <-- unique?

    [Required]
    public int UserId { get; set; } // <-- CreatedByUserId, no real relationship, just a reference.

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ProductVariantEntity> ProductVariants { get; set; } = new List<ProductVariantEntity>();
}

