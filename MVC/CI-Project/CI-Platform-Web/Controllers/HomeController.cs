﻿using CI_Platform_Web.Models;
using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Platform_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
            _userRepository = userRepository;
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

        public IActionResult bringMissionsToGridView(string sortBy ,string missionToSearch)
        {
            sortBy = String.IsNullOrEmpty(sortBy) ? "Newest" : sortBy;

            List<Mission> missions ;
            List<MissionModel> missionVmList = new();
            if(missionToSearch != null)
            {
                missions =  _homeRepository.searchMissionAccToTitle(missionToSearch);
                foreach (var currMisssion in missions)
                {
                    missionVmList.Add(convertDataModelToMissionModel(currMisssion));
                }
                return PartialView("_GridViewListViewPartial", missionVmList);
            }

            missions =  _homeRepository.getAllMissions();

            switch (sortBy)
            {
                case "Newest":
                    missions = missions.OrderByDescending(mission => mission.CreatedAt).ToList();
                    break;
                case "Oldest":
                    missions = missions.OrderBy(mission => mission.CreatedAt).ToList();
                    break;
                default:
                    missions = missions.OrderBy(mission => mission.CreatedAt).ToList();
                    break;
            }

            foreach (var currMisssion in missions)
            {
                missionVmList.Add(convertDataModelToMissionModel(currMisssion));
            }

            return PartialView("_GridViewListViewPartial", missionVmList);
        }

        public MissionModel convertDataModelToMissionModel(Mission mission)
        {
            MissionModel missionModel = new MissionModel();
            missionModel.MissionId = mission.MissionId;
            missionModel.CityId = mission.CityId;
            missionModel.City = mission.City;
            missionModel.ThemeId = mission.ThemeId;
            missionModel.Theme = mission.Theme;
            missionModel.Title = mission.Title;
            missionModel.ShortDescription= mission.ShortDescription;
            missionModel.StartDate= mission.StartDate.ToString().Remove(10);
            missionModel.EndDate= mission.EndDate.ToString().Remove(10);
            missionModel.OrganizationName = mission.OrganizationName;
            missionModel.MissionType= mission.MissionType;
            missionModel.CoverImage = getMissionCoverImageUrl(mission.MissionId);
            return missionModel;
        }

        public string getMissionCoverImageUrl(long id)
        {
            MissionMedium? missionMedia = _homeRepository.getAllMissionMediaRows(id);
            string missionCoverImageUrl = missionMedia?.MediaPath + missionMedia?.MediaName + missionMedia?.MediaType;
            return missionCoverImageUrl;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}