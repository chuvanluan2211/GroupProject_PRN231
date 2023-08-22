using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiceManagement.DTOs;
using RiceManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RiceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ProjectBl5Context _context;
        public ImportController(ProjectBl5Context context)
        {
            _context = context;
        }
        // GET: api/<ImportController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Import>>> Get()
        {
            var getlist = _context.Imports.ToList();
            return Ok(getlist);
        }

        // GET api/<ImportController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ImportController>
        [HttpPost("AddNew")]
        public async Task<IActionResult> Post([FromBody] AddImportRequest import
           )
        {
            var addImport = new Import
            {
                ImportDate = import.ImportDate,
                Quantity = import.Quantity,
                QuantityInStock = import.QuantityInStock,
            };
            await _context.Imports.AddAsync(addImport);
            await _context.SaveChangesAsync();
            return Ok(addImport);
        }

        // PUT api/<ImportController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateImportRequest import)
        {
            var getImport = _context.Imports.SingleOrDefault(o => o.ImportId == id);
            if (getImport == null)
            {
                return BadRequest();
            }
            getImport.ImportDate = import.ImportDate;
            getImport.Quantity = import.Quantity;

            _context.Update(getImport);
            await _context.SaveChangesAsync();
            return Ok(getImport);
        }

        // DELETE api/<ImportController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var getImport = _context.Imports.SingleOrDefault(o => o.ImportId == id);
            if (getImport == null)
            {
                return BadRequest();
            }
            _context.Remove(getImport);
            _context.SaveChanges();
            return Ok("xoa thanh cong");
        }
    }
}
