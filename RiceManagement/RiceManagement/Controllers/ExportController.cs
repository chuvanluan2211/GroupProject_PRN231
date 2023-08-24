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
        [HttpGet("GetLast")]
        public async Task<ActionResult<Export>> GetLast()
        {
            var getlist = _context.Exports.OrderByDescending(i => i.ExprotId).Take(1);
                
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
                    Quantity = 0,
                };
                await _context.Exports.AddAsync(addExport);
                
                await _context.SaveChangesAsync();
                return Ok(addExport);
            
        }
        [HttpPost("ExportDetail")]
        public async Task<IActionResult> PostEx([FromBody] AddExportDetailRequest export)

        {
            var getRice = _context.Rice.SingleOrDefault(i => i.RiceId == export.RiceId);
            var getExport = _context.Exports.SingleOrDefault(i => i.ExprotId == export.ExportId);
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
                    getExport.Quantity = export.Quantity;
                    getRice.QuantityInStock -= export.Quantity;
                    _context.Rice.Update(getRice);
                    _context.Imports.Update(getImport);
                    _context.Exports.Update(getExport);

                    await _context.ExportDetails.AddAsync(addExport);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, "add thanh cong");
                    break;
                }
                
                else
                {

                    var addExport = new ExportDetail
                    {
                        ExportId = export.ExportId,
                        ImportId = getImportDetail[i].ImportId,
                        RiceId = export.RiceId,
                        Quantity = export.Quantity,
                    };
                    var getImport = _context.Imports.SingleOrDefault(i => i.ImportId == addExport.ImportId);
                    var getNextImport = _context.Imports.SingleOrDefault(i => i.ImportId == addExport.ImportId +1);
                    int getImport1 = _context.ImportRiceDetails
                        .Where(i => i.ImportId == addExport.ImportId && i.RiceId == addExport.RiceId )
                        .Sum(o => o.Quantity).Value;

                    int lastUnit = (int)(addExport.Quantity - getImportDetail[i].Quantity);

                    getImport.QuantityInStock -= getImport1;
                    getNextImport.QuantityInStock = getNextImport.QuantityInStock -lastUnit
                       ;

                    getRice.QuantityInStock -= export.Quantity;
                    getExport.Quantity = export.Quantity;
                    _context.Exports.Update(getExport);

                    _context.Rice.Update(getRice);
                    _context.Imports.Update(getImport);
                    _context.Imports.Update(getNextImport);

                    await _context.ExportDetails.AddAsync(addExport);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, "add thanh cong");
                    break;
                }
                



            }
            if (export.Quantity > total)
            {

                return StatusCode(400, "khong export qua gioi han: " + total);
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
            _context.ExportDetails.Update(getExportDetail);
            _context.Exports.Update(getEx);
            await _context.SaveChangesAsync();
            return StatusCode(200, "update thanh cong");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExport(int id)
        {
            var getEx =  _context.Exports.SingleOrDefault(i => i.ExprotId==id);
            if(getEx == null)
            {
                return BadRequest(" ko co export nay");
            }
            else
            {
                var getExportDetail =  _context.ExportDetails.Where(o => o.ExportId == id).ToList();

                _context.Exports.Remove(getEx);
                _context.ExportDetails.RemoveRange(getExportDetail);

                _context.SaveChanges();

                return Ok("xoa thanh cong");

            }
        }
    }
}
