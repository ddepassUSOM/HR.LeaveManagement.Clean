﻿using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;

namespace HR.LeaveManagement.Persistence.Repositories
{
	public class LeaveAllocationRepository : GenericRespository<LeaveAllocation>, ILeaveAllocationRepository
	{
		public LeaveAllocationRepository(HrDatabaseContext context) : base(context)
		{
		}

	}
}
