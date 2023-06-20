﻿using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.MappingProfiles
{
	public class LeaveAllocationProfile : Profile
	{
        public LeaveAllocationProfile()
        {
			CreateMap<LeaveAllocationDto, LeaveAllocation>().ReverseMap();
			CreateMap<LeaveAllocation, LeaveAllocationDetailsDto>();
			CreateMap<CreateLeaveAllocationCommand, LeaveAllocation>();
			CreateMap<UpdateLeaveAllocationCommand, LeaveAllocation>();
		}
    }
}
