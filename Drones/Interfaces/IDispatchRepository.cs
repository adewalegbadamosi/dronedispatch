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
        Task<string> LoadingDroneWithMedication(LoadedMedications model);
        List<MedicationReturnDTO> CheckLoadedMedicationsForADrone(int droneId);
        double CheckBatteryLevelOfADrone(int droneId);
        List<DroneDTO> GetAvailableDrones();
        List<DroneDTO> GetAllDrones();
        Task<string> AddDefaultData();
        string RechargeDrone(int droneId);
        DroneView RemoveDrone(int droneId);

    }
}
