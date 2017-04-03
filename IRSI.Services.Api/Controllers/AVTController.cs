using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IRSI.Services.Api.Data;
using IRSI.Services.Api.Models.Data.AVT;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRSI.Services.Api.Controllers
{
    [Route("api/[controller]")]
    public class AVTController : Controller
    {
        private readonly AVTContext _context;

        public AVTController(AVTContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int page_size = 25)
        {
            var recordCount = await _context.AVTItems.CountAsync();
            var totalPages = Convert.ToInt32(Math.Ceiling(new decimal(recordCount) / page_size));
            var avts = from avt in _context.AVTItems
                       orderby avt.FiscalYear descending
                       orderby avt.Period descending
                       select new { AVTItem = avt, _link = Url.Action("GetById", new { id = avt.Id }) };
            if (page > 1)
            {
                avts = avts.Skip((page - 1) * page_size);
            }
            avts = avts.Take(page_size);

            return new JsonResult(new
            {
                totalRecords = recordCount,
                totalPages = totalPages,
                page = page,
                avts = avts,
                _prev = (page > 1) ? Url.Action("Get", new { page = (page - 1), page_size = page_size }) : string.Empty,
                _next = (page < totalPages) ? Url.Action("Get", new { page = (page + 1), page_size = page_size }) : string.Empty,
            });
        }

        [HttpGet("stores")]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _context.Stores.ToListAsync();
            return new JsonResult(stores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return new JsonResult(await _context.AVTItems.SingleOrDefaultAsync(e => e.Id == id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AVTItem value)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new { Description = "AVTItem is not valid" });
            }
            else
            {
                _context.AVTItems.Add(value);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetById", new { id = value.Id }, value);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]AVTItem value)
        {
            var found = await _context.AVTItems.AnyAsync(e => e.Id == id);
            if (!found)
            {
                return new BadRequestObjectResult(new { Description = $"AVTItem with id {id} not found." });
            }

            if (id != value.Id)
            {
                return new BadRequestObjectResult(new { Description = $"id does not match AVTItem." });
            }

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new { Description = $"AVTItem is not valid" });
            }

            _context.AVTItems.Update(value);
            await _context.SaveChangesAsync();
            return new JsonResult(value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var avt = await _context.AVTItems.SingleOrDefaultAsync(e => e.Id == id);
            if (avt == null)
            {
                return new NotFoundObjectResult(new { Description = $"AVTItem with id {id} not found." });
            }
            _context.AVTItems.Remove(avt);
            await _context.SaveChangesAsync();
            return new JsonResult(avt);
        }
    }
}
