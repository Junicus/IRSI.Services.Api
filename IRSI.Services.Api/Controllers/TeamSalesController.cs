using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IRSI.Services.Api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using IRSI.Services.Api.Models.TeamSales;
using Microsoft.Extensions.Caching.Memory;
using IRSI.Services.Api.Models.Data.TeamSales.Aloha;
using IRSI.Services.Api.Extensions.ModelExtensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRSI.Services.Api.Controllers
{
    [Authorize(Policy = "CanCallTeam")]
    [Route("api/{concept}/[controller]")]
    public class TeamSalesController : Controller
    {
        private readonly CommonContext _commonContext;
        private readonly TeamSalesDbContext _context;

        public TeamSalesController(IDbContextFactory<TeamSalesDbContext> contextFactory, CommonContext commonContext)
        {
            _context = contextFactory.Create(new DbContextFactoryOptions());
            _commonContext = commonContext;
        }

        public IActionResult Get()
        {
            var concept = RouteData.Values["concept"];
            return new JsonResult(concept);
        }

        [HttpGet("/api/[controller]/store/{id}")]
        public async Task<IActionResult> GetStore(Guid id)
        {
            var result = await _commonContext.Stores.Where(t => t.Id == id).SingleOrDefaultAsync();
            return new JsonResult(result);
        }

        [HttpGet("empsalesperhour")]
        public IActionResult EmpSalesPerHour(int storeId, DateTime startDate, DateTime endDate, string filter = "top", int count = 10)
        {
            if (filter != "top" && filter != "bottom")
            {
                return BadRequest();
            }

            if (_context == null)
            {
                return BadRequest();
            }

            var concept = HttpContext.GetRouteValue("concept").ToString().ToLower();
            if (string.IsNullOrEmpty(concept) && (concept != "cgb" || concept != "rmg" || concept != "otb" || concept != "pfc"))
            {
                return BadRequest();
            }

            var jobCodePredicate = GetJobCodePredicate();

            var employeeList = _context.EmployeeByStore.Where(c => c.StoreId == storeId).ToList();

            var employeeSalesFilter = _context.ShiftHistory
                .Where(t => t.DateOfBusiness >= startDate && t.DateOfBusiness <= endDate && t.StoreId == storeId && t.TotalHours > 0)
                .Where(jobCodePredicate)
                .GroupBy(t => t.EmployeeNumber);
            if (filter == "top")
            {
                employeeSalesFilter = employeeSalesFilter.OrderByDescending(cl => (cl.Sum(t => t.Sales) / cl.Sum(t => t.TotalHours)));
            }
            else
            {
                employeeSalesFilter = employeeSalesFilter.OrderBy(cl => (cl.Sum(t => t.Sales) / cl.Sum(t => t.TotalHours)));
            }

            var employeeSales = employeeSalesFilter.Select(cl => new EmployeeSalesPerHour
            {
                EmployeeId = cl.Key,
                SalesPerHour = (cl.Sum(t => t.Sales) / cl.Sum(t => t.TotalHours)),
                Shifts = cl.Count(),
                EmployeeName = employeeList.Where(t => t.EmployeeNumber == cl.Key).SingleOrDefault()?.GetFullName(),
                StoreId = storeId
            }).Take(count);


            return new JsonResult(employeeSales);
        }

        [HttpGet("empsalesperhourimprove")]
        public IActionResult EmpSalesPerHourImprove([FromServices]IMemoryCache cache, int storeId, DateTime startDate, DateTime endDate, int count = 10, string filter = "week")
        {
            if (filter != "week" && filter != "period")
            {
                return BadRequest();
            }

            if (_context == null)
            {
                return BadRequest();
            }

            var concept = HttpContext.GetRouteValue("concept").ToString().ToLower();
            if (string.IsNullOrEmpty(concept) && (concept != "cgb" || concept != "rmg" || concept != "otb" || concept != "pfc"))
            {
                return BadRequest();
            }

            var cacheKey = $"empsalesperhour_{storeId}_{startDate.ToFileTimeUtc()}_{endDate.ToFileTimeUtc()}_{count}_{filter}";
            List<EmployeeSalesPerHourComparative> results;
            if (!cache.TryGetValue(cacheKey, out results))
            {
                DateTime compStartDate = DateTime.MinValue;
                DateTime compEndDate = DateTime.MinValue;

                var jobCodePredicate = GetJobCodePredicate();

                var employeeList = _context.EmployeeByStore.Where(t => t.StoreId == storeId).ToList();

                var employeeSalesCurrent = _context.ShiftHistory
                    .Where(t => t.DateOfBusiness >= startDate && t.DateOfBusiness <= endDate && t.StoreId == storeId && t.TotalHours > 0)
                    .Where(jobCodePredicate)
                    .GroupBy(t => t.EmployeeNumber)
                    .Select(c => new EmployeeSalesPerHour
                    {
                        EmployeeId = c.Key,
                        SalesPerHour = (c.Sum(t => t.Sales) / c.Sum(t => t.TotalHours)),
                        Shifts = c.Count(),
                        EmployeeName = employeeList.Where(t => t.EmployeeNumber == c.Key).SingleOrDefault()?.GetFullName(),
                        StoreId = storeId
                    }).ToList();

                if (filter == "week")
                {
                    var dayOfWeek = ((int)startDate.DayOfWeek) - 1;
                    var startOfWeek = startDate.AddDays(dayOfWeek * -1);
                    compStartDate = startOfWeek.AddDays(-7);
                    compEndDate = startOfWeek.AddDays(-1);
                }
                else
                {
                    var dateDim = _context.DateDimensions.Where(t => t.Date.Date == startDate.Date).SingleOrDefault();
                    if (dateDim != null)
                    {
                        var compFiscalPeriod = dateDim.FiscalPeriod - 1;
                        if (compFiscalPeriod > 0)
                        {
                            var compDates = _context.DateDimensions.Where(t => t.FiscalYear == dateDim.FiscalYear && t.FiscalPeriod == compFiscalPeriod)
                                .OrderBy(t => t.FiscalDayOfPeriod).ToList();
                            compStartDate = compDates.First().Date;
                            compEndDate = compDates.Last().Date;
                        }
                        else
                        {
                            var compDates = _context.DateDimensions.Where(t => t.FiscalYear == dateDim.FiscalYear - 1).OrderBy(t => t.Date).ToList();
                            var compCorrectPeriod = compDates.Last().FiscalPeriod;
                            var compCorrectDates = compDates.Where(t => t.FiscalPeriod == compCorrectPeriod);
                            compStartDate = compCorrectDates.First().Date;
                            compEndDate = compCorrectDates.Last().Date;
                        }
                    }
                }

                var employeeSalesComparative = _context.ShiftHistory
                    .Where(t => t.DateOfBusiness >= compStartDate && t.DateOfBusiness <= compEndDate && t.StoreId == storeId && t.TotalHours > 0)
                    .Where(jobCodePredicate)
                    .GroupBy(t => t.EmployeeNumber)
                    .Select(c => new EmployeeSalesPerHour
                    {
                        EmployeeId = c.Key,
                        SalesPerHour = (c.Sum(t => t.Sales) / c.Sum(t => t.TotalHours)),
                        Shifts = c.Count(),
                        EmployeeName = employeeList.Where(t => t.EmployeeNumber == c.Key).SingleOrDefault()?.GetFullName(),
                        StoreId = storeId
                    }).ToList();

                var resultsTemp = new List<EmployeeSalesPerHourComparative>();
                foreach (var emp in employeeSalesCurrent)
                {
                    var result = new EmployeeSalesPerHourComparative()
                    {
                        EmployeeId = emp.EmployeeId,
                        EmployeeName = emp.EmployeeName,
                        StoreId = storeId,
                        CurrentSalesPerHour = emp.SalesPerHour,
                        ComparativeSalesPerHour = employeeSalesComparative.Where(t => t.EmployeeId == emp.EmployeeId).Any() ? employeeSalesComparative.Where(t => t.EmployeeId == emp.EmployeeId).SingleOrDefault().SalesPerHour : 0
                    };
                    result.Difference = result.CurrentSalesPerHour - result.ComparativeSalesPerHour;
                    resultsTemp.Add(result);
                }

                foreach (var emp in employeeSalesComparative)
                {
                    if (resultsTemp.Any(t => t.EmployeeId == emp.EmployeeId)) continue;
                    var result = new EmployeeSalesPerHourComparative()
                    {
                        EmployeeId = emp.EmployeeId,
                        EmployeeName = emp.EmployeeName,
                        StoreId = storeId,
                        CurrentSalesPerHour = 0,
                        ComparativeSalesPerHour = emp.SalesPerHour
                    };
                    result.Difference = result.CurrentSalesPerHour - result.ComparativeSalesPerHour;
                    resultsTemp.Add(result);
                }
                var orderResult = resultsTemp.OrderByDescending(t => t.Difference).ToList();

                results = orderResult.Take(count).ToList();

                if (results.Any())
                {
                    if (endDate >= DateTime.Today)
                    {
                        cache.Set(cacheKey, results, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                    }
                    else
                    {
                        cache.Set(cacheKey, results, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(5)));
                    }
                }
            }

            return new JsonResult(results);
        }

        [HttpGet("salesperhour")]
        public IActionResult StoreSalesPerHour(int storeId, DateTime startDate, DateTime endDate)
        {
            if (_context == null)
            {
                return BadRequest();
            }

            var jobCodePredicate = GetJobCodePredicate();
            var salesPerHourFilter = _context.ShiftHistory
                .Where(t => t.DateOfBusiness >= startDate && t.DateOfBusiness <= endDate && t.StoreId == storeId)
                .Where(jobCodePredicate)
                .GroupBy(t => t.StoreId);
            var salesPerHour = salesPerHourFilter.Select(cl => cl.Sum(t => t.Sales) / cl.Sum(t => t.TotalHours)).SingleOrDefault();
            return new JsonResult(new { SalesPerHour = salesPerHour });
        }

        private Func<ShiftHistory, bool> GetJobCodePredicate()
        {
            Func<ShiftHistory, bool> jobCodePredicate = null;
            switch (HttpContext.GetRouteValue("concept").ToString().ToLower())
            {
                case "cgb":
                case "rmg":
                    jobCodePredicate = t => (t.JobCodeId == 1 || t.JobCodeId == 14);
                    break;
            }
            return jobCodePredicate;
        }
    }
}
