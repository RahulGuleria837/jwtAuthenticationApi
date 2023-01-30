using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_JWT_Angular_1.Identity
{
  public class ApplicationSigninManager : SignInManager<ApplicationUser>
  {
    public ApplicationSigninManager(ApplicationUserManager applicationUserManager,
  IHttpContextAccessor httpContextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
  IOptions<IdentityOptions> options, ILogger<ApplicationSigninManager> logger,
  IAuthenticationSchemeProvider authenticationSchemeProvider, IUserConfirmation<ApplicationUser> userConfirmation
  ) : base(applicationUserManager, httpContextAccessor, userClaimsPrincipalFactory, options, logger, authenticationSchemeProvider, userConfirmation)
    {

    }

  }
}
