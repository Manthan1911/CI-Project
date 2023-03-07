using CI_Platform_Web.Models;
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
        
        public HomeController(ILogger<HomeController> logger,IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login","Authentication");
            }

            List<Country> countryList = _homeRepository.getAllCountries();
            List<City> cityList = _homeRepository.getAllCities();
            List<MissionTheme> themeList = _homeRepository.getAllThemes();
            List<Skill> skillList = _homeRepository.getAllSkills();

            HomeModel model = new HomeModel()
            {
                countries = new(),
                cities = new(),
                themes= new(),
                skills=new(),
            };

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
                MissionThemeId= currentTheme.MissionThemeId,
                MissionThemeTitle = currentTheme.Title
            }));
            
            model.skills.AddRange(skillList.Select(currentSkill => new SkillModel
            {
                SkillId= currentSkill.SkillId,
                SkillName= currentSkill.SkillName,
            }));



            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}