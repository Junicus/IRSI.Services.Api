using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IRSI.Services.Api.Security.Requirements;
using IRSI.Services.Data;
using Microsoft.AspNetCore.Authorization;
using IRSI.Services.Api.Data;
using Microsoft.AspNetCore.Hosting;
using IRSI.Services.Api.Services;
using Microsoft.EntityFrameworkCore;
using IRSI.Services.Api.Models.SOS;
using IRSI.Services.Api.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRSI.Services.Api.Controllers
{
    [Authorize(Policy = "CanCallSOS")]
    [Route("api/[controller]")]
    public class SOSController : Controller
    {
        private readonly CommonContext _commonContext;
        private readonly SOSContext _sosContext;
        private IHostingEnvironment _environment;
        private ISOSServiceBusService _busService;
        private IAuthorizationService _authorizationService;

        public SOSController(CommonContext commonContext, SOSContext sosContext, IHostingEnvironment environment, ISOSServiceBusService busService, IAuthorizationService authorizationService)
        {
            _commonContext = commonContext;
            _sosContext = sosContext;
            _environment = environment;
            _busService = busService;
            _authorizationService = authorizationService;
        }

        [HttpGet("stores")]
        public IActionResult GetStores()
        {
            var sosRole = HttpContext.User.FindFirst("sosApiRole").Value;
            switch (sosRole)
            {
                case "office_manager":
                    return new JsonResult(new { Stores = _sosContext.Stores });
                case "region_manager":
                    var region = HttpContext.User.FindFirst("sosApiRegion").Value;
                    if (string.IsNullOrEmpty(region))
                    {
                        return BadRequest();
                    }
                    var regionId = Guid.Empty;
                    if (Guid.TryParse(region, out regionId))
                    {
                        var stores = _sosContext.Stores.Where(s => s.RegionId == regionId);
                        if (stores.Any())
                        {
                            return new JsonResult(new { Stores = stores });
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    return BadRequest();
                case "store_manager":
                    var storeClaim = HttpContext.User.FindFirst("sosApiStore").Value;
                    if (string.IsNullOrEmpty(storeClaim))
                    {
                        return BadRequest();
                    }
                    var storeId = Guid.Empty;
                    if (Guid.TryParse(storeClaim, out storeId))
                    {
                        var stores = _sosContext.Stores.Where(s => s.Id == storeId).ToList();
                        if (stores != null)
                        {
                            return new JsonResult(new { Stores = stores });
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    return BadRequest();
                default:
                    return Forbid();
            }
        }

        [HttpGet("stores/{storeId}")]
        public async Task<IActionResult> GetStoreAsync(Guid storeId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var store = await _sosContext.Stores.Where(s => s.Id == storeId).SingleOrDefaultAsync();
            if (store == null)
            {
                return NotFound();
            }

            if (await _authorizationService.AuthorizeAsync(User, store, new StoreAccessRequirement()))
            {
                return new JsonResult(store);
            }
            else
            {
                return Forbid();
            }
        }

        [HttpGet("stores/{storeId}/kpi")]
        public async Task<IActionResult> GetKpis(Guid storeId, DateTime startDate, DateTime endDate)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var store = await _sosContext.Stores.SingleOrDefaultAsync(s => s.Id == storeId);
            if (await _authorizationService.AuthorizeAsync(User, store, new StoreAccessRequirement()))
            {
                var kpis = await _sosContext.SOSAverateTTs.GetAverageTTAsync(storeId, startDate, endDate);
                return new JsonResult(kpis);
            }
            else
            {
                return Forbid();
            }
        }

        [HttpGet("stores/{storeId}/summary")]
        public async Task<IActionResult> GetSummary(Guid storeId, DateTime startDate, DateTime endDate)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var store = await _sosContext.Stores.SingleOrDefaultAsync(s => s.Id == storeId);
            if (await _authorizationService.AuthorizeAsync(User, store, new StoreAccessRequirement()))
            {
                var dayParts = await _commonContext.DayParts.OrderBy(dp => dp.OrderHint).ToListAsync();
                var summaries = await _sosContext.SOSSummaries.GetSummaries(storeId, startDate, endDate);
                var summaryModels = new List<SummaryModel>();
                foreach (var dayPart in dayParts)
                {
                    foreach (var summary in summaries.Where(s => s.DayPartId == dayPart.Id))
                    {
                        summaryModels.Add(new SummaryModel()
                        {
                            BusinessDate = summary.BusinessDate,
                            DayPart = dayPart.Name,
                            Summary = summary.SummaryGroup.Name(),
                            Count = summary.Count
                        });
                    }
                }

                var summaryGrouped = from s in summaryModels
                                     group s by s.DayPart into gDatePart
                                     select new
                                     {
                                         DayPart = gDatePart.Key,
                                         DayPartGroup = from s in gDatePart
                                                        group s by s.BusinessDate into gDate
                                                        select new
                                                        {
                                                            BusinessDate = gDate.Key,
                                                            DateGroup = gDate
                                                        }
                                     };

                return new JsonResult(summaryGrouped);
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPost("stores/{storeId}/uploadSOS")]
        public async Task<IActionResult> PostSOSFile([FromBody]SOSItemsPost sosItemsPost)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _busService.PostSOSDataAsync(sosItemsPost.SOSItems);
                    return Ok();
                }
                catch (Exception ex)
                {
                    //log
                    return BadRequest(ex);
                }
            }
            return BadRequest();
        }
    }
}
