using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
	public class Admin : Controller
	{

		private readonly IUserRepository _userRepository;
		private readonly IUnitOfService _unitOfService;
		private readonly ICmsRepository _cmsRepository;
		public Admin(IUserRepository userRepository, IUnitOfService unitOfService, ICmsRepository cmsRepository)
		{
			_userRepository = userRepository;
			_unitOfService = unitOfService;
			_cmsRepository = cmsRepository;
		}

		public IActionResult Index()
		{
			return View();
		}


		// ----------------- User -------------------
		public IActionResult GetUserPartial()
		{
			var usersList = _userRepository.getAllUsers();

			List<UserModel> usersVmList = new List<UserModel>();

			foreach (var user in usersList)
			{
				usersVmList.Add(ConvertUserToUserModel(user));
			}
			return PartialView("_AdminUserPartial", usersVmList);
		}

		public IActionResult GetAddUserPartial()
		{
			UserModel userVm = new();
			return PartialView("_AddUserPartial", userVm);
		}

		[HttpPost]
		public IActionResult SaveUser(UserModel userVm)
		{

			try
			{
				User user = new User()
				{
					FirstName = userVm.FirstName,
					LastName = userVm.LastName,
					PhoneNumber = userVm.PhoneNo,
					Password = _unitOfService.Password.Encode(userVm.Password),
					Email = userVm.EmailId,
					EmployeeId = userVm.EmployeeId,
					Department = userVm.Department,
					Status = true,
					CreatedAt = DateTime.Now,
				};

				_userRepository.addUser(user);
				return Ok(200);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return NoContent();
		}

		public IActionResult GetEditUserPartial(long userId)
		{
			User user = _userRepository.findUser(userId);
			return PartialView("_EditUserPartial", ConvertUserToUserModel(user));
		}

		public IActionResult EditUser(UserModel userVm)
		{
			try
			{
				User user = _userRepository.findUser(userVm.UserId);


				user.FirstName = userVm.FirstName;
				user.LastName = userVm.LastName;
				user.EmployeeId = userVm.EmployeeId;
				user.Department = userVm.Department;
				user.CreatedAt = DateTime.Now;

				_userRepository.updateUser(user);
				return Ok(200);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return NoContent();
		}

		[HttpDelete]
		public IActionResult DeleteUser(long userId)
		{
			try
			{
				User user = _userRepository.findUser(userId);

				user.Status = false;
				user.DeletedAt = DateTime.Now;

				_userRepository.updateUser(user);
				return Ok(200);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return NoContent();
		}

		public IActionResult RestoreUser(long userId)
		{
			try
			{
				User user = _userRepository.findUser(userId);

				user.Status = true;
				user.DeletedAt = null;

				_userRepository.updateUser(user);
				return Ok(200);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return NoContent();
		}

		public UserModel ConvertUserToUserModel(User user)
		{
			UserModel userVm = new()
			{
				UserId = user.UserId,
				EmailId = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				EmployeeId = user.EmployeeId,
				Department = user.Department,
				Password = _unitOfService.Password.Decode(user.Password),
				PhoneNo = user.PhoneNumber,
				Status = user.Status,
			};
			return userVm;
		}


		//------------------ CMS ----------------
		public IActionResult GetCmsPartial()
		{
			List<CmsPage> cmsPages = _cmsRepository.GetAll();
			List<CmsModel> cmsPagesVm = new List<CmsModel>();

			foreach (CmsPage cmsPage in cmsPages)
			{
				cmsPagesVm.Add(convertCmsPageToCmsPageVm(cmsPage));
			}
			return PartialView("_AdminCmsPartial", cmsPagesVm);
		}

		public IActionResult GetAddCmsPartial()
		{
			CmsModel cmsModelcmsModel = new();
			return PartialView("_AddCmsPartial", cmsModelcmsModel);
		}

		public IActionResult AddCmsPage(string title,string description ,string slug,bool status )
		{
			try
			{
				CmsPage cmsPage = new()
				{
					Title = title,
					Description = description,
					Slug = slug,
					Status = status,
					CreatedAt = DateTime.Now,
				};
				_cmsRepository.addCmsPage(cmsPage);
				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult GetEditCmsPartial(long cmsPageId)
		{
			CmsPage cmsPage = _cmsRepository.findCmsPageById(cmsPageId);
			return PartialView("_EditCmsPartial", convertCmsPageToCmsPageVm(cmsPage));
		}

		public IActionResult EditCmsPage(long id,string title, string description, string slug, bool status)
		{
			try
			{
				CmsPage cmsPage = _cmsRepository.findCmsPageById(id);

				cmsPage.Title = title;
				cmsPage.Description = description;
				cmsPage.Slug = slug;
				cmsPage.Status = status;
				cmsPage.UpdatedAt = DateTime.Now;

				_cmsRepository.updateCmsPage(cmsPage);

				return NoContent();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult HardDeleteCmsPage(long cmsPageId)
		{
			try
			{
				CmsPage cmsPage = _cmsRepository.findCmsPageById(cmsPageId);
				_cmsRepository.deleteCmsPage(cmsPage);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult SoftDeleteCmsPage(long cmsPageId)
		{
			try
			{
				CmsPage cmsPage = _cmsRepository.findCmsPageById(cmsPageId);
				cmsPage.DeletedAt = DateTime.Now;
				cmsPage.Status = false;
				_cmsRepository.updateCmsPage(cmsPage);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}
		
		public IActionResult RestoreCmsPage(long cmsPageId)
		{
			try
			{
				CmsPage cmsPage = _cmsRepository.findCmsPageById(cmsPageId);
				cmsPage.Status = true;
				cmsPage.DeletedAt = null;
				_cmsRepository.updateCmsPage(cmsPage);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		private CmsModel convertCmsPageToCmsPageVm(CmsPage cmsPage)
		{
			CmsModel cmsPageVm = new()
			{
				CmsPageId = cmsPage.CmsPageId,
				Title = cmsPage.Title,
				Description = cmsPage.Description,
				Slug = cmsPage.Slug,
				Status = cmsPage.Status,
			};
			return cmsPageVm;
		}

		// -------------------- Skill --------------------------
	}
}
