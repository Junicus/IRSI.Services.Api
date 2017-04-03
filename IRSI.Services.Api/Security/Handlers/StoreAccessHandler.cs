using IRSI.Services.Api.Security.Requirements;
using IRSI.Services.Models.Data.Common;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Security.Handlers
{
    public class StoreAccessHandler : AuthorizationHandler<StoreAccessRequirement, Store>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StoreAccessRequirement requirement, Store resource)
        {
            var sosRole = context.User.FindFirst("sosApiRole").Value;
            switch (sosRole)
            {
                case "office_manager":
                    context.Succeed(requirement);
                    break;
                case "region_manager":
                    var region = context.User.FindFirst("sosApiRegion").Value;
                    if (string.IsNullOrEmpty(region))
                    {
                        context.Fail();
                        break;
                    }
                    var regionId = Guid.Empty;
                    if (Guid.TryParse(region, out regionId))
                    {
                        if (resource.RegionId == regionId)
                        {
                            context.Succeed(requirement);
                            break;
                        }
                    }
                    context.Fail();
                    break;
                case "store_manager":
                    var storeClaim = context.User.FindFirst("sosApiStore").Value;
                    if (string.IsNullOrEmpty(storeClaim))
                    {
                        context.Fail();
                        break;
                    }
                    var storeId = Guid.Empty;
                    if (Guid.TryParse(storeClaim, out storeId))
                    {
                        if (resource.Id == storeId)
                        {
                            context.Succeed(requirement);
                            break;
                        }
                    }
                    context.Fail();
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
