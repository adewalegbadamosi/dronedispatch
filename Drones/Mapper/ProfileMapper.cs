using System;
using AutoMapper;
using Drones.Models;
using Drones.ViewModels;
using Drones.ViewModels.DTOs;
using Microsoft.Extensions.Configuration;

namespace Drones.Mapper
{
	public class ProfileMapper : Profile
	{
        public ProfileMapper ()
		{			
            // source to target
            CreateMap<Drone, DroneView>()
                .ForMember(dest => dest.serialNumber, opt => opt.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.batteryCapacity, opt => opt.MapFrom(src => src.BatteryCapacity));
                //.ForMember(dest => dest.batteryCapacity, opt => opt.Ignore())
            CreateMap<Drone, DroneDTO>();
            CreateMap<Audit, AuditTrail>()
                .ForMember(dest => dest.task, opt => opt.MapFrom(src => src.CurrentTask)).ReverseMap();


        }
    }
}

