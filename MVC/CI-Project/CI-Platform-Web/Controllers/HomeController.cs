using CI_Platform_Web.Models;
using CI_Platform_Web.Utilities;
using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Cms;
using System.Diagnostics;

namespace CI_Platform_Web.Controllers
{
    [Authenticate]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        private readonly IMissionMediaRepository _missionMediaRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVolunteeringTimesheetRepository _volunteeringTimesheetRepository;
        private readonly CIProjectDbContext _cIProjectDbContext;
        private readonly IMissionApplication _missionApplicationRepository;
        private readonly ICmsRepository _cmsRepository;

        public HomeController(ILogger<HomeController> logger, ICmsRepository cmsRepository, IMissionApplication missionApplicationRepository, IVolunteeringTimesheetRepository volunteeringTimesheetRepository, IMissionMediaRepository missionMediaRepository, IHomeRepository homeRepository, IUserRepository userRepository, CIProjectDbContext cIProjectDbContext)
        {
            _logger = logger;
            _homeRepository = homeRepository;
            _userRepository = userRepository;
            _cIProjectDbContext = cIProjectDbContext;
            _missionMediaRepository = missionMediaRepository;
            _volunteeringTimesheetRepository = volunteeringTimesheetRepository;
            _missionApplicationRepository = missionApplicationRepository;
            _cmsRepository = cmsRepository;
        }

        public IActionResult Index(string? profileSuccess)
        {
            ViewBag.ProfileSuccess = profileSuccess;
            var userEmailId = HttpContext.Session.GetString("UserEmail");
            if (userEmailId == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            List<Country> countryList = _homeRepository.getAllCountries();
            List<City> cityList = _homeRepository.getAllCities();
            List<MissionTheme> themeList = _homeRepository.getAllThemes();
            List<Skill> skillList = _homeRepository.getAllSkills();

            HomeModel model = new HomeModel()
            {
                user = new(),
                countries = new(),
                cities = new(),
                themes = new(),
                skills = new(),
            };

            model.user = _userRepository.findUser(userEmailId);

            model.countries.AddRange(countryList.Select(currentCountry => new CountryModel
            {
                CountryId = currentCountry.CountryId,
                CountryName = currentCountry.Name
            }));

            model.cities.AddRange(cityList.Select(currentCity => new CityModel
            {
                CityId = currentCity.CityId,
                CityName = currentCity.Name
            }));

            model.themes.AddRange(themeList.Select(currentTheme => new ThemeModel
            {
                MissionThemeId = currentTheme.MissionThemeId,
                MissionThemeTitle = currentTheme.Title
            }));

            model.skills.AddRange(skillList.Select(currentSkill => new SkillModel
            {
                SkillId = currentSkill.SkillId,
                SkillName = currentSkill.SkillName,
            }));
            return View(model);
        }

        [HttpPost]
        public IActionResult bringMissionsToGridView(long?[] countries, long?[] cities, long?[] themes, long?[] skills, string sortBy, string missionToSearch, int pageNo, int pageSize)
        {
            sortBy = String.IsNullOrEmpty(sortBy) ? "Newest" : sortBy;
            pageNo = pageNo <= 0 ? 1 : pageNo;

            List<Mission> missions = _homeRepository.getAllMissions().Where(mission => mission.Status == true).ToList();
            List<MissionModel> missionVmList = new();
            GridListModel gridListModel = new GridListModel();
            string? currentUserEmail = HttpContext.Session.GetString("UserEmail");
            var userId = _userRepository.findUser(currentUserEmail!).UserId;
            gridListModel.userId = userId;

            if (!countries.IsNullOrEmpty() || !cities.IsNullOrEmpty() || !themes.IsNullOrEmpty() || !skills.IsNullOrEmpty())
            {
                missions = applyFiltersOnMission(missions, countries, cities, themes, skills);
            }

            foreach (var currMisssion in missions)
            {
                missionVmList.Add(convertDataModelToMissionModel(currMisssion, gridListModel.userId));
            }

            missionVmList = sortMissions(sortBy, missionVmList);

            if (missionToSearch != null)
            {
                List<MissionModel> searchedMissions = new List<MissionModel>();
                missionVmList.ForEach((missionVm) =>
                {

                    if (missionVm.Title.ToLower().Contains(missionToSearch))
                    {
                        searchedMissions.Add(missionVm);
                    }

                });

                missionVmList = sortMissions(sortBy,searchedMissions);

                gridListModel.missionModels = missionVmList;
                ViewBag.totalMissions = gridListModel.missionModels.Count;
                ViewBag.paginationLimit = pageSize;

                gridListModel.missionModels = gridListModel.missionModels.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                return PartialView("_GridViewListViewPartial", gridListModel);
            }

            ViewBag.totalMissions = missionVmList.Count;
            ViewBag.paginationLimit = pageSize;

            gridListModel.missionModels = missionVmList.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return PartialView("_GridViewListViewPartial", gridListModel);
        }

        public MissionModel convertDataModelToMissionModel(Mission mission, long userId)
        {
            MissionModel missionModel = new MissionModel
            {
                MissionId = mission.MissionId,
                CurrentUserId = userId,
                CityId = mission.CityId,
                City = mission.City,
                ThemeId = mission.ThemeId,
                Theme = mission.Theme,
                Title = mission.Title,
                Availability = mission.Availability,
                ShortDescription = mission.ShortDescription,
                Description = mission.Description,
                StartDate = mission.StartDate,
                EndDate = mission.EndDate,
                OrganizationName = mission.OrganizationName,
                OrganizationDetails = mission.OrganizationDetail,
                MissionType = mission.MissionType,
                MissionSkills = mission.MissionSkills,
                GoalMissions = mission.GoalMissions,
                GoalMission = mission.GoalMissions.FirstOrDefault(gm => gm.MissionId == mission.MissionId),
                FavouriteMissions = mission.FavouriteMissions,
                MissionRatings = mission.MissionRatings,
                countOfRatingsByPeople = mission.MissionRatings.Select(m => m.MissionId == mission.MissionId).Count(),
                sumOfRating = mission.MissionRatings.Where(m => m.MissionId == mission.MissionId).Sum(m => m.Rating),
                CoverImage = getMissionCoverImageUrl(mission.MissionId),
                isMissionFavourite = mission.FavouriteMissions.Any(fm => fm.MissionId == mission.MissionId && fm.UserId == userId) ? 1 : 0,
                totalSeats = mission.TotalSeats,
                seatsLeft = mission.TotalSeats - mission.MissionApplications.Where(ma => ma.MissionId == mission.MissionId && ma.ApprovalStatus.ToLower().Equals("approved")).Count(),
                isMissionApplied = mission.MissionApplications.Any(ma => ma.MissionId == mission.MissionId && ma.UserId == userId) ? 1 : 0,
                isMissionAppliedByCurrentUser = _missionApplicationRepository.GetAllMissionApplicationsWithInclude().FirstOrDefault(missionApplication => missionApplication.MissionId == mission.MissionId && missionApplication.UserId == userId && missionApplication.ApprovalStatus.Equals("APPROVED")) != null ? true : false,
                CreatedAt = mission.CreatedAt,
            };
            if (missionModel.countOfRatingsByPeople != 0)
            {
                missionModel.avgRating = ((missionModel.sumOfRating % missionModel.countOfRatingsByPeople) != 0) ? ((missionModel.sumOfRating / missionModel.countOfRatingsByPeople) + 1) : (missionModel.sumOfRating / missionModel.countOfRatingsByPeople);
            }
            if (missionModel.MissionType.Equals("goal"))
            {
                missionModel.GoalAchieved = (int)_volunteeringTimesheetRepository.GetAllWithInclude().Where(timesheet => timesheet.MissionId == missionModel.MissionId && timesheet.Status.Equals("APPROVED")).Sum(timesheet => timesheet.Action);
                missionModel.AvgGoal = ((missionModel.GoalAchieved * 100) / (int)missionModel.GoalMission?.GoalValue);
            }
            return missionModel;
        }

        public string? getMissionCoverImageUrl(long missionId)
        {
            var media = _missionMediaRepository.GetAllMissionMedia().FirstOrDefault(missionMedia => missionMedia.MissionId == missionId);
            string? missionCoverImageUrl = media?.MediaPath + media?.MediaName + media?.MediaType;
            return missionCoverImageUrl;
        }

        public List<MissionModel> sortMissions(string sortBy, List<MissionModel> missionVmList)
        {
            switch (sortBy)
            {
                case "Newest":
                    return missionVmList.OrderByDescending(mission => mission.CreatedAt).ToList();
                case "Oldest":
                    return missionVmList.OrderBy(mission => mission.CreatedAt).ToList();
                case "Favourites":
                    return missionVmList.Where(currentMission => currentMission.FavouriteMissions.Any(favouriteMission => favouriteMission.UserId == currentMission.CurrentUserId)).ToList();
                case "Lowest Available Seats":
                    return missionVmList.OrderBy(currentMisison => currentMisison.seatsLeft).ToList();
                case "Highest Available Seats":
                    return missionVmList.OrderByDescending(currentMisison => currentMisison.seatsLeft).ToList();
                //case "Sort By Deadline":
                //    return missionVmList.OrderBy(currentMisison => currentMisison.seatsLeft).ToList();
                default:
                    return missionVmList.OrderBy(mission => mission.CreatedAt).ToList();
            }
        }

        public List<Mission> applyFiltersOnMission(List<Mission> missions, long?[] countries, long?[] cities, long?[] themes, long?[] skills)
        {
            if (countries.Length > 0)
            {
                missions = missions.Where(x => countries.Contains(x.CountryId)).ToList();
            }

            if (cities.Length > 0)
            {
                missions = missions.Where(x => cities.Contains(x.CityId)).ToList();
            }

            if (themes.Length > 0)
            {
                missions = missions.Where(x => themes.Contains(x.ThemeId)).ToList();
            }
            if (skills.Length > 0)
            {
                missions = missions.Where(x => skills.Any(s => x.MissionSkills.Any(ms => ms.SkillId == s))).ToList();
            }
            return missions;
        }

        public void addOrRemoveFavourite(long missionId)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var userId = _userRepository.findUser(email!).UserId;
            _homeRepository.addOrRemoveFavourite(missionId, userId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetCmsList()
        {
            List<CmsModel> cmsPageVMs = _cmsRepository.GetConvertedAll().ToList();
            return Json(cmsPageVMs.Select(cms => new { cms.Title, cms.CmsPageId }));
        }

        public IActionResult CmsPage(long id)
        {
            try
            {
                CmsModel cms = _cmsRepository.GetConvertedAll().FirstOrDefault(cms => cms.CmsPageId == id);
                return View("Cms", cms);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { area = "Users" });
            }
        }
    }
}