using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
	public class StoryListingController : Controller
	{
		private readonly IStoryRepository _storyRepository;
		private readonly IUserRepository _userRepository;
		public StoryListingController(IStoryRepository storyRepository,IUserRepository userRepository) 
		{
			_storyRepository = storyRepository;
			_userRepository = userRepository;
		}
		public IActionResult Index()
		{
			var userEmailId = HttpContext.Session.GetString("UserEmail");
			if (userEmailId == null)
			{
				return RedirectToAction("Login", "Authentication");
			}

			StoryListingModel storyListingModel = new()
			{
				User = _userRepository.findUser(userEmailId)
			};
			return View(storyListingModel);
		}

		public IActionResult getAllStories(int pageNo, int pageSize)
		{
			pageNo = pageNo <= 0 ? 1 : pageNo;
			pageSize = pageSize <= 0 ? 1 : pageSize;

			List<Story> stories = _storyRepository.getAllStories();

			List<StoryModel> storiesVm = new List<StoryModel>();

			foreach (var currStory in stories)
			{
				storiesVm.Add(ConvertStoryToStoryModel(currStory));
			}

			ViewBag.totalStories = storiesVm.Count;
			ViewBag.paginationLimit = pageSize;
			return PartialView("_StoryListing", storiesVm.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
		}


		public StoryModel ConvertStoryToStoryModel(Story story)
		{
			StoryModel storyModel = new()
			{
				StoryId=story.StoryId,
				UserId=story.UserId,
				MissionId=story.MissionId,
				User = story.User,
				Mission = story.Mission,
				StoryTitle=story.Title,
				StoryDescription=story.Description,
				Status= story.Status,
				PublishedAt	= story.PublishedAt,
				CreatedAt = story.CreatedAt,
				StoryMedia=story.StoryMedia,
				CoverImage=getCoverImage(story.StoryId),
			};
			return storyModel;
		}

		private string getCoverImage(long storyId)
		{
			StoryMedium StoryMediaObj = _storyRepository.getStoryMedia(storyId);
			return (StoryMediaObj.Path+StoryMediaObj.Type);
		}
	}
}
