using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;

namespace CI_Project.Services
{
	public class VolunteeringMissionService:IVolunteeringMissionService
	{
		private readonly IUnitOfWork _unitOfWork;

		public VolunteeringMissionService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
	}
}
