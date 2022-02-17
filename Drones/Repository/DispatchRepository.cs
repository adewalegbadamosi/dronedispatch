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

        public async Task<string> AddDrone(DroneView model)
        {
            //Check if max fleet of 10 drones has been reached
            if (await context.Drones.FindAsync(10) != null) return "New drone cannot be added, maximum fleet of 10 drones has been reached";
                        
            var newDrone = new Drone
            {
                SerialNumber = model.serialNumber,
                Model = model.model,
                WeightLimit = model.weightLimit,
                BatteryCapacity = model.batteryCapacity,
                State = model.batteryCapacity > 25? model.state : Convert.ToInt32(DroneState.Idle),
                BatteryLevel = model.batteryCapacity
            };

            await context.Drones.AddAsync(newDrone);

          
            var audit = new AuditViewModel
            {
                auditType = "Battery Level Check",
                task = "Register new Drone",
                detail = $"Created new drone with serial number { model.serialNumber} and battery level: {model.batteryCapacity}"
            };

                this.auditTrail.AddAuditTrail(audit);

            if (await context.SaveChangesAsync() > 0) return "New Drone Successfully registered";
            

            return "Drone registration failed";

        }

        public async Task<bool> CheckBatteryLevelOfADrone(int droneId)
        {
            
            Drone targetDrone = await context.Drones.FindAsync(droneId);

            //Check if drone is in loading state and battery level greater than 25%
            //var isLoadingState = targetDrone.State;
            var batteryLevel = Convert.ToInt32(targetDrone.BatteryLevel);

            if (batteryLevel > 25 ) return true;

            return false;

        }
        
        public async Task<string> LoadingDroneWithMedication(LoadedMedications model)
        {
            //Check if drone can be loaded
            var canBeLoaded = CheckBatteryLevelOfADrone(model.droneId);           


            if (Convert.ToBoolean(canBeLoaded) == false) return "Selected drone is not available for loading";

            Drone targetDrone = await context.Drones.FindAsync(model.droneId);
            if (targetDrone.WeightLimit < model.weight) return "Overload! Drone can not carry the medication.";


            var loadMediication = new Medication
            {
                Name = model.name,
                Weight = model.weight,
                Code = model.code,
                DroneId = model.droneId
            };

            await context.Medications.AddAsync(loadMediication);
            await context.SaveChangesAsync();

            var updatedBatteryLevel = UpdateDroneStatusAfterLoading(model.droneId);

            var audit = new AuditViewModel
            {
                auditType = "Battery Level Check",
                task = "Load Drone with Medication",
                detail = $"Loaded {model.name} with code number {model.code} into a drone with Unique Id: { model.droneId} , battery level {updatedBatteryLevel }"
            };

            this.auditTrail.AddAuditTrail(audit);

            if (await context.SaveChangesAsync() > 0) return $"Medications {model.name} has been loaded successfully";


            return "Medication loading failed";

        }

        public List<LoadedMedications> CheckLoadedMedicationsForADrone(int droneId)
        {

            var loadedMedications = context.Medications
                .Where(x => x.DroneId == droneId)
                .Select(u => new LoadedMedications
                 {
                     droneId = u.DroneId,
                     name = u.Name,
                     code = u.Code,
                     weight = u.Weight

                 }).ToList();

            return loadedMedications;
        }

        public List<DroneDTO> GetAvailableDrones()
        {

            var availableDrones = context.Drones
                .Where(a => a.State == 1 && a.BatteryLevel > 25)
                .Select(u =>  new DroneDTO
                 {
                     droneId = u.DroneId,
                     serialNumber = u.SerialNumber,
                     weightLimit = u.WeightLimit,
                     batteryCapacity = u.BatteryCapacity,
                     model = u.Model,
                     modelName = Convert.ToString((DroneModels)u.Model),
                     state = u.State,
                     stateName = Convert.ToString((DroneState)u.State),
                     //batteryLevel = u.BatteryLevel

                 }).ToList();

            return availableDrones;
        }

        public List<DroneDTO> GetAllDrones()
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
                 //batteryLevel = u.BatteryLevel

             }).ToList();

            return drones;
        }
        public double UpdateDroneStatusAfterLoading(int droneId)
        {

            Drone targetDrone = context.Drones.Find(droneId);
            var updatedBatteryLevel = targetDrone.BatteryLevel - 25;
            targetDrone.BatteryLevel = updatedBatteryLevel;  //Drop battery level by 30% after every loading
            targetDrone.State = updatedBatteryLevel < 25 ? Convert.ToInt32(DroneState.Idle) : Convert.ToInt32(DroneState.Loaded);

            context.SaveChanges();

            return updatedBatteryLevel;

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
                    auditType = "battery level check",
                    task = "register new drone",
                    detail = $"created new drone with serial number { item.SerialNumber} and battery level: {item.BatteryCapacity}"
                };

                 this.auditTrail.AddAuditTrail(audit);
            }     

            if (await context.SaveChangesAsync() > 0) return "Database successfully Initiated";

            return "Database initiation failed";

        }

       
    }
}
