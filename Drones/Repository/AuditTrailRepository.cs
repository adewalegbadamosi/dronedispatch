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

        public void AddAuditTrail(AuditViewModel model)
        {
            var audit = new Audit
            {
                AuditType = model.auditType,
                CurrentTask = model.task,
                Detail = model.detail
            };

            context.Audits.Add(audit);

        }

        public  IEnumerable<AuditViewModel> CheckDroneBatteryLevelLog()
        {
   
             var batteryLevelLog = context.Audits.Select(u =>
             new AuditViewModel
             {
                 auditType = u.AuditType,
                 task = u.CurrentTask,
                 detail = u.Detail,
                 timeCreated = u.DateTimeCreated

             }).ToList();
            
            

            return batteryLevelLog;

        }

    }
}
