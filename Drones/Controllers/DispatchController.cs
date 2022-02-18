using Drones.Context;
using Drones.Interfaces;
using Drones.Models;
using Drones.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Drones.Controllers
{
    [AllowAnonymous]
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
        public async Task<IActionResult> AddNewDrone([FromBody] DroneView model)
        {
            // Checks and validations 
            var serialNumberLength = model.serialNumber.Length;
            if (serialNumberLength > 100) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Maximum length of serial number is 100" });

            var weightLimit = model.weightLimit;
            if (weightLimit > 500) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Weight of Drone cannot be more than 500 (gr)" });

            var batteryCapacity = model.batteryCapacity;
            if (batteryCapacity > 100) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Battery Capacity in (%), cannot be more than 100" });
            if (batteryCapacity < 25 || batteryCapacity < 0) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Battery Capacity in (%), cannot be less than 25" });

            var modelCode = model.model;
            if (modelCode < 1 || modelCode > 4) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Invalid model! Models code range from 1 to 4" });

            var response = await repo.AddDrone(model);

            return Ok(new
            {
                status = true,                
                data = response
            });
            
                
        }

        [HttpGet]
        [Route("get-available-drones")]
        public IActionResult GetAvailableDrones()
        {
            var response = repo.GetAvailableDrones();

            return Ok(new
            {
                status = true,
                count = response.Count,
                data = response

            });
        }

        [HttpPost]
        [Route("load-medication")]
        public async Task<IActionResult> LoadMedications ([FromForm] LoadedMedications model)
        {
           
            if (model != null)
            {
                // Checks and validations 
                var allowedName = @"^[a-zA-Z0-9 _-]+$";
                var allowedCode = @"^([A-Z0-9_]+)$";

                if (!Regex.IsMatch(model.name, allowedName)) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Invalid!. Name may only contain letters, numbers, ‘-‘, ‘_’" });
                if (!Regex.IsMatch(model.code, allowedCode)) return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "Invalid!. Code may only contain upper case letters,underscore and numbers" });


                if (model.imageData.Length > 0)
                {
                    // load drone with medication
                    var message = await repo.LoadingDroneWithMedication(model);
                        
                        // check if not null and return success message
                    if ( message != null )
                        return Ok(new
                        {
                            status = true,
                            message = message
                        });
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Error loading medication" });
        }

        [HttpGet]
        [Route("check-loaded-medication-for-a-drone/{droneId}")]
        public IActionResult GetLoadedMedicationByDrone(int droneId)
        {
            var response = repo.CheckLoadedMedicationsForADrone(droneId);

            if (response != null)
            {
                return Ok(new
                {
                    status = true,
                    count = response.Count,
                    data = response

                });
            }
           

            return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Server Error" });

        }


        [HttpGet]
        [Route("check-drone-battery/{droneId}")]
        public IActionResult GetBatteryLevelByDrone(int droneId)
        {
            var response = repo.CheckBatteryLevelOfADrone(droneId);

            if (response == 0) return Ok(new { status = false, message = "This drone has not been registered on our fleet" });

            return Ok(new
            {
                status = true,                
                batteryLevel = response

            });

    

        }


        [HttpGet]
        [Route("get-all-drones")]
        public IActionResult GetAllDrones()
        {
            var response = repo.GetAllDrones();

            return Ok(new
            {
                status = true,
                count = response.Count,
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
