using System;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Core
{
    public class TenantResolve : ITenantResolve
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TenantResolve(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetTenantId()
        {
            var userIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var tenantClaim  = userIdentity?.Claims.FirstOrDefault(c => c.Type == "TenantId");
            if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
            {
                return tenantId;
            }
            throw new InvalidOperationException("Tenant ID not found in the JWT token.");
        }
    }
}
