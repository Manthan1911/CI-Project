using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;
using System.Runtime.CompilerServices;

namespace CI_Project.Services
{
	public class HomeService:IHomeService
	{
		private readonly IUnitOfWork _unitOfWork;

		public HomeService(IUnitOfWork unitOfWork) 
		{
			_unitOfWork= unitOfWork;
		}


	}
}
