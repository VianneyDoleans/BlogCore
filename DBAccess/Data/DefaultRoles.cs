using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBAccess.Data;

public class DefaultRoles : IPoco
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public Role Role { get; set; }
}