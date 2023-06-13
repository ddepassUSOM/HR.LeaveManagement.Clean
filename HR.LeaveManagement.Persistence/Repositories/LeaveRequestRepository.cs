using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;

namespace HR.LeaveManagement.Persistence.Repositories
{
	public class LeaveRequestRepository : GenericRespository<LeaveRequest>, ILeaveRequestRepository
	{
		public LeaveRequestRepository(HrDatabaseContext context) : base(context)
		{
		}
				
	}
}
