using Drones.Context;
using Drones.Interfaces;
using Drones.Models;
using Drones.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Controllers
{
    [Route("api/v1/drone[controller]")]
    [ApiController]
    public class DispatchController : ControllerBase
    {

        private readonly ILogger<DispatchController> _logger;       
        
        private readonly IDispatchRepository repo;

        public DispatchController(ILogger<DispatchController> logger, IDispatchRepository dispatchrepository)
        {
            _logger = logger;            
            repo = dispatchrepository;
        }

        [HttpPost]
        [Route("register-drone")]
        public async Task<IActionResult> AddNewDrone([FromBody] DroneViewModel model)
        {
            // Check the 
            var serialNumberLength = model.serialNumber.Length;
            if (serialNumberLength > 100) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Maximum length of serial number is 100" });

            var weightLimit = model.weightLimit;
            if (weightLimit > 500) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Weight of Drone cannot be more than 500 (gr)" });

            var batteryCapacity = model.batteryCapacity;
            if (batteryCapacity > 100) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Battery Capacity in (%), cannot be more than 100" });

            var response = await repo.AddDrone(model);

            return Ok(new
            {
                status = true,
                data = response
            });
        }

      
        [HttpGet]
        [Route("get-registered-drones")]
        public IActionResult GetAllDrones()
        {
            var response = repo.GetAllDrones();

            return Ok(new
            {
                status = true,
                data = response
            });
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> InitializeDb()

        {

            var response = await repo.AddDefaultData();

            return Ok(new
            {
                status = response

            }) ;
        }
}

}
