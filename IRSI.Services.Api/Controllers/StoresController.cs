using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IRSI.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRSI.Services.Api.Controllers
{
    [Route("api/[controller]")]
    public class StoresController : Controller
    {
        private readonly CommonContext _commonContext;
        private readonly IAuthorizationService _authorizationService;

        public StoresController(CommonContext commonContext, IAuthorizationService authorizationService)
        {
            _commonContext = commonContext;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _commonContext.Stores.ToListAsync();
            return new JsonResult(stores);
        }
    }
}
