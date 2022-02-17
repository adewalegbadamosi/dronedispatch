using Drones.ViewModels;
using Drones.ViewModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Interfaces
{
    public interface IDispatchRepository
    {
        Task<string> AddDrone(DroneView model);
        List<DroneDTO> GetAllDrones();

        Task<string> AddDefaultData();
        
    }
}
