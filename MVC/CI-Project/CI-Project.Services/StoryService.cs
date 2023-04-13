using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;

namespace CI_Project.Services
{
	public class StoryService:IStoryService
	{
		private readonly IUnitOfWork _unitOfWork;

		public StoryService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
	}
}
