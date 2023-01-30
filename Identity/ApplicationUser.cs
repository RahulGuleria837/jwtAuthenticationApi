using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_JWT_Angular_1.Identity
{
  public class ApplicationUser : IdentityUser
  {
    [NotMapped]
    public string? RefreshToken { get; set; }
    [NotMapped]
    public string? Role { get; set; }
  }
}
