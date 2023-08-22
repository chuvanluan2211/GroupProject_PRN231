using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiceManagement.Models;
using System.Data;

namespace RiceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly ProjectBl5Context _context;
        public StatisticController(ProjectBl5Context context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize(Policy = "AdminRolePolicy")]
        public ActionResult GetAll () {
            var imports = _context.Imports.ToList();
            if(imports == null)
            {
                return NotFound();
            }
            return Ok(imports);
        }

    }
}
