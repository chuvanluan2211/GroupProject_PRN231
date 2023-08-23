using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiceManagement.DTOs;
using RiceManagement.Models;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RiceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly ProjectBl5Context _context;
        public ExportController(ProjectBl5Context context)
        {
            _context = context;
        }
        // GET: api/<ExportController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Export>>> Get()
        {
            var getlist = _context.Exports.ToList();
            return Ok(getlist);
        }

        // GET api/<ExportController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ExportDetailResponse>>> GetList(int id)
        {
            var getlist = _context.ExportDetails.Include(i => i.Rice).Where(o => o.ExportId == id)
                .Select(a => new ExportDetailResponse
                {
                    ExportId = a.ExportId,
                    ExportDetailId = a.ExportDetailId,
                    ImportId = a.ImportId,
                    Quantity = a.Quantity,
                    RiceName = a.Rice.Name
                }).ToList();
            return Ok(getlist);
        }

        // POST api/<ExportController>
        [HttpPost]
        public async Task<IActionResult> Post( [FromBody] AddExportRequest export)

        {
            //var getImport = _context.Imports.SingleOrDefault();
            //int total = (int)(getImport.QuantityInStock - export.Quantity);
           
                var addExport = new Export
                {
                    ExportDate = export.ExportDate,
                    Quantity = export.Quantity,
                };
                await _context.Exports.AddAsync(addExport);
                
                await _context.SaveChangesAsync();
                return Ok(addExport);
            
        }
        [HttpPost("ExportDetail")]
        public async Task<IActionResult> PostEx([FromBody] AddExportDetailRequest export)

        {
            var getRice = _context.Rice.SingleOrDefault(i => i.RiceId == export.RiceId);
            var getImportDetail = _context.ImportRiceDetails.Where(o => o.RiceId == export.RiceId ).ToList();
            int total = getImportDetail.Sum(i => i.Quantity).Value;
            for (int i = 0; i <  getImportDetail.Count ; i++)
            {
               
                if (export.Quantity <= getImportDetail[i].Quantity)
                    {
                    var addExport = new ExportDetail
                    {
                         ExportId = export.ExportId,
                         ImportId = getImportDetail[i].ImportId,
                        RiceId = export.RiceId,
                        Quantity = export.Quantity,
                    };
                    var getImport = _context.Imports.SingleOrDefault(i => i.ImportId == addExport.ImportId);
                    getImport.QuantityInStock -= export.Quantity;

                    getRice.QuantityInStock -= export.Quantity;
                    _context.Rice.Update(getRice);
                    _context.Imports.Update(getImport);

                    await _context.ExportDetails.AddAsync(addExport);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, "add thanh cong");
                    break;
                }
                if(export.Quantity > total)
                {
                    return StatusCode(400, "khong export qua gioi han: " + total);
                }
                



            }
            return StatusCode(200);


        }
        
        [HttpPut("ImportDetail/{id}/{id2}/{id3}")]
        public async Task<IActionResult> PutImport(int id, int id2,int id3, [FromBody] UpdateExportRequest export)
        {
            var getExportDetail = _context.ExportDetails.SingleOrDefault(o => o.ImportId == id2 &&
            o.RiceId == id3 && o.ExportId == id);
            var getEx = _context.Exports.SingleOrDefault(i => i.ExprotId == id);
            int sum = (int)_context.ExportDetails.Where(o => o.ExportId == id ).Sum(i => i.Quantity);
            if (getExportDetail == null)
            {
                return BadRequest();
            }
            getExportDetail.Quantity = export.Quantity;
            getEx.Quantity = sum;
            getEx.ExportDate = export.ExportDate;
            _context.Update(getExportDetail);
            _context.Update(getEx);
            await _context.SaveChangesAsync();
            return StatusCode(200, "update thanh cong");
        }
    }
}
