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
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ImportRiceDetailResponse>>> GetDetail(int id)
        {
            var getlist = _context.ImportRiceDetails.Where(i => i.ImportId == id)
                .Include(i => i.Rice).Select(o => new ImportRiceDetailResponse 
                {
                    ImportDetailId = o.ImportDetailId,
                    ImportId = o.ImportId,
                    Quantity = o.Quantity,
                    RiceName = o.Rice.Name
                }).ToList();
            return Ok(getlist);
        }
        

        // POST api/<ImportController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddImportRequest import)
           
        {
            var addImport = new Import
            {
                ImportDate = import.ImportDate,
                Quantity = 0,
                QuantityInStock = import.QuantityInStock,
            };
            await _context.Imports.AddAsync(addImport);
            await _context.SaveChangesAsync();
            return Ok(addImport);
        }
        [HttpPost("ImportDetail")]
        public async Task<IActionResult> PostImport( [FromBody] AddImportRiceDetailRequest import)

        {
            var getRice = _context.Rice.SingleOrDefault(i => i.RiceId == import.RiceId);
            var getImport = _context.Imports.SingleOrDefault(i => i.ImportId == import.ImportId);

            var addImport = new ImportRiceDetail
            {
                ImportId = import.ImportId,
                RiceId = import.RiceId,
                Quantity = import.Quantity,
            };
            getRice.QuantityInStock += import.Quantity;
            getImport.Quantity += import.Quantity;
            _context.Rice.Update(getRice);
            _context.Imports.Update(getImport);

            await _context.ImportRiceDetails.AddAsync(addImport);
            await _context.SaveChangesAsync();
            return Ok(addImport);
        }

       
        [HttpPut("ImportDetail/{id}/{id2}")]
        public async Task<IActionResult> PutImport(int id, int id2 , [FromBody] ImportRiceDetailRequest import)
        {
            var getImportDetail = _context.ImportRiceDetails.SingleOrDefault(o => o.ImportId == id && 
            o.RiceId == id2);
            var getImport = _context.Imports.SingleOrDefault(i => i.ImportId == id);
            int sum = (int)_context.ImportRiceDetails.Where(i => i.ImportId == id).Sum(o => o.Quantity);

            if (getImport == null)
            {
                return BadRequest();
            }
            getImportDetail.Quantity = import.Quantity;
            getImport.Quantity = sum ;
            getImport.ImportDate = import.ImportDate;
            _context.Update(getImport);
            _context.Update(getImportDetail);
            await _context.SaveChangesAsync();
            return StatusCode(200, "update thanh cong");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImport(int id)
        {
            var getEx = await _context.Imports.SingleAsync(i => i.ImportId == id);
            if (getEx == null)
            {
                return BadRequest(" ko co export nay");
            }
            else
            {
                var getExportDetail = _context.ImportRiceDetails.Where(o => o.ImportId == id).ToList();

                _context.ImportRiceDetails.RemoveRange(getExportDetail);
                _context.Imports.Remove(getEx);

                _context.SaveChanges();

                return Ok("xoa thanh cong");

            }
        }
    }
}
