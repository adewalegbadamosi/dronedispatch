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
        private readonly IAuditTrailRepository auditTrail;

        public DispatchRepository(ApplicationContext _context, IConfiguration _configuration, IAuditTrailRepository auditTrailRepository)
        {
            context = _context;
            configuration = _configuration;
            auditTrail = auditTrailRepository;
        }

        public async Task<string> AddDrone(DroneViewModel model)
        {
            //Check if max fleet of 10 drones has been reached
            if (await context.Drones.FindAsync(10) != null) return "New drone cannot be added, maximum fleet of 10 drones has been reached";
                        
            var newDrone = new Drone
            {
                SerialNumber = model.serialNumber,
                Model = model.model,
                WeightLimit = model.weightLimit,
                BatteryCapacity = model.batteryCapacity,
                State = model.state,
                BatteryLevel = model.batteryCapacity
            };

            await context.Drones.AddAsync(newDrone);

            if (await context.SaveChangesAsync() > 0)
            {
                var audit = new AuditViewModel
                {
                    auditType = "Battery Level Check",
                    task = "Register new Drone",
                    detail = $"Created new drone with serial number { model.serialNumber} and battery level: {model.batteryCapacity}"
                };

                await this.auditTrail.AddAuditTrail(audit);

                return "New Drone Successfully registered";
            }

            return "Drone registration failed";

        }

        public  List<DroneDTO> GetAllDrones()
        {

            var drones = context.Drones.Select(u =>
             new DroneDTO
             {
                 droneId = u.DroneId,
                 serialNumber = u.SerialNumber,
                 weightLimit = u.WeightLimit,
                 batteryCapacity = u.BatteryCapacity,
                 model = u.Model,
                 modelName = Convert.ToString((DroneModels)u.Model),
                 state = u.State,
                 stateName = Convert.ToString((DroneState)u.State),
                 batteryLevel = u.BatteryLevel

             }).ToList();            

            return drones;
        }

        public List<DroneDTO> checkDroneBattery(int droneId)
        {

            var drones = context.Drones.Select(u =>
             new DroneDTO
             {
                 droneId = u.DroneId,
                 serialNumber = u.SerialNumber,
                 weightLimit = u.WeightLimit,
                 batteryCapacity = u.BatteryCapacity,
                 model = u.Model,
                 modelName = Convert.ToString((DroneModels)u.Model),
                 state = u.State,
                 stateName = Convert.ToString((DroneState)u.State),
                 batteryLevel = u.BatteryLevel

             }).ToList();

            return drones;
        }
        public async Task<string> AddDefaultData()
        {
            //Check if database has been initialized
            var dataExist = await context.Drones.FindAsync(1);
            if (dataExist != null) return "Database has already been initialized.";    


            var defaultData = new List<Drone>
            {
                new Drone { SerialNumber = "3477Yu48588443", Model = 1, WeightLimit = 200,  BatteryCapacity = 80, State = 0,BatteryLevel =80 },
                new Drone { SerialNumber = "12UYu48588443", Model = 4, WeightLimit = 450, BatteryCapacity = 95,  State = 0 , BatteryLevel = 95},
                new Drone { SerialNumber = "3468PyYu48588443", Model = 1, WeightLimit = 200,  BatteryCapacity = 98, State = 0 , BatteryLevel = 98},
                new Drone { SerialNumber = "1092UYu4858843", Model = 4, WeightLimit = 450, BatteryCapacity = 90,  State = 0, BatteryLevel = 90 },
                new Drone { SerialNumber = "98477YITu48588443", Model = 1, WeightLimit = 200,  BatteryCapacity = 80, State = 0 , BatteryLevel = 89 },
                new Drone { SerialNumber = "Q12UYu48588443973", Model = 4, WeightLimit = 450, BatteryCapacity = 95,  State = 0 , BatteryLevel = 95}
            };       
            
            

            foreach(Drone item in defaultData)
            {
               await context.Drones.AddAsync(item);

                var audit = new AuditViewModel
                {
                    auditType = "Battery Level Check",
                    task = "Register new Drone",
                    detail = $"Created new drone with serial number { item.SerialNumber} and battery level: {item.BatteryCapacity}"
                };

                await this.auditTrail.AddAuditTrail(audit);
            }     

            if (await context.SaveChangesAsync() > 0) return "Database successfully Initiated";

            return "Database initiation failed";

        }
    }
}
