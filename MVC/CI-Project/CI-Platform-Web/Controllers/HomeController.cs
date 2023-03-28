using CI_Platform_Web.Models;
using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace CI_Platform_Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IHomeRepository _homeRepository;
		private readonly IUserRepository _userRepository;
		private readonly CIProjectDbContext _cIProjectDbContext;

		public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository, IUserRepository userRepository, CIProjectDbContext cIProjectDbContext)
		{
			_logger = logger;
			_homeRepository = homeRepository;
			_userRepository = userRepository;
			_cIProjectDbContext = cIProjectDbContext;
		}

		public IActionResult Index()
		{
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
		public IActionResult bringMissionsToGridView(long?[] countries,long?[] cities, long?[] themes, long?[] skills, string sortBy, string missionToSearch, int pageNo, int pageSize)
		{
			sortBy = String.IsNullOrEmpty(sortBy) ? "Newest" : sortBy;
			pageNo = pageNo <= 0 ? 1 : pageNo;

			List<Mission> missions = _homeRepository.getAllMissions();
			List<MissionModel> missionVmList = new();
			GridListModel gridListModel = new GridListModel();
			string? currentUserEmail = HttpContext.Session.GetString("UserEmail");
			gridListModel.userId = _userRepository.findUser(currentUserEmail!).UserId;


			if (!countries.IsNullOrEmpty() || !cities.IsNullOrEmpty() || !themes.IsNullOrEmpty() || !skills.IsNullOrEmpty())
			{
				missions = applyFiltersOnMission(missions, countries, cities, themes, skills);
			}

			missions = sortMissions(sortBy, missions);

			if (missionToSearch != null)
			{
				missions = sortMissions(sortBy, _homeRepository.searchMissionAccToTitle(missionToSearch,missions));

				foreach (var currMisssion in missions)
				{
					missionVmList.Add(convertDataModelToMissionModel(currMisssion, gridListModel.userId));
				}

				ViewBag.totalMissions = missionVmList.Count;
				ViewBag.paginationLimit = pageSize;
				return PartialView("_GridViewListViewPartial", missionVmList.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
			}

			foreach (var currMisssion in missions)
			{
				missionVmList.Add(convertDataModelToMissionModel(currMisssion,gridListModel.userId));
			}

			ViewBag.totalMissions = missionVmList.Count;
			ViewBag.paginationLimit = pageSize;

			gridListModel.missionModels = missionVmList.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
			return PartialView("_GridViewListViewPartial", gridListModel);
		}

		public static MissionModel convertDataModelToMissionModel(Mission mission,long userId)
		{
			MissionModel missionModel = new MissionModel
			{
				MissionId = mission.MissionId,
				CityId = mission.CityId,
				City = mission.City,
				ThemeId = mission.ThemeId,
				Theme = mission.Theme,
				Title = mission.Title,
				Availability = mission.Availability,
				ShortDescription = mission.ShortDescription,
				Description = mission.Description,
				StartDate = mission.StartDate.ToString().Remove(10),
				EndDate = mission.EndDate.ToString().Remove(10),
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
				CoverImage = getMissionCoverImageUrl(mission.MissionMedia),
				isMissionFavourite = mission.FavouriteMissions.Any(fm => fm.MissionId == mission.MissionId && fm.UserId == userId) ? 1 : 0,
				totalSeats=mission.TotalSeats,
				seatsLeft = mission.TotalSeats - mission.MissionApplications.Where(ma => ma.MissionId == mission.MissionId && ma.ApprovalStatus.ToLower().Equals("approved")).Count()
			};
			if (missionModel.countOfRatingsByPeople != 0)
			{
				missionModel.avgRating = ((missionModel.sumOfRating % missionModel.countOfRatingsByPeople) != 0) ? ((missionModel.sumOfRating / missionModel.countOfRatingsByPeople) + 1) : (missionModel.sumOfRating / missionModel.countOfRatingsByPeople);
			}
			return missionModel;
		}

        private static string? getMissionCoverImageUrl(ICollection<MissionMedium> missionMedia)
        {
			var media =  missionMedia.FirstOrDefault(m => m.Default == true);
            string? missionCoverImageUrl = media?.MediaPath + media?.MediaName + media?.MediaType;
			return missionCoverImageUrl;
        }

        public List<Mission> sortMissions(string sortBy, List<Mission> missions)
		{
			switch (sortBy)
			{
				case "Newest":
					return missions.OrderByDescending(mission => mission.CreatedAt).ToList();
				case "Oldest":
					return missions.OrderBy(mission => mission.CreatedAt).ToList();
				default:
					return missions.OrderBy(mission => mission.CreatedAt).ToList();
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
			_homeRepository.addOrRemoveFavourite(missionId,userId);
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}