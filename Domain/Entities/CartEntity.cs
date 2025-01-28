using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


/**
 * Regular Cart table
 */

namespace Ally.Domain.Entities
{
    public class CartEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        /**
         * Come back to this entity later for tracking functionality.
         */

        // cart initiated at (session "tracking")
        // hasBeenAbondedAt (session "tracking")
    }
}
