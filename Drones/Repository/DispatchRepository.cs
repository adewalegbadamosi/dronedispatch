using Drones.Context;
using Drones.Enums;
using Drones.Interfaces;
using Drones.Models;
using Drones.ViewModels;
using Drones.ViewModels.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Repository
{
    public class DispatchRepository : IDispatchRepository
    {

        private readonly ApplicationContext context;
        private readonly IConfiguration configuration;

        public DispatchRepository(ApplicationContext _context, IConfiguration _configuration)
        {
            context = _context;
            configuration = _configuration;
        }

        public async Task<string> AddDrone(DroneViewModel model)
        {
            
            var newDrone = new Drone
            {
                SerialNumber = model.serialNumber,
                Model = model.model,
                WeightLimit = model.weightLimit,
                BatteryCapacity = model.batteryCapacity,
                State = model.state
            };

            await context.Drones.AddAsync(newDrone);

            if (await context.SaveChangesAsync() > 0) return "New Drone Successfully registered";

            return "Drone registration failed";

        }

        public  List<DroneDTO> GetAllDrones()
        {

            var drones = context.Drones.Select(u =>
             new DroneDTO
             {
                 serialNumber = u.SerialNumber,
                 weightLimit = u.WeightLimit,
                 batteryCapacity = u.BatteryCapacity,
                 model = u.Model,
                 modelName = Convert.ToString((DroneModels)u.Model),
                 state = u.State,
                 stateName = Convert.ToString((DroneState)u.State)

             }).ToList();            

            return drones;
        }
        public async Task<string> AddDefaultData()
        {
            
            var firstDrone = new Drone
            {
                SerialNumber = "3477Yu48588443",
                Model = 1,
                WeightLimit = 200,
                BatteryCapacity = 80,
                State = 0
            };

            await context.Drones.AddAsync(firstDrone);

            var secondDrone = new Drone
            {
                SerialNumber = "12UYu48588443",
                Model = 4,
                WeightLimit = 450,
                BatteryCapacity = 95,
                State = 0
            };

            await context.Drones.AddAsync(secondDrone);

            if (await context.SaveChangesAsync() > 0) return "Database successfully Initiated";

            return "Database initiation failed";

        }
    }
}
