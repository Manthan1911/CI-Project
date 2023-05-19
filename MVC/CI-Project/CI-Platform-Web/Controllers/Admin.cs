using CI_Platform_Web.Utilities;
using CI_Project.Entities.DataModels;
using CI_Project.Entities.Enums;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;


namespace CI_Platform_Web.Controllers
{
    [Authentication]
    public class Admin : Controller
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IBannerRepository _bannerRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IGoalMissionRepository _goalMissionRepository;
        private readonly IMissionDocument _missionDocument;
        private readonly IMissionMediaRepository _missionMediaRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IHomeRepository _homeRepository;
        private readonly IVolunteeringMissionRepository _volunteeringMissionRepository;
        private readonly IMissionThemeRepository _missionThemeRepository;
        private readonly IUsersSkill _usersSkill;
        private readonly IMissionApplication _missionsApplication;
        private readonly IMissionsSkill _missionsSkills;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfService _unitOfService;
        private readonly ICmsRepository _cmsRepository;
        private readonly ISkillsRepository _skillRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Admin(IWebHostEnvironment webHostEnvironment, ITimesheetRepository timesheetRepository, IBannerRepository bannerRepository, IStoryRepository storyRepository, IGoalMissionRepository goalMissionRepository, IMissionDocument missionDocument, IMissionMediaRepository missionMediaRepository, IMissionRepository missionRepository, IHomeRepository homeRepository, IVolunteeringMissionRepository volunteeringMissionRepository, IMissionThemeRepository missionThemeRepository, IMissionApplication missionApplication, IUsersSkill usersSkill, IMissionsSkill missionsSkills, IUserRepository userRepository, IUnitOfService unitOfService, ICmsRepository cmsRepository, ISkillsRepository skillRepository)
        {
            _timesheetRepository = timesheetRepository;
            _bannerRepository = bannerRepository;
            _storyRepository = storyRepository;
            _goalMissionRepository = goalMissionRepository;
            _missionMediaRepository = missionMediaRepository;
            _webHostEnvironment = webHostEnvironment;
            _missionDocument = missionDocument;
            _missionRepository = missionRepository;
            _homeRepository = homeRepository;
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

        public IActionResult Index(string? redirectToStory)
        {
            if (redirectToStory != null && redirectToStory.Equals("true"))
            {
                ViewBag.redirectToStory = true;
            }
            return View();
        }

        //------------------ Searching ---------------------------
        public IActionResult GetSearchedUserPartial(string searchInput)
        {
            List<User> users = _userRepository.getAllUsers().ToList();

            if (!String.IsNullOrEmpty(searchInput))
            {
                users = users.Where(user => user.FirstName.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            List<UserModel> usersVm = new List<UserModel>();
            users.ForEach((user) =>
            {
                usersVm.Add(ConvertUserToUserModel(user));
            });

            return PartialView("_AdminUserPartial", usersVm);
        }

        public IActionResult GetSearchedCmsPartial(string searchInput)
        {
            List<CmsPage> cms = _cmsRepository.GetAll().ToList();

            if (!String.IsNullOrEmpty(searchInput))
            {
                cms = cms.Where(cms => cms.Title.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            List<CmsModel> cmsVm = new List<CmsModel>();
            cms.ForEach((cmsPage) =>
            {
                cmsVm.Add(convertCmsPageToCmsPageVm(cmsPage));
            });

            return PartialView("_AdminCmsPartial", cmsVm);
        }
        public IActionResult GetSearchedMissionPartial(string searchInput)
        {
            List<Mission> missions = _missionRepository.GetAllMissionsWithInclude().ToList();

            if (!String.IsNullOrEmpty(searchInput))
            {
                missions = missions.Where(mission => mission.Title.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            List<MissionModel> missionVm = new List<MissionModel>();
            missions.ForEach((mission) =>
            {
                missionVm.Add(ConvertMissionToMissionModel(mission));
            });

            return PartialView("_AdminMissionPartial", missionVm);
        }
        public IActionResult GetSearchedMissionThemePartial(string searchInput)
        {
            List<MissionTheme> missionThemes = _missionThemeRepository.GetAllThemes().ToList();

            if (!String.IsNullOrEmpty(searchInput))
            {
                missionThemes = missionThemes.Where(missionTheme => missionTheme.Title.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            List<ThemeModel> missionThemeVm = new List<ThemeModel>();
            missionThemes.ForEach((missionTheme) =>
            {
                missionThemeVm.Add(ConvertMissionThemeToMissionThemeModel(missionTheme));
            });

            return PartialView("_AdminMissionThemePartial", missionThemeVm);
        }

        public IActionResult GetSearchedMissionApplicationPartial(string searchInput)
        {
            List<MissionApplication> missionApplications = _missionsApplication.GetAllMissionApplicationsWithInclude().ToList();

            if (!String.IsNullOrEmpty(searchInput))
            {
                missionApplications = missionApplications.Where(missionApplication => missionApplication.Mission.Title.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            List<MissionApplicationModel> missionApplicationVm = new List<MissionApplicationModel>();
            missionApplications.ForEach((missionApplication) =>
            {
                missionApplicationVm.Add(ConvertMissionToMissionApplicationVm(missionApplication));
            });

            return PartialView("_AdminMissionApplicationPartial", missionApplicationVm);
        }

        public IActionResult GetSearchedSkillPartial(string searchInput)
        {
            List<Skill> skills = _skillRepository.getAllSkills().ToList();

            if (!String.IsNullOrEmpty(searchInput))
            {
                skills = skills.Where(skill => skill.SkillName.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            List<SkillModel> skillVm = new List<SkillModel>();
            skills.ForEach((skill) =>
            {
                skillVm.Add(ConvertSkillToSkillModel(skill));
            });

            return PartialView("_AdminSkillPartial", skillVm);
        }

        public IActionResult GetSearchedBannerPartial(string searchInput)
        {
            List<Banner> banners = _bannerRepository.GetAllBanner().ToList();

            if (!String.IsNullOrEmpty(searchInput))
            {
                banners = banners.Where(banner => banner.Title.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            List<BannerModel> bannerVm = new List<BannerModel>();
            banners.ForEach((banner) =>
            {
                bannerVm.Add(ConvertBannerToBannerModel(banner));
            });

            return PartialView("_AdminBannerPartial", bannerVm);
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
                
                NewCmsNotification(title,cmsPage.CmsPageId);

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


        //--------------- Mission -----------------------

        public IActionResult GetMissionPartial()
        {
            List<Mission> missions = _homeRepository.getAllMissions();

            List<MissionModel> missionVmList = new List<MissionModel>();

            missions.ForEach((mission) =>
            {

                missionVmList.Add(ConvertMissionToMissionModel(mission));
            });

            return PartialView("_AdminMissionPartial", missionVmList);
        }

        public IActionResult GetTimeMissionPartial()
        {
            TimeMissionModel timeMissionVm = new();
            timeMissionVm.Countrys = _homeRepository.getAllCountries();
            timeMissionVm.Cities = _homeRepository.getAllCities();
            timeMissionVm.Themes = _missionThemeRepository.GetAllThemes().Where(theme => theme.Status == 1).ToList();
            timeMissionVm.Skills = _skillRepository.getAllSkills().Where(skill => skill.Status == 1).ToList();
            return PartialView("_AddTimeMissionPartial", timeMissionVm);
        }
        public IActionResult GetGoalMissionPartial()
        {
            GoalMissionModel goalMissionVm = new();
            goalMissionVm.Countries = _homeRepository.getAllCountries();
            goalMissionVm.Cities = _homeRepository.getAllCities();
            goalMissionVm.Themes = _missionThemeRepository.GetAllThemes().Where(theme => theme.Status == 1).ToList();
            goalMissionVm.Skills = _skillRepository.getAllSkills().Where(skill => skill.Status == 1).ToList();
            return PartialView("_AddGoalMissionPartial", goalMissionVm);
        }

        public IActionResult AddTimeMission(TimeMissionModel timeMissionVm)
        {
            try
            {

                Mission missionObj = new()
                {
                    ThemeId = timeMissionVm.MissionThemeId,
                    CityId = timeMissionVm.MissionCity,
                    CountryId = timeMissionVm.MissionCountry,
                    Title = timeMissionVm.Title,
                    ShortDescription = timeMissionVm.ShortDescription,
                    Description = timeMissionVm.Description,
                    StartDate = timeMissionVm.StartDate,
                    EndDate = timeMissionVm.EndDate,
                    MissionType = ("time").ToLower(),
                    Status = timeMissionVm.IsActive ?? false,
                    OrganizationName = timeMissionVm.OrganizationName,
                    OrganizationDetail = timeMissionVm.OrganizationDetails,
                    TotalSeats = timeMissionVm.TotalSeats,
                    Availability = timeMissionVm.Availability,
                    CreatedAt = DateTime.Now,
                    RegisterationDeadline = timeMissionVm.RegistrationDeadline,
                };

                _missionRepository.AddMission(missionObj);

                Mission currentlyAddedMission = _missionRepository.GetAllMissionsWithInclude().OrderByDescending(mission => mission.CreatedAt).First();
                var currentMissionId = currentlyAddedMission.MissionId;

                if (timeMissionVm.SelectedSkills != null && timeMissionVm.SelectedSkills.Count() > 0)
                {
                    AddMissionSkillToDatabase(timeMissionVm.SelectedSkills, currentMissionId);
                }

                if (timeMissionVm.Images != null && timeMissionVm.Images.Count() > 0)
                {

                    CopyMissionImagesToWwwRootAndSaveToDatabase(timeMissionVm.Images, currentMissionId);
                }

                if (timeMissionVm.Documents != null && timeMissionVm.Documents.Count() > 0)
                {
                    CopyMissionDocumentToWwwRootAndSaveToDatabase(timeMissionVm.Documents, currentMissionId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return NoContent();
        }

        private void AddMissionSkillToDatabase(List<int> selectedSkills, long currentMissionId)
        {
            selectedSkills.ForEach((skillId) =>
            {
                MissionSkill missionSkill = new()
                {
                    SkillId = skillId,
                    MissionId = currentMissionId,
                    CreatedAt = DateTime.Now,
                };

                _missionsSkills.AddMissionSkill(missionSkill);
            });
        }

        public IActionResult AddGoalMission(GoalMissionModel goalMissionVm)
        {
            try
            {

                Mission missionObj = new()
                {
                    ThemeId = goalMissionVm.MissionThemeId,
                    CityId = goalMissionVm.MissionCity,
                    CountryId = goalMissionVm.MissionCountry,
                    Title = goalMissionVm.Title,
                    ShortDescription = goalMissionVm.ShortDescription,
                    Description = goalMissionVm.Description,
                    StartDate = goalMissionVm.StartDate,
                    EndDate = goalMissionVm.EndDate,
                    MissionType = ("goal").ToLower(),
                    Status = goalMissionVm.IsActive ?? false,
                    OrganizationName = goalMissionVm.OrganizationName,
                    OrganizationDetail = goalMissionVm.OrganizationDetails,
                    TotalSeats = goalMissionVm.TotalSeats,
                    Availability = goalMissionVm.Availability,
                    RegisterationDeadline = goalMissionVm.RegistrationDeadline,
                    CreatedAt = DateTime.Now,

                };

                _missionRepository.AddMission(missionObj);

                Mission currentlyAddedMission = _missionRepository.GetAllMissionsWithInclude().OrderByDescending(mission => mission.CreatedAt).First();
                var currentMissionId = currentlyAddedMission.MissionId;


                GoalMission goalMissionObj = new()
                {
                    MissionId = currentMissionId,
                    GoalObjectiveText = goalMissionVm.GoalObjective,
                    GoalValue = goalMissionVm.GoalValue ?? 0,
                    CreatedAt = DateTime.Now,
                };

                _goalMissionRepository.AddGoalMission(goalMissionObj);


                if (goalMissionVm.SelectedSkills != null && goalMissionVm.SelectedSkills.Count() > 0)
                {
                    AddMissionSkillToDatabase(goalMissionVm.SelectedSkills, currentMissionId);
                }

                if (goalMissionVm.Images != null && goalMissionVm.Images.Count() > 0)
                {
                    CopyMissionImagesToWwwRootAndSaveToDatabase(goalMissionVm.Images, currentMissionId);
                }

                if (goalMissionVm.Documents != null && goalMissionVm.Documents.Count() > 0)
                {
                    CopyMissionDocumentToWwwRootAndSaveToDatabase(goalMissionVm.Documents, currentMissionId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return NoContent();
        }

        public IActionResult GetEditTimeMissionPartial(long missionId)
        {
            Mission? mission = _missionRepository.GetAllMissionsWithInclude().FirstOrDefault(mission => mission.MissionId == missionId);
            TimeMissionModel timeMissionVm = new();

            try
            {
                if (mission != null)
                {

                    timeMissionVm.MissionId = mission.MissionId;
                    timeMissionVm.MissionCity = mission.CityId;
                    timeMissionVm.MissionCountry = mission.CountryId;
                    timeMissionVm.MissionThemeId = mission.ThemeId;
                    timeMissionVm.StartDate = mission.StartDate;
                    timeMissionVm.EndDate = mission.EndDate;
                    timeMissionVm.Title = mission.Title;
                    timeMissionVm.ShortDescription = mission.ShortDescription;
                    timeMissionVm.Description = mission.Description;
                    timeMissionVm.OrganizationName = mission.OrganizationName;
                    timeMissionVm.OrganizationDetails = mission.OrganizationDetail;
                    timeMissionVm.Availability = mission.Availability;
                    timeMissionVm.TotalSeats = mission.TotalSeats;
                    timeMissionVm.IsActive = mission.Status;
                    timeMissionVm.Skills = _skillRepository.getAllSkills().ToList();
                    timeMissionVm.Countrys = _homeRepository.getAllCountries();
                    timeMissionVm.Cities = _homeRepository.getAllCities();
                    timeMissionVm.Themes = _missionThemeRepository.GetAllThemes().Where(theme => theme.Status == 1).ToList();
                    timeMissionVm.Skills = _skillRepository.getAllSkills().Where(skill => skill.Status == 1).ToList();
                    timeMissionVm.MissionSkills = _missionsSkills.GetAllMissionSkills().Where(skill => skill.MissionId == missionId).ToList();
                    timeMissionVm.MissionMedia = _missionMediaRepository.GetAllMissionMedia().Where(missionMedia => missionMedia.MissionId == missionId).ToList();
                    timeMissionVm.MissionDocuments = _missionDocument.GetAllMissionDocumentsd().Where(missionDoc => missionDoc.MissionId == missionId).ToList();
                    timeMissionVm.RegistrationDeadline = mission.RegisterationDeadline;

                    if (timeMissionVm.MissionMedia.Count() > 0)
                    {
                        timeMissionVm.FetchMissionImages = true;
                    }

                    if (timeMissionVm.MissionDocuments.Count() > 0)
                    {
                        timeMissionVm.FetchMissionDocuments = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }

            return PartialView("_EditTimeMissionPartial", timeMissionVm);
        }

        public IActionResult EditTimeMission(TimeMissionModel timeMissionVm)
        {
            try
            {

                long currentMissionId = timeMissionVm.MissionId;
                Mission? mission = _missionRepository.GetAllMissionsWithInclude().FirstOrDefault(mission => mission.MissionId == currentMissionId);


                if (mission != null)
                {

                    mission.ThemeId = timeMissionVm.MissionThemeId;
                    mission.CityId = timeMissionVm.MissionCity;
                    mission.CountryId = timeMissionVm.MissionCountry;
                    mission.Title = timeMissionVm.Title;
                    mission.ShortDescription = timeMissionVm.ShortDescription;
                    mission.Description = timeMissionVm.Description;
                    mission.StartDate = timeMissionVm.StartDate;
                    mission.EndDate = timeMissionVm.EndDate;
                    mission.MissionType = ("time").ToLower();
                    mission.Status = timeMissionVm.IsActive ?? false;
                    mission.OrganizationName = timeMissionVm.OrganizationName;
                    mission.OrganizationDetail = timeMissionVm.OrganizationDetails;
                    mission.TotalSeats = timeMissionVm.TotalSeats;
                    mission.Availability = timeMissionVm.Availability;
                    mission.RegisterationDeadline = timeMissionVm.RegistrationDeadline;

                    _missionRepository.UpdateMission(mission);
                }


                // ------------------------ Skill ------------------------

                List<MissionSkill>? missionSkills = _missionsSkills.GetAllMissionSkills().Where(missionSkill => missionSkill.MissionId == currentMissionId).ToList();
                if (missionSkills != null && missionSkills.Count > 0)
                {
                    _missionsSkills.DeleteListOfMissionSkills(missionSkills);
                }
                if (timeMissionVm.SelectedSkills != null && timeMissionVm.SelectedSkills.Count() > 0)
                {
                    AddMissionSkillToDatabase(timeMissionVm.SelectedSkills, currentMissionId);
                }

                // ------------------------ Images ------------------------

                List<MissionMedium>? missionMedia = _missionMediaRepository.GetAllMissionMedia().Where(missionMedia => missionMedia.MissionId == currentMissionId).ToList();
                if (missionMedia != null && missionMedia.Count > 0)
                {
                    _missionMediaRepository.DeleteAllMissionMediaByMissionId(currentMissionId);
                    DeleteImagesFromWebRoot(missionMedia);
                }
                if (timeMissionVm.Images != null && timeMissionVm.Images.Count > 0)
                {
                    CopyMissionImagesToWwwRootAndSaveToDatabase(timeMissionVm.Images, currentMissionId);
                }

                // ------------------------ Documents ------------------------

                List<MissionDocument>? missionDocuments = _missionDocument.GetAllMissionDocumentsd().Where(missionDocuments => missionDocuments.MissionId == currentMissionId).ToList();
                if (missionDocuments != null && missionDocuments.Count > 0)
                {
                    _missionDocument.DeleteAllMissionDocumentsByMissionId(currentMissionId);
                    DeleteDocumentsFromWebRoot(missionDocuments);
                }
                if (timeMissionVm.Documents != null && timeMissionVm.Documents.Count > 0)
                {
                    CopyMissionDocumentToWwwRootAndSaveToDatabase(timeMissionVm.Documents, currentMissionId);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public IActionResult GetEditGoalMissionPartial(long missionId)
        {
            Mission? mission = _missionRepository.GetAllMissionsWithInclude().FirstOrDefault(mission => mission.MissionId == missionId);
            GoalMissionModel goalMissionVm = new();

            try
            {
                if (mission != null)
                {

                    goalMissionVm.MissionId = mission.MissionId;
                    goalMissionVm.MissionCity = mission.CityId;
                    goalMissionVm.MissionCountry = mission.CountryId;
                    goalMissionVm.MissionThemeId = mission.ThemeId;
                    goalMissionVm.StartDate = mission.StartDate;
                    goalMissionVm.EndDate = mission.EndDate;
                    goalMissionVm.Title = mission.Title;
                    goalMissionVm.ShortDescription = mission.ShortDescription;
                    goalMissionVm.Description = mission.Description;
                    goalMissionVm.OrganizationName = mission.OrganizationName;
                    goalMissionVm.OrganizationDetails = mission.OrganizationDetail;
                    goalMissionVm.Availability = mission.Availability;
                    goalMissionVm.TotalSeats = mission.TotalSeats;
                    goalMissionVm.IsActive = mission.Status;
                    goalMissionVm.Skills = _skillRepository.getAllSkills().ToList();
                    goalMissionVm.Countries = _homeRepository.getAllCountries();
                    goalMissionVm.Cities = _homeRepository.getAllCities();
                    goalMissionVm.Themes = _missionThemeRepository.GetAllThemes().Where(theme => theme.Status == 1).ToList();
                    goalMissionVm.Skills = _skillRepository.getAllSkills().Where(skill => skill.Status == 1).ToList();
                    goalMissionVm.MissionSkill = _missionsSkills.GetAllMissionSkills().Where(skill => skill.MissionId == missionId).ToList();
                    goalMissionVm.MissionMedia = _missionMediaRepository.GetAllMissionMedia().Where(missionMedia => missionMedia.MissionId == missionId).ToList();
                    goalMissionVm.MissionDocument = _missionDocument.GetAllMissionDocumentsd().Where(missionDoc => missionDoc.MissionId == missionId).ToList();
                    goalMissionVm.RegistrationDeadline = mission.RegisterationDeadline;

                    GoalMission? goalMissionObj = _goalMissionRepository.GetAllGoalMissionWithInclude().FirstOrDefault(goalMission => goalMission.MissionId == goalMissionVm.MissionId);

                    if (goalMissionObj != null)
                    {
                        goalMissionVm.GoalObjective = goalMissionObj.GoalObjectiveText;
                        goalMissionVm.GoalValue = goalMissionObj.GoalValue;
                    }

                    if (goalMissionVm.MissionMedia.Count() > 0)
                    {
                        goalMissionVm.FetchMissionImages = true;
                    }

                    if (goalMissionVm.MissionDocument.Count() > 0)
                    {
                        goalMissionVm.FetchMissionDocuments = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }

            return PartialView("_EditGoalMissionPartial", goalMissionVm);
        }

        public IActionResult EditGoalMission(GoalMissionModel goalMissionVm)
        {
            try
            {

                long currentMissionId = goalMissionVm.MissionId;
                Mission? mission = _missionRepository.GetAllMissionsWithInclude().FirstOrDefault(mission => mission.MissionId == currentMissionId);


                if (mission != null)
                {

                    mission.ThemeId = goalMissionVm.MissionThemeId;
                    mission.CityId = goalMissionVm.MissionCity;
                    mission.CountryId = goalMissionVm.MissionCountry;
                    mission.Title = goalMissionVm.Title;
                    mission.ShortDescription = goalMissionVm.ShortDescription;
                    mission.Description = goalMissionVm.Description;
                    mission.StartDate = goalMissionVm.StartDate;
                    mission.EndDate = goalMissionVm.EndDate;
                    mission.MissionType = ("goal").ToLower();
                    mission.Status = goalMissionVm.IsActive ?? false;
                    mission.OrganizationName = goalMissionVm.OrganizationName;
                    mission.OrganizationDetail = goalMissionVm.OrganizationDetails;
                    mission.TotalSeats = goalMissionVm.TotalSeats;
                    mission.Availability = goalMissionVm.Availability;
                    mission.RegisterationDeadline = goalMissionVm.RegistrationDeadline;
                    mission.UpdatedAt = DateTime.Now;

                    _missionRepository.UpdateMission(mission);
                }

                GoalMission? goalMission = _goalMissionRepository.GetAllGoalMissionWithInclude().FirstOrDefault(goal => goal.MissionId == currentMissionId);

                if (goalMission != null)
                {
                    goalMission.GoalValue = goalMissionVm.GoalValue ?? 0;
                    goalMission.GoalObjectiveText = goalMissionVm.GoalObjective;
                    goalMission.UpdatedAt = DateTime.Now;

                    _goalMissionRepository.UpdateGoalMission(goalMission);
                }

                // ------------------------ Skill ------------------------

                List<MissionSkill>? missionSkills = _missionsSkills.GetAllMissionSkills().Where(missionSkill => missionSkill.MissionId == currentMissionId).ToList();
                if (missionSkills != null && missionSkills.Count > 0)
                {
                    _missionsSkills.DeleteListOfMissionSkills(missionSkills);
                }
                if (goalMissionVm.SelectedSkills != null && goalMissionVm.SelectedSkills.Count() > 0)
                {
                    AddMissionSkillToDatabase(goalMissionVm.SelectedSkills, currentMissionId);
                }


                // ------------------------ Images ------------------------

                List<MissionMedium>? missionMedia = _missionMediaRepository.GetAllMissionMedia().Where(missionMedia => missionMedia.MissionId == currentMissionId).ToList();
                if (missionMedia != null && missionMedia.Count > 0)
                {
                    _missionMediaRepository.DeleteAllMissionMediaByMissionId(currentMissionId);
                    DeleteImagesFromWebRoot(missionMedia);
                }
                if (goalMissionVm.Images != null && goalMissionVm.Images.Count > 0)
                {
                    CopyMissionImagesToWwwRootAndSaveToDatabase(goalMissionVm.Images, currentMissionId);
                }

                // ------------------------ Documents ------------------------

                List<MissionDocument>? missionDocuments = _missionDocument.GetAllMissionDocumentsd().Where(missionDocuments => missionDocuments.MissionId == currentMissionId).ToList();
                if (missionDocuments != null && missionDocuments.Count > 0)
                {
                    _missionDocument.DeleteAllMissionDocumentsByMissionId(currentMissionId);
                    DeleteDocumentsFromWebRoot(missionDocuments);
                }
                if (goalMissionVm.Documents != null && goalMissionVm.Documents.Count > 0)
                {
                    CopyMissionDocumentToWwwRootAndSaveToDatabase(goalMissionVm.Documents, currentMissionId);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public IActionResult SoftDeleteMission(long missionId)
        {
            try
            {
                Mission? mission = _missionRepository.GetAllMissionsWithInclude().FirstOrDefault(mission => mission.MissionId == missionId);
                if (mission != null)
                {
                    mission.Status = false;
                    mission.DeletedAt = DateTime.Now;
                    _missionRepository.UpdateMission(mission);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public IActionResult RestoreMission(long missionId)
        {
            try
            {
                Mission? mission = _missionRepository.GetAllMissionsWithInclude().FirstOrDefault(mission => mission.MissionId == missionId);
                if (mission != null)
                {
                    mission.Status = true;
                    mission.DeletedAt = null;
                    _missionRepository.UpdateMission(mission);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public MissionModel ConvertMissionToMissionModel(Mission missionObj)
        {

            MissionModel missionVm = new()
            {
                MissionId = missionObj.MissionId,
                Title = missionObj.Title,
                MissionType = missionObj.MissionType,
                StartDate = missionObj.StartDate,
                EndDate = missionObj.EndDate,
                Status = missionObj.Status,
                IsActive = missionObj.IsActive,
                RegisterationDeadline = missionObj.RegisterationDeadline,
            };

            return missionVm;

        }

        private void CopyMissionDocumentToWwwRootAndSaveToDatabase(List<IFormFile> documents, long currentMissionId)
        {
            foreach (var document in documents)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"mission_documents");
                var extension = Path.GetExtension(document.FileName);

                using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    document.CopyTo(fileStrems);
                }

                MissionDocument missionDocumentObj = new()
                {
                    DocumentName = fileName,
                    DocumentPath = @"\mission_documents\",
                    DocumentType = extension!,
                    MissionId = currentMissionId,
                    CreatedAt = DateTime.Now,
                };

                _missionDocument.AddMissionDocument(missionDocumentObj);

            }
        }

        private void CopyMissionImagesToWwwRootAndSaveToDatabase(List<IFormFile> images, long currentMissionId)
        {
            foreach (var image in images)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\mission_images");
                var extension = Path.GetExtension(image.FileName);

                using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    image.CopyTo(fileStrems);
                }

                MissionMedium missionMediaObj = new()
                {
                    MediaName = fileName,
                    MediaPath = @"\images\mission_images\",
                    MediaType = extension!,
                    MissionId = currentMissionId,
                    CreatedAt = DateTime.Now,
                };

                _missionMediaRepository.AddMissionMedia(missionMediaObj);

            }
        }

        private void DeleteImagesFromWebRoot(List<MissionMedium> missionMedia)
        {
            if (missionMedia != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var path = $@"{wwwRootPath}\images\mission_images";

                foreach (var currMedia in missionMedia)
                {
                    var filePath = currMedia.MediaName;
                    var fileType = currMedia.MediaType;

                    var url = Path.Combine(path, filePath + fileType);
                    System.IO.File.Delete(url);
                }
            }
        }

        private void DeleteDocumentsFromWebRoot(List<MissionDocument> missionDocuments)
        {
            if (missionDocuments != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var path = $@"{wwwRootPath}\mission_documents";

                foreach (var currDocument in missionDocuments)
                {
                    var filePath = currDocument.DocumentName;
                    var fileType = currDocument.DocumentType;

                    var url = Path.Combine(path, filePath + fileType);
                    System.IO.File.Delete(url);
                }
            }
        }

        public bool CheckIsEmailAlreadyUsed(string email)
        {
            return _userRepository.validateEmail(email);
        }

        // -------------------------- Story -------------------------- 

        public IActionResult GetStoryPartial()
        {
            List<Story> listOfStories = _storyRepository.getAllStories().Where(story => story.Status.Equals(("PENDING").ToUpper())).ToList();
            List<StoryModel> listOfStoriesVm = new List<StoryModel>();
            listOfStories.ForEach((story) =>
            {
                listOfStoriesVm.Add(ConvertStoryToStoryModel(story));
            });
            return PartialView("_AdminStoryPartial", listOfStoriesVm);
        }

        public IActionResult GetSearchedStoryPartial(string searchInput)
        {
            //.Where(story => story.Status.Equals(("PENDING").ToUpper()
            List<Story> listOfStories = _storyRepository.getAllStories();

            if (!String.IsNullOrEmpty(searchInput))
            {
                listOfStories = listOfStories.Where(story => story.Title.ToLower().Contains(searchInput.ToLower())).ToList();
            }
            List<StoryModel> listOfStoriesVm = new List<StoryModel>();
            listOfStories.ForEach((story) =>
            {
                listOfStoriesVm.Add(ConvertStoryToStoryModel(story));
            });

            return PartialView("_AdminStoryPartial", listOfStoriesVm);
        }

        public IActionResult StoryPreview(long storyId)
        {
            Story? story = _storyRepository.getStory(storyId);
            StoryModel storyVm = new();
            if (story != null)
            {
                storyVm = ConvertStoryToStoryModel(story);
            }
            return View(storyVm);
        }

        public IActionResult ApproveStory(long storyId)
        {
            try
            {
                Story? story = _storyRepository.getStory(storyId);
                if (story != null)
                {
                    story.Status = ("Approved").ToUpper();
                    story.PublishedAt = DateTime.Now;
                    _storyRepository.updateStory(story);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public IActionResult DeclineStory(long storyId)
        {
            try
            {
                Story? story = _storyRepository.getStory(storyId);
                if (story != null)
                {
                    story.Status = ("Declined").ToUpper();
                    story.PublishedAt = DateTime.Now;
                    _storyRepository.updateStory(story);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public IActionResult DeleteStory(long storyId)
        {
            try
            {
                Story? story = _storyRepository.getStory(storyId);
                if (story != null)
                {
                    List<StoryMedium> storyMedia = _storyRepository.getAllMediaOfStory(storyId);
                    _storyRepository.DeleteAllStoryInvite(storyId);
                    _storyRepository.deleteAllMediaOfStory(storyMedia);
                    _storyRepository.deleteStory(story);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        private StoryModel ConvertStoryToStoryModel(Story story)
        {
            StoryModel storyVm = new()
            {
                StoryId = story.StoryId,
                Status = story.Status,
                UserId = story.UserId,
                MissionId = story.MissionId,
                StoryTitle = story.Title,
                StoryDescription = story.Description,
                User = _userRepository.findUser(story.UserId),
                Mission = story.Mission,
                StoryMedia = _storyRepository.getAllMediaOfStory(story.StoryId),
                CoverImage = _storyRepository.getAllMediaOfStory(story.StoryId).First().Path + _storyRepository.getAllMediaOfStory(story.StoryId).First().Type,
            };
            return storyVm;
        }

        //------------------------ Banner ------------------------------
        public IActionResult GetBannerPartial()
        {
            List<Banner> banners = _bannerRepository.GetAllBanner();
            List<BannerModel> bannerVm = new();
            banners.ForEach((banner) =>
            {
                bannerVm.Add(ConvertBannerToBannerModel(banner));
            });
            return PartialView("_AdminBannerPartial", bannerVm);
        }

        public IActionResult GetAddBannerPartial()
        {
            BannerModel bannerVm = new();
            return PartialView("_AddBannerPartial", bannerVm);
        }

        [HttpPost]
        public IActionResult SaveBanner(BannerModel bannerVm)
        {
            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\banner_images");
                var extension = Path.GetExtension(bannerVm.BannerImage.FileName);
                var path = Path.Combine(uploads, fileName + extension);

                Banner banner = new()
                {
                    MediaName = fileName,
                    MediaPath = @"\images\banner_images\",
                    MediaType = extension,
                    Title = bannerVm.Title,
                    Description = bannerVm.Description,
                    SortOrder = bannerVm.SortOrder,
                    CreatedAt = DateTime.Now,
                };
                _bannerRepository.AddBanner(banner);

                using (var fileStrems = new FileStream(path, FileMode.Create))
                {
                    bannerVm.BannerImage.CopyTo(fileStrems);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public IActionResult GetEditBannerPartial(long bannerId)
        {

            Banner? banner = _bannerRepository.GetBannerById(bannerId);

            BannerModel bannerVm = new();

            if (banner != null)
            {
                bannerVm = ConvertBannerToBannerModel(banner);
            }

            return PartialView("_EditBannerPartial", bannerVm);
        }

        [HttpPost]
        public IActionResult EditBanner(BannerModel bannerVm)
        {
            try
            {

                Banner? banner = _bannerRepository.GetBannerById(bannerVm.BannerId);
                if (banner != null)
                {
                    var image = bannerVm.BannerImage;
                    if (!banner.MediaName.Equals(image.FileName))
                    {
                        DeleteBannerImagesFromWebRoot(banner);
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"images\banner_images");
                        var extension = Path.GetExtension(image.FileName);

                        using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            image.CopyTo(fileStrems);
                        }


                        banner.MediaName = fileName;
                        banner.MediaType = extension;
                    }

                    banner.Description = bannerVm.Description;
                    banner.Title = bannerVm.Title;
                    banner.SortOrder = bannerVm.SortOrder;
                    banner.UpdatedAt = DateTime.Now;

                    _bannerRepository.UpdateBanner(banner);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public IActionResult DeleteBanner(long bannerId)
        {
            try
            {
                Banner? banner = _bannerRepository.GetBannerById(bannerId);
                if (banner != null)
                {
                    DeleteBannerImagesFromWebRoot(banner);
                    _bannerRepository.DeleteBanner(banner);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        public BannerModel ConvertBannerToBannerModel(Banner banner)
        {
            BannerModel bannerVm = new()
            {
                BannerId = banner.BannerId,
                MediaName = banner.MediaName,
                MediaPath = banner.MediaPath,
                MediaType = banner.MediaType,
                Title = banner.Title,
                Description = banner.Description,
                SortOrder = banner.SortOrder,
            };
            return bannerVm;
        }

        private void DeleteBannerImagesFromWebRoot(Banner banner)
        {
            if (banner != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var path = $@"{wwwRootPath}\images\banner_images";
                var fileName = banner.MediaName;
                var fileType = banner.MediaType;
                var url = Path.Combine(path, fileName + fileType);
                System.IO.File.Delete(url);
            }
        }


        // ----------------------- Timesheet -------------------------------

        public IActionResult GetMissionTimesheetPartial()
        {

            List<Timesheet> timesheets = _timesheetRepository.GetAllTimesheet();

            List<TimesheetModel> timesheetModels = new List<TimesheetModel>();

            timesheets.ForEach(timesheet =>
            {
                timesheetModels.Add(ConvertTimesheetToTimesheetModel(timesheet));
            });

            return PartialView("_AdminTimesheetPartial", timesheetModels);
        }

        public IActionResult ApproveMissionTimesheet(long timesheetId)
        {
            try
            {
                Timesheet?  timesheet = _timesheetRepository.GetTimesheetById(timesheetId);
                if (timesheet != null)
                {
                    timesheet.Status = ("Approved").ToUpper();
                    _timesheetRepository.UpdateTimesheet(timesheet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }


        public IActionResult DeclineMissionTimesheet(long timesheetId)
        {
            try
            {
                Timesheet? timesheet = _timesheetRepository.GetTimesheetById(timesheetId);
                if (timesheet != null)
                {
                    timesheet.Status = ("Declined").ToUpper();
                    _timesheetRepository.UpdateTimesheet(timesheet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
            return NoContent();
        }

        private TimesheetModel ConvertTimesheetToTimesheetModel(Timesheet timesheet)
        {
            return new TimesheetModel()
            {
                TimesheetId = timesheet.TimesheetId,
                Action = timesheet.Action,
                Time = timesheet.Time,
                UserId = timesheet.UserId,
                MissionId = timesheet.MissionId,
                User = timesheet.User,
                Mission = timesheet.Mission,
                DateVolunteered = timesheet.DateVolunteered,
                Notes = timesheet.Notes,
                Status = timesheet.Status,
                CreatedAt = timesheet.CreatedAt,
                UpdatedAt = timesheet.UpdatedAt,
            };
        }


        public void NewCmsNotification(string title,long cmsId)
        {
            SendNotificationVm sendNotificationVm = new SendNotificationVm()
            {
                NotificationText = $"New Cms Page Added - {title} !",
                NotificationType = NotificationType.ADD,
                SettingTypeName = "news",
                Link = $"/Home/CmsPage/{cmsId}"
            };

            _unitOfService.Notification.SendNotificationToAllUsers(sendNotificationVm);
        }

        //public string GetCommaSeperatedStringOfUserId()
        //{
        //    var userIds = _unitOfService.Notification.GetAllNotificationSettings();
        //}
    }
}
