using Drones.Context;
using Drones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Controllers
{
    [Route("api/v1/audit[controller]")]
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
        [Route("get-battery-levels-log")]
        public IActionResult GetBatteryLevelsLog()
        {
            var response = repo.CheckDroneBatteryLevelLog();

            return Ok(new
            {
                status = true,
                data = response
            });
        }
    }
}
