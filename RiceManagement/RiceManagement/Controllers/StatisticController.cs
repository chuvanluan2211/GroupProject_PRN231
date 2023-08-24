using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiceManagement.DTO;
using RiceManagement.Models;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        [HttpGet("{month}/{year}")]
        //[Authorize(Policy = "AdminRolePolicy")]
        public ActionResult GetAll (string month,string year) {
            int parsedMonth = int.Parse(month);
            int parsedYear = int.Parse(year);

            var results = from import in _context.Imports
                          join exportDetail in _context.ExportDetails on import.ImportId equals exportDetail.ImportId
                          join export in _context.Exports on exportDetail.ExportId equals export.ExprotId
                          where export.ExportDate.Value.Month == parsedMonth
                              && export.ExportDate.Value.Year == parsedYear
                              && import.ImportDate.Value.Month == parsedMonth
                              && import.ImportDate.Value.Year == parsedYear
                          select new Statistic
                          {
                              ImportId = import.ImportId,
                              ExportId = export.ExprotId,
                              Date = import.ImportDate.ToString(),
                              ImportQuantity = import.Quantity,
                              QuantityInStock = import.QuantityInStock,
                              ExportQuantity = export.Quantity
                          };

            if (results == null)
            {
                return NotFound();
            }
            return Ok(results);
        }

        [HttpGet("Details/{iID}/{eID}")]
        
        public IActionResult GetDetail(int iID,int eID)
        {
            var results = _context.ExportDetails.Include(x => x.Rice).Where(x => x.ImportId == iID && x.ExportId == eID).ToList();
            if (results == null)
            {
                return NotFound();
            }
            var rs = results.Select(x => new StatisticDetails
            {
                RiceName = x.Rice.Name,
                Quantity = x.Quantity
            });

           
            return Ok(rs);
        }

    }
}
