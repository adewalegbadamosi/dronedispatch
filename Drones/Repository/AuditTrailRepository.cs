using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuditTrailRepository(
            ApplicationContext _context,
            IConfiguration _configuration,
            IMapper mapper)
        {
            context = _context;

            configuration = _configuration;
            _mapper = mapper;
        }

        public void AddAuditTrail(AuditTrail model)
        {         
            var audit = _mapper.Map<Audit>(model);

            context.Audits.Add(audit);
        }

        public  IEnumerable<AuditTrail> CheckAllDronesBatteryLevelLog()
        {   

            var batteryLevelLog = context.Audits.ToList();

            return _mapper.Map<List<AuditTrail>>(batteryLevelLog);
        }

        public IEnumerable<AuditTrail> CheckDroneBatteryLevelLog(int droneId)
        {
            var batteryLevelLog = context.Audits.Where(x => x.DroneId == droneId).ToList();

            return _mapper.Map<List<AuditTrail>>(batteryLevelLog);
        }
    }
}
