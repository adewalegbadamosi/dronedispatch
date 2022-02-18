﻿using Drones.Context;
using Drones.Enums;
using Drones.Interfaces;
using Drones.Models;
using Drones.ViewModels;
using Drones.ViewModels.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
                State = (int)DroneState.Loading,
                //State = model.batteryCapacity > 25? model.state : Convert.ToInt32(DroneState.Idle),
                BatteryLevel = model.batteryCapacity
            };

            await context.Drones.AddAsync(newDrone);

          
            var audit = new AuditTrail
            {
                auditType = "Battery Level Check",
                task = "Register new Drone",
                droneId = context.Drones.LastOrDefault().DroneId + 1,
                droneBatteryLevel = model.batteryCapacity,
                detail = $"Created new drone with serial number { model.serialNumber} and battery level: {model.batteryCapacity}"
            };

                this.auditTrail.AddAuditTrail(audit);

            if (await context.SaveChangesAsync() > 0) return "New Drone Successfully registered";
            

            return "Drone registration failed";

        }

        public  double CheckBatteryLevelOfADrone(int droneId)
        {
            double batteryLevel;

            if (context.Medications.Count() < 1 || context.Medications.Where( x=> x.DroneId == droneId).Count() < 1)
            {
                batteryLevel = this.auditTrail.CheckDroneBatteryLevelLog(droneId).LastOrDefault().droneBatteryLevel;
                return batteryLevel;
            }

            //First update drone and medication statuses
             batteryLevel = this.UpdateDroneAndMedicationDeliveryStatus(droneId);
            
            //batteryLevel = this.auditTrail.CheckDroneBatteryLevelLog(droneId).Last().droneBatteryLevel;

            
            return batteryLevel;

        }

        public async Task<string> LoadingDroneWithMedication(LoadedMedications model)
        {
            Drone targetDrone = await context.Drones.FindAsync(model.droneId);

            //Check if drone can be loaded
            var canBeLoaded = CheckBatteryLevelOfADrone(model.droneId);


            if (canBeLoaded < 25 || targetDrone.State != (int)DroneState.Loading) return "Selected drone is not available for loading";


            if (targetDrone.WeightLimit < model.weight) return "Overload! Drone can not carry the medication.";

            var LoadedMedicationCount = context.Medications.Count();
            var loadMedication = new Medication();

            loadMedication.MedicationId = LoadedMedicationCount + 1;
            loadMedication.Name = model.name;
            loadMedication.Weight = model.weight;
            loadMedication.Code = model.code;
            loadMedication.DroneId = model.droneId;

            using (var target = new MemoryStream())
            {
                model.imageData.CopyTo(target);
                loadMedication.Image = target.ToArray();
            }


            await context.Medications.AddAsync(loadMedication);
            targetDrone.State = (int)DroneState.Loaded;
            var status = await context.SaveChangesAsync() > 0;

            if (status)
            {
                var audit = new AuditTrail
                {
                    auditType = "Battery Level Check",
                    task = "Load Drone with Medication",
                    droneId = model.droneId,
                    droneBatteryLevel = this.auditTrail.CheckDroneBatteryLevelLog(model.droneId).LastOrDefault().droneBatteryLevel,
                    detail = $"Loaded medication named: {model.name} with code number {model.code} into a drone with Id: { model.droneId}."
                };

                this.auditTrail.AddAuditTrail(audit);
                await context.SaveChangesAsync();

                return $"Medications named: {model.name} has been loaded successfully";
            }


            return "Medication loading failed";

        }

        //public async Task<string> LoadingDroneWithMedication(LoadedMedications model)
        //{
        //    Drone targetDrone = await context.Drones.FindAsync(model.droneId);

        //    //Check if drone can be loaded
        //    var canBeLoaded = CheckBatteryLevelOfADrone(model.droneId);


        //    if (canBeLoaded < 25 || targetDrone.State != (int)DroneState.Loading) return "Selected drone is not available for loading";


        //    if (targetDrone.WeightLimit < model.weight) return "Overload! Drone can not carry the medication.";

        //    var LoadedMedicationCount = context.Medications.Count();
        //    var loadMedication = new Medication();

        //    //loadMedication.MedicationId = LoadedMedicationCount + 1;
        //    loadMedication.Name = model.name;
        //    loadMedication.Weight = model.weight;
        //    loadMedication.Code = model.code;
        //    loadMedication.DroneId = model.droneId;






        //    await context.Medications.AddAsync(loadMedication);
        //    targetDrone.State = (int)DroneState.Loaded;
        //    var status = await context.SaveChangesAsync() > 0;

        //    if (status)
        //    {
        //        var audit = new AuditTrail
        //        {
        //            auditType = "Battery Level Check",
        //            task = "Load Drone with Medication",
        //            droneId = model.droneId,
        //            droneBatteryLevel = this.auditTrail.CheckDroneBatteryLevelLog(model.droneId).LastOrDefault().droneBatteryLevel,
        //            detail = $"Loaded medication named: {model.name} with code number {model.code} into a drone with Id: { model.droneId}."
        //        };

        //        this.auditTrail.AddAuditTrail(audit);
        //        await context.SaveChangesAsync();

        //        return $"Medications named: {model.name} has been loaded successfully";
        //    }


        //    return "Medication loading failed";

        //}

        public List<MedicationReturnDTO> CheckLoadedMedicationsForADrone(int droneId)
        {
            if( context.Medications.Count() < 1) return new List<MedicationReturnDTO>();

            // First update the status of the drones and medications
            var droneIds = context.Medications.Where(c => c.DeliveryStatus != (int)MedicationDeliveryStatus.Delivered).Select(x => x.DroneId);

            foreach (var id in droneIds)
            {
                this.UpdateDroneAndMedicationDeliveryStatus(id);
            }

            var loadedMedications = context.Medications
                .Where(x => x.DroneId == droneId)
                .Select(u => new MedicationReturnDTO
                 {
                     droneId = u.DroneId,
                     name = u.Name,
                     code = u.Code,
                     weight = u.Weight,
                     deliveryStatus = Convert.ToString((MedicationDeliveryStatus)u.DeliveryStatus),
                     dateTimeCreated = u.DateTimeCreated,
                     image = u.Image

                 }).ToList();

            return loadedMedications;
        }

        public List<DroneDTO> GetAvailableDrones()
        {
            // First update the ths status of the drones
          
            var droneIds = context.Medications.Where(c => c.DeliveryStatus != (int)MedicationDeliveryStatus.Delivered).Select(x => x.DroneId);

            foreach ( var id in droneIds)
            {
                this.UpdateDroneAndMedicationDeliveryStatus(id);
            }

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
                     batteryLevel = u.BatteryLevel

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
                 batteryLevel = u.BatteryLevel

             }).ToList();

            return drones;
        }
        public double UpdateDroneAndMedicationDeliveryStatus(int droneId)
        {
            //Get reference to targets
            //var droneStatus = this.auditTrail.CheckDroneBatteryLevelLog(droneId).LastOrDefault();
            Drone targetDrone = context.Drones.Find(droneId);
            Medication targetMedication = context.Medications.Where(x => x.DroneId == droneId).Last();

            //Update Drone status  and medication delivery status every 2 minutes
            var dateSpan = DateTime.Now.Subtract(targetMedication.DateTimeCreated);
            var dateSpanMinute = (double)(dateSpan.TotalDays * 24 * 60);

            

            if (targetDrone.BatteryLevel < 25)
            {
                targetDrone.State = (int)DroneState.Idle;
            }
            else if(targetDrone.State != (int)DroneState.Loading || targetMedication != null)
            {           

                if (dateSpanMinute >= 2 && dateSpanMinute < 4 && targetDrone.State != (int)DroneState.Delivering)
                {
                    targetDrone.State = (int)DroneState.Delivering;
                    targetDrone.BatteryLevel = targetDrone.BatteryLevel - 5;
                    targetMedication.DeliveryStatus = (int)MedicationDeliveryStatus.InTransit;
                }
                else if (dateSpanMinute >= 4 && dateSpanMinute < 6 && targetDrone.State !=(int)DroneState.Delivered)
                {
                    targetDrone.State = (int)DroneState.Delivered;
                    targetDrone.BatteryLevel = targetDrone.BatteryLevel - 10;
                    targetMedication.DeliveryStatus = (int)MedicationDeliveryStatus.Delivered;

                }
                else if (dateSpanMinute >= 6 && dateSpanMinute < 8 && targetDrone.State !=(int)DroneState.Returning)
                {
                    targetDrone.State = (int)DroneState.Returning;
                    targetDrone.BatteryLevel = targetDrone.BatteryLevel - 10;
                }
                else if (dateSpanMinute >= 8 && targetDrone.State == (int)DroneState.Returning)
                {
                    targetDrone.State = (int)DroneState.Loading;
                
                }
                else if (dateSpanMinute >= 8)
                {
                    targetDrone.State = (int)DroneState.Loading;
                    targetDrone.BatteryLevel = targetDrone.BatteryLevel - 25;
                }
            }


            var isTargetStatusUpdated = context.SaveChanges() > 0;

            if (isTargetStatusUpdated)
            {
                var audit = new AuditTrail
                {
                    auditType = "Battery Level Check",
                    task = "Update Drone and Medication delivery status",
                    droneId = droneId,
                    droneBatteryLevel = this.auditTrail.CheckDroneBatteryLevelLog(droneId).LastOrDefault().droneBatteryLevel,
                    detail = $"Updated Drone status and medication delivery status for Drone with Id {droneId} and medication with name {targetMedication.Name} ."
                };

                this.auditTrail.AddAuditTrail(audit);
                context.SaveChanges();

            }



            return targetDrone.BatteryLevel;
            

        }
        public async Task<string> AddDefaultData()
        {
            //Check if database has been initialized
            var dataExist = await context.Drones.FindAsync(1);
            if (dataExist != null) return "Database has already been initialized.";

            var count = 0;
            var defaultData = new List<Drone>
            {
                new Drone { SerialNumber = "3477Yu48588443", Model = 1, WeightLimit = 200,  BatteryCapacity = 80, State = 1,BatteryLevel =80 },
                new Drone { SerialNumber = "12UYu48588443", Model = 4, WeightLimit = 450, BatteryCapacity = 95,  State = 1 , BatteryLevel = 95},
                new Drone { SerialNumber = "3468PyYu48588443", Model = 1, WeightLimit = 200,  BatteryCapacity = 98, State = 1 , BatteryLevel = 98},
                new Drone { SerialNumber = "1092UYu4858843", Model = 4, WeightLimit = 450, BatteryCapacity = 90,  State = 1, BatteryLevel = 90 },
                new Drone { SerialNumber = "98477YITu48588443", Model = 1, WeightLimit = 200,  BatteryCapacity = 80, State = 1 , BatteryLevel = 89 },
                new Drone { SerialNumber = "Q12UYu48588443973", Model = 4, WeightLimit = 450, BatteryCapacity = 95,  State = 1 , BatteryLevel = 95}
            };        
            

            foreach(Drone item in defaultData)
            {
               await context.Drones.AddAsync(item);

                var audit = new AuditTrail
                {
                    auditType = "battery level check",
                    task = "register new drone",
                    droneId = ++count,
                    droneBatteryLevel = item.BatteryCapacity,
                    detail = $"created new drone with serial number { item.SerialNumber} and battery level: {item.BatteryCapacity}"
                };

                 this.auditTrail.AddAuditTrail(audit);
            }     

            if (await context.SaveChangesAsync() > 0) return "Database successfully Initiated";

            return "Database initiation failed";

        }

       
    }
}
