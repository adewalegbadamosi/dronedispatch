using Drones.Context;
using Drones.Interfaces;
using Drones.Models;
using Drones.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Repository
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly ApplicationContext context;

        private readonly IConfiguration configuration;

        public AuditTrailRepository(ApplicationContext _context, IConfiguration _configuration)
        {
            context = _context;

            configuration = _configuration;
        }

        public void AddAuditTrail(AuditTrail model)
        {
            var audit = new Audit
            {
                AuditType = model.auditType,
                CurrentTask = model.task,
                DroneId = model.droneId,
                DroneBatteryLevel = model.droneBatteryLevel,
                Detail = model.detail
            };

            context.Audits.Add(audit);

        }

        public  IEnumerable<AuditTrail> CheckAllDronesBatteryLevelLog()
        {
   
             var batteryLevelLog = context.Audits.Select(u =>
             new AuditTrail
             {
                 auditType = u.AuditType,
                 task = u.CurrentTask,
                 detail = u.Detail,
                 droneId = u.DroneId,
                 droneBatteryLevel = u.DroneBatteryLevel,
                 timeCreated = u.DateTimeCreated

             }).ToList();
            
            

            return batteryLevelLog;

        }

        public IEnumerable<AuditTrail> CheckDroneBatteryLevelLog(int droneId)
        {

            var batteryLevelLog = context.Audits.Where(x => x.DroneId == droneId).Select(u =>
            new AuditTrail
            {
                auditType = u.AuditType,
                task = u.CurrentTask,
                detail = u.Detail,
                droneId = u.DroneId,
                droneBatteryLevel = u.DroneBatteryLevel,
                timeCreated = u.DateTimeCreated

            }).ToList();



            return batteryLevelLog;

        }

    }
}
