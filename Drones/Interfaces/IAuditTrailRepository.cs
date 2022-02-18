using Drones.Context;
using Drones.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Interfaces
{
    public interface IAuditTrailRepository
    {
        void AddAuditTrail(AuditTrail model);
        IEnumerable<AuditTrail> CheckDroneBatteryLevelLog(int droneId);
        IEnumerable<AuditTrail> CheckAllDronesBatteryLevelLog();
    }
}
