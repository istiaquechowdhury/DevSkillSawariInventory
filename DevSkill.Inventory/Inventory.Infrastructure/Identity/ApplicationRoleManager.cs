﻿using System.Collections.Generic;
using Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Identity
{
    public class ApplicationRoleManager
        : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole> store, 
            IEnumerable<IRoleValidator<ApplicationRole>> roleValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
            ILogger<RoleManager<ApplicationRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
