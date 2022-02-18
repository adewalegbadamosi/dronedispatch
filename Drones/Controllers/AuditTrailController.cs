using Drones.Context;
using Drones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuditTrailController : ControllerBase
    {

       
        private readonly IAuditTrailRepository repo;
        private readonly ApplicationContext context;

        public AuditTrailController(IAuditTrailRepository auditTrailrepository, ApplicationContext _context)
        {

            repo = auditTrailrepository;
            context = _context;
        }


        [HttpGet]
        [Route("check-audit-trail")]
        public IActionResult GetAllBatteryLevelsLog()
        {
            var response = repo.CheckAllDronesBatteryLevelLog();

            return Ok(new
            {
                status = true,
                count = response.Count(),
                data = response
            }); ;
        }

        //[HttpGet]
        //[Route("check-drone-battery-levels/{droneId}")]
        //public IActionResult GetBatteryLevelLog(int droneId)
        //{
        //    var response = repo.CheckDroneBatteryLevelLog(droneId);

        //    return Ok(new
        //    {
        //        status = true,
        //        count = response.Count(),
        //        data = response
        //    }); ;
        //}
    }
}
