using Microsoft.AspNetCore.Mvc;
using RiceManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RiceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiceController : ControllerBase
    {
        private readonly ProjectBl5Context _context;
        public RiceController(ProjectBl5Context context)
        {
            _context = context;
        }
        // GET: api/<RiceController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rice>>> Get()
        {
            var getlist = _context.Rice.ToList();
            return Ok(getlist);
        }

        // GET api/<RiceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RiceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
