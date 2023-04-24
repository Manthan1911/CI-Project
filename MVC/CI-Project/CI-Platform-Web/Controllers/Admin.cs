using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
	public class Admin : Controller
	{
		private readonly IVolunteeringMissionRepository _volunteeringMissionRepository;
		private readonly IMissionThemeRepository _missionThemeRepository;
		private readonly IUsersSkill _usersSkill;
		private readonly IMissionApplication _missionsApplication;
		private readonly IMissionsSkills _missionsSkills;
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfService _unitOfService;
		private readonly ICmsRepository _cmsRepository;
		private readonly ISkillsRepository _skillRepository;
		public Admin(IVolunteeringMissionRepository volunteeringMissionRepository, IMissionThemeRepository missionThemeRepository, IMissionApplication missionApplication, IUsersSkill usersSkill, IMissionsSkills missionsSkills, IUserRepository userRepository, IUnitOfService unitOfService, ICmsRepository cmsRepository, ISkillsRepository skillRepository)
		{
			_volunteeringMissionRepository = volunteeringMissionRepository;
			_missionThemeRepository = missionThemeRepository;
			_missionsApplication = missionApplication;
			_usersSkill = usersSkill;
			_missionsSkills = missionsSkills;
			_userRepository = userRepository;
			_unitOfService = unitOfService;
			_cmsRepository = cmsRepository;
			_skillRepository = skillRepository;

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

		private UserModel ConvertUserToUserModel(User user)
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

		public IActionResult AddCmsPage(string title, string description, string slug, bool status)
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

		public IActionResult EditCmsPage(long id, string title, string description, string slug, bool status)
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

		public IActionResult GetSkillsPartial()
		{
			List<Skill> skills = _skillRepository.getAllSkills();

			List<SkillModel> skillVm = new();

			foreach (Skill skill in skills)
			{
				skillVm.Add(ConvertSkillToSkillModel(skill));
			}

			return PartialView("_AdminSkillPartial", skillVm);
		}

		public IActionResult GetAddSkillsPartial()
		{
			SkillModel skillVm = new();
			return PartialView("_AddSkillPartial", skillVm);
		}


		public IActionResult AddSkill(SkillModel skillVm)
		{
			try
			{
				Skill skill = new()
				{
					SkillId = skillVm.SkillId,
					SkillName = skillVm.SkillName,
					Status = skillVm.Status,
					CreatedAt = skillVm.CreatedAt,
				};
				_skillRepository.AddSkill(skill);
				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult GetEditSkillsPartial(int skillId)
		{
			Skill skill = _skillRepository.getSkillById(skillId);
			return PartialView("_EditSkillPartial", ConvertSkillToSkillModel(skill));
		}

		public IActionResult EditSkill(SkillModel skillVm)
		{
			try
			{
				Skill skill = _skillRepository.getSkillById(skillVm.SkillId);
				skill.SkillId = skillVm.SkillId;
				skill.SkillName = skillVm.SkillName;
				skill.Status = skillVm.Status;

				_skillRepository.UpdateSkill(skill);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}
		public IActionResult SoftDeleteSkill(int skillId)
		{
			try
			{
				Skill skill = _skillRepository.getSkillById(skillId);

				var countOfMissionSkill = _missionsSkills.GetAllMissionSkills().Where(missionSkill => missionSkill.SkillId == skillId).Count();
				var countOfUserSkill = _usersSkill.GetUserSkills().Where(userSkill => userSkill.SkillId == skillId).Count();

				var isSkillAlreadyInUse = countOfMissionSkill > 0 || countOfUserSkill > 0 ? true : false;

				if (!isSkillAlreadyInUse)
				{
					skill.Status = 0;
					skill.DeletedAt = DateTime.Now;

					_skillRepository.UpdateSkill(skill);
					return Ok("Skill Soft Deleted Successfully !"); ;
				}

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult HardDeleteSkill(int skillId)
		{
			try
			{
				Skill skill = _skillRepository.getSkillById(skillId);

				var countOfMissionSkill = _missionsSkills.GetAllMissionSkills().Where(missionSkill => missionSkill.SkillId == skillId).Count();
				var countOfUserSkill = _usersSkill.GetUserSkills().Where(userSkill => userSkill.SkillId == skillId).Count();

				var isSkillAlreadyInUse = countOfMissionSkill > 0 || countOfUserSkill > 0 ? true : false;

				if (!isSkillAlreadyInUse)
				{
					_skillRepository.DeleteSkill(skill);
					return Ok("Skill Deleted Successfully !"); ;
				}
				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult RestoreSkill(int skillId)
		{
			try
			{
				Skill skill = _skillRepository.getSkillById(skillId);
				skill.Status = 1;
				skill.DeletedAt = null;

				_skillRepository.UpdateSkill(skill);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		private SkillModel ConvertSkillToSkillModel(Skill skill)
		{
			SkillModel skillVm = new()
			{
				SkillId = skill.SkillId,
				SkillName = skill.SkillName,
				Status = skill.Status,
				CreatedAt = skill.CreatedAt,
				isSkillInUse = _missionsSkills.GetAllMissionSkills().Where(missionSkill => missionSkill.SkillId == skill.SkillId).Count() > 0 || _usersSkill.GetUserSkills().Where(userSkill => userSkill.SkillId == skill.SkillId).Count() > 0 ? true : false,
			};

			return skillVm;
		}

		// -------------------- Misison Application --------------------------

		public IActionResult GetMissionApplicationsPartial()
		{
			List<MissionApplication> missionApplication = _missionsApplication.GetAllMissionApplicationsWithInclude();
			List<MissionApplicationModel> missionApplicationsVm = new List<MissionApplicationModel>();

			missionApplication.ForEach(missionApplicationItem =>
			{

				missionApplicationsVm.Add(ConvertMissionToMissionApplicationVm(missionApplicationItem));

			});
			return PartialView("_AdminMissionApplicationPartial", missionApplicationsVm);
		}

		public IActionResult ApproveMissionApplication(long missionApplicationId)
		{
			try
			{
				MissionApplication missionApplication = _missionsApplication.GetMissionApplicationById(missionApplicationId);
				if (missionApplication != null)
				{
					missionApplication.ApprovalStatus = "APPROVED";
					_missionsApplication.UpdaateMissionApplication(missionApplication);
					return Ok("Application Approved !");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
			return NoContent();
		}

		public IActionResult DeclineMissionApplication(long missionApplicationId)
		{
			try
			{
				MissionApplication missionApplication = _missionsApplication.GetMissionApplicationById(missionApplicationId);
				if (missionApplication != null)
				{
					missionApplication.ApprovalStatus = "DECLINED";
					_missionsApplication.UpdaateMissionApplication(missionApplication);
					return Ok("Application Declined!");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
			return NoContent();
		}

		public MissionApplicationModel ConvertMissionToMissionApplicationVm(MissionApplication missionApplicationObj)
		{
			MissionApplicationModel missionApplicationVm = new()
			{
				MissionApplicationId = missionApplicationObj.MissionApplicationId,
				MissionId = missionApplicationObj.MissionId,
				UserId = missionApplicationObj.UserId,
				UserName = missionApplicationObj.User?.FirstName + " " + missionApplicationObj.User?.LastName,
				MissionTitle = missionApplicationObj.Mission?.Title,
				AppliedAt = missionApplicationObj.AppliedAt,
				Mission = missionApplicationObj.Mission,
				User = missionApplicationObj.User,
				ApprovalStatus = missionApplicationObj.ApprovalStatus,
			};
			return missionApplicationVm;
		}


		//-------------------- Mission Theme --------------------------
		public IActionResult GetMissionThemePartial()
		{

			List<MissionTheme> missionThemes = _missionThemeRepository.GetAllThemes();

			List<ThemeModel> listOfMissionThemeVm = new List<ThemeModel>();

			missionThemes.ForEach((missionTheme) =>
			{
				listOfMissionThemeVm.Add(ConvertMissionThemeToMissionThemeModel(missionTheme));
			});

			return PartialView("_AdminMissionThemePartial", listOfMissionThemeVm);
		}

		public IActionResult GetAddMissionThemePartial()
		{
			ThemeModel missionThemeObject = new();
			return PartialView("_AddMissionThemePartial", missionThemeObject);
		}

		public IActionResult AddTheme(ThemeModel themeVm)
		{
			try
			{
				MissionTheme theme = new()
				{
					MissionThemeId = themeVm.MissionThemeId,
					Title = themeVm.MissionThemeTitle,
					Status = themeVm.Status,
					CreatedAt = themeVm.CreatedAt,
				};
				_missionThemeRepository.AddTheme(theme);
				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult GetEditMissionThemePartial(long themeId)
		{
			try
			{
				MissionTheme theme = _missionThemeRepository.GetThemeById(themeId);

				return PartialView("_EditMissionThemePartial", ConvertMissionThemeToMissionThemeModel(theme));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());
				return StatusCode(500);
			}
		}

		public IActionResult EditMissionTheme(ThemeModel themeVm)
		{
			try
			{
				MissionTheme theme = _missionThemeRepository.GetThemeById(themeVm.MissionThemeId);

				theme.MissionThemeId = themeVm.MissionThemeId;
				theme.Title = themeVm.MissionThemeTitle;
				theme.Status = themeVm.Status;

				_missionThemeRepository.UpdateTheme(theme);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult SoftDeleteMissionTheme(long themeId)
		{
			try
			{
				MissionTheme theme = _missionThemeRepository.GetThemeById(themeId);

				var isThemeAlreadyInUse = _volunteeringMissionRepository.getAllMissions().Where(volunteeringMission => volunteeringMission.ThemeId == themeId).Count() > 0 ? true : false;
				if (!isThemeAlreadyInUse)
				{
					theme.Status = 0;
					theme.DeletedAt = DateTime.Now;

					_missionThemeRepository.UpdateTheme(theme);
					return Ok("Theme Soft Deleted Successfully !"); ;
				}

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult HardDeleteMissionTheme(long themeId)
		{
			try
			{
				MissionTheme theme = _missionThemeRepository.GetThemeById(themeId);

				var isThemeAlreadyInUse = _volunteeringMissionRepository.getAllMissions().Where(volunteeringMission => volunteeringMission.ThemeId == themeId).Count() > 0 ? true : false;

				if (!isThemeAlreadyInUse)
				{
					_missionThemeRepository.DeleteTheme(theme);
					return Ok("Theme Deleted Successfully !"); ;
				}
				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public IActionResult RestoreMissionTheme(long themeId)
		{
			try
			{
				MissionTheme theme = _missionThemeRepository.GetThemeById(themeId);

				theme.Status = 1;
				theme.DeletedAt = null;

				_missionThemeRepository.UpdateTheme(theme);

				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500);
			}
		}

		public ThemeModel ConvertMissionThemeToMissionThemeModel(MissionTheme missionTheme)
		{
			ThemeModel missionThemeVm = new()
			{
				MissionThemeId = missionTheme.MissionThemeId,
				MissionThemeTitle = missionTheme.Title,
				Status = missionTheme.Status,
				IsThemeAlreadyInUse = _volunteeringMissionRepository.getAllMissions().Where(volunteeringMission => volunteeringMission.ThemeId == missionTheme.MissionThemeId).Count() > 0 ? true : false,
			};
			return missionThemeVm;
		}


	}
}
