using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ally.Domain.Entities;

public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress] // <- verify
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string ConfirmPassword { get; set; }
    
    // RoleId <- a user works one role.

    [Required] public int RoleId { get; set; } = 0; // <-- check if reference error in db.
    
    [ForeignKey("RoleId")]
    public RoleEntity Role { get; set; }
}

