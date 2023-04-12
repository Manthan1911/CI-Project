using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository;
using CI_Project.Repository.Repository.Interface;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace CI_Platform_Web.Controllers
{
	public class StoryListingController : Controller
	{
		private readonly IStoryRepository _storyRepository;
		private readonly IUserRepository _userRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public StoryListingController(IStoryRepository storyRepository, IUserRepository userRepository, IWebHostEnvironment webHostEnvironment)
		{
			_storyRepository = storyRepository;
			_userRepository = userRepository;
			_webHostEnvironment = webHostEnvironment;
		}
		//------------------------------------ StoryListing ------------------------------------

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

			List<Story> stories = _storyRepository.getAllStories().Where(s => s.Status.ToLower().Equals("approved")).ToList();

			List<StoryModel> storiesVm = new List<StoryModel>();

			foreach (var currStory in stories)
			{
				storiesVm.Add(ConvertStoryToStoryModel(currStory));
			}

			ViewBag.totalStories = storiesVm.Count;
			ViewBag.paginationLimit = pageSize;
			return PartialView("_StoryListing", storiesVm.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
		}

		//------------------------------------ Share Story ------------------------------------

		[HttpGet]
		public IActionResult ShareStory(long userId)
		{
			List<MissionModel> appliedMissionsOfUser = _storyRepository.getAllAppliedMissionsOfCurrentUser(userId);

			if (appliedMissionsOfUser.Count() <= 0)
			{
				return RedirectToAction("Index", "StoryListing");
			}

			StoryModel storyVm = new();

			Story draftedStory = _storyRepository.getDraftedStory(userId);
			if (draftedStory != null)
			{
				storyVm = ConvertStoryToStoryModel(draftedStory);
				storyVm.Missions = appliedMissionsOfUser;
				storyVm.StoryMedia = _storyRepository.getAllMediaOfStory(storyVm.StoryId);
				storyVm.StoriesOfCurrentUser = _userRepository.getAllStoriesOfCurrentUser(storyVm.UserId);
				storyVm.IsStoryDraft = 1;
			}
			else
			{
				storyVm.Missions = appliedMissionsOfUser;
				storyVm.IsStoryDraft = 0;
			}
			return View(storyVm);
		}

		[HttpPost]
		public IActionResult ShareStory(StoryModel storyVm, string action, List<IFormFile> files, long? storyId)
		{
			if (files.Count() > 20)
			{
				ModelState.AddModelError("StoryMedia", "Images cant be more than 20 !");
			}
			var isStorySaved = 0;
			if (ModelState.IsValid)
			{
				
				if (action != null)
				{
					Story story = new()
					{
						Title = storyVm.StoryTitle,
						Description = storyVm.StoryDescription,
						MissionId = storyVm.MissionId,
						UserId = storyVm.UserId,
						Status = action.ToLower().Equals("share") ? ("pending").ToUpper() : ("draft").ToUpper(),
						CreatedAt = DateTime.Now,
						VideoUrl = storyVm.VideoUrl,
						Views = storyVm.Views,
					};

					var draftedStory = _storyRepository.getDraftedStory(storyVm.UserId);

					if (draftedStory != null)
					{
						draftedStory.Status = action.ToLower().Equals("share") ? ("pending").ToUpper() : ("draft").ToUpper();
						draftedStory.Title = storyVm.StoryTitle;
						draftedStory.Description = storyVm.StoryDescription;
						draftedStory.UserId = storyVm.UserId;
						draftedStory.MissionId = storyVm.MissionId;
						draftedStory.VideoUrl = storyVm.VideoUrl;
						draftedStory.UpdatedAt = DateTime.Now;
						var isStoryUpdated = _storyRepository.updateStory(draftedStory);
					}
					else
					{
						isStorySaved = _storyRepository.saveStory(story);
					}

					if (storyId != null && storyId > 0)
					{
						List<StoryMedium> storyMedia = _storyRepository.getAllMediaOfStory(storyId);
						if (storyMedia.Count() > 0)
						{
							removeStoryMediaFromDatabase(storyMedia);
							deleteImagesFromWebRoot(storyMedia);
						}
					}
					if (files.Count() > 0)
					{
						copyFilesToWebRootAndDatabase(files, storyVm.UserId, storyVm.MissionId);
					}

				}
			}
			else
			{
				List<MissionModel> appliedMissionsOfUser;
				if (storyVm.UserId > 0)
				{
					appliedMissionsOfUser = _storyRepository.getAllAppliedMissionsOfCurrentUser(storyVm.UserId);
					storyVm.Missions = appliedMissionsOfUser;
				}
				return View(storyVm);
			}
			return RedirectToAction("Index");
		}

		private void copyFilesToWebRootAndDatabase(List<IFormFile> files, long userId, long missionId)
		{
			string wwwRootPath = _webHostEnvironment.WebRootPath;
			foreach (var f in files)
			{
				string fileName = Guid.NewGuid().ToString();
				var uploads = Path.Combine(wwwRootPath, @"images\storyimages");
				var extension = Path.GetExtension(f?.FileName);

				using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
				{
					f?.CopyTo(fileStrems);
				}
				var storyID = _storyRepository.getAllStories().OrderByDescending(s => s.CreatedAt).FirstOrDefault(s => s.UserId == userId && s.MissionId == missionId)!.StoryId;
				StoryMedium storyMediaObj = new()
				{
					Path = @"\images\storyimages\" + fileName,
					Type = extension!,
					StoryId = storyID,
				};

				var isStoryMediaSaved = _storyRepository.saveStoryMedia(storyMediaObj);

			}
		}

		private void deleteImagesFromWebRoot(List<StoryMedium> storyMedia)
		{
			if (storyMedia != null)
			{

				string wwwRootPath = _webHostEnvironment.WebRootPath;
				var path = $@"{wwwRootPath}\images\storyimages";

				foreach (var m in storyMedia)
				{
					var filePath = m.Path.Remove(0, 20);
					var fileType = m.Type;

					var url = Path.Combine(path, filePath + fileType);
					System.IO.File.Delete(url);
				}
			}
		}

		private void removeStoryMediaFromDatabase(List<StoryMedium> storyMedia)
		{
			if (storyMedia != null)
			{
				_storyRepository.deleteAllMediaOfStory(storyMedia);
			}
		}

		public IActionResult clearDraft(long storyId)
		{
			if (storyId > 0)
			{
				Story? draftedStory = _storyRepository.getStory(storyId);
				if (draftedStory != null)
				{
					List<StoryMedium> storyMedia = _storyRepository.getAllMediaOfStory(storyId);
					removeStoryMediaFromDatabase(storyMedia);
					deleteImagesFromWebRoot(storyMedia);
					_storyRepository.deleteStory(draftedStory);
				}
			}
			return RedirectToAction("Index");
		}

		//------------------------------------ StoryDetails ------------------------------------
		[HttpGet]
		public IActionResult StoryDetails(long storyId)
		{
			var userEmailId = HttpContext.Session.GetString("UserEmail");
			if (userEmailId == null)
			{
				return RedirectToAction("Login", "Authentication");
			}

			if (storyId <= 0)
			{
				return RedirectToAction("Index", "StoryListing");
			}
			Story? currentStory = _storyRepository.getStory(storyId);

			if (currentStory != null && currentStory.Status.ToLower().Equals("approved"))
			{
				_storyRepository.incrementStoryView(storyId);
				StoryDetailsModel currStoryDetails = new StoryDetailsModel();
				currStoryDetails.Story = ConvertStoryToStoryModel(currentStory);
				User user = _userRepository.findUser(currStoryDetails.Story?.UserId);
				currStoryDetails.User = ConvertUserToUserModel(user);
				return View(currStoryDetails);
			}
				
			return RedirectToAction("Index", "StoryListing");
		}



		// -----------------------------------
		public StoryModel ConvertStoryToStoryModel(Story story)
		{
			StoryModel storyModel = new()
			{
				StoryId = story.StoryId,
				UserId = story.UserId,
				MissionId = story.MissionId,
				User = story.User,
				Mission = story.Mission,
				StoryTitle = story.Title,
				StoryDescription = story.Description,
				Status = story.Status,
				PublishedAt = story.PublishedAt,
				CreatedAt = story.CreatedAt,
				StoryMedia = story.StoryMedia,
				Views = story.Views,
				VideoUrl = story.VideoUrl,
			};

			if (story.StoryMedia.Count() > 0)
			{
				storyModel.CoverImage = getCoverImage(story.StoryId);
			}
			return storyModel;
		}

		private UserModel ConvertUserToUserModel(User user)
		{
			UserModel userVm = new()
			{
				UserId = user.UserId,
				EmailId = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				WhyIVolunteer = user.WhyIVolunteer,
				missionInvitesTo=user.MissionInviteToUsers,
				missionInvitesFrom=user.MissionInviteFromUsers
			};
			return userVm;
		}

		private string getCoverImage(long storyId)
		{
			StoryMedium StoryMediaObj = _storyRepository.getStoryMedia(storyId);
			return (StoryMediaObj.Path + StoryMediaObj.Type);
		}


		public IActionResult getAllUsersForRecommendingStory(long userId)
		{
			List<User> users = _storyRepository.getAllUsersToRecommendStory().ToList();
			List<UserModel> userVm = new();
			foreach (var user in users)
			{
				userVm.Add(ConvertUserToUserModel(user));
			}

			return PartialView("_RecommentToCoWorkers", userVm.Where(u => u.UserId != userId).ToList());
		}



		public void addDataToStoryInvite(long[] userIdList, long storyId , long userId)
		{
			User? currentUser = _storyRepository.getAllUsersToRecommendStory().FirstOrDefault(u => u.UserId == userId);

			var url = Url.Action("StoryDetails", "StoryListing", new { storyId = storyId }, "https");
			string htmlMessage = $"<p style='text-align:center;font-size:2rem'>Your co-worker {currentUser?.FirstName} has recommended a story to you.</p><p style='text-align:center;font-size:1.5rem'>Click on the link below check mission out</p><hr/>{url}";

			for (int i = 0; i < userIdList.Length; i++)
			{
				
				StoryInvite storyInvite = new()
				{
					FromUserId = userId,
					ToUserId = userIdList[i],
					StoryId= storyId,
					CreatedAt= DateTime.Now,
				};

				_storyRepository.addDataToStoryInvite(storyInvite);
				

				var userObj = _storyRepository.getAllUsersToRecommendStory().FirstOrDefault(u => u.UserId == userIdList[i]);

				if (userObj != null)
				{
					//send mail
					try
					{
						var message = new MimeMessage();
						message.From.Add(new MailboxAddress("CI-Platform", "coder5255@gmail.com"));
						message.To.Add(new MailboxAddress("User", userObj.Email));
						message.Subject = "CI-Platform Recommended Story";
						message.Body = new TextPart("html")
						{
							Text = $"<p style='font-size:1rem'>Your co-worker {currentUser?.FirstName} has recommended a story to you.</p><p style='font-size:0.7rem;'>Click on the link </p>{url}"
						};
						using (var client = new SmtpClient())
						{
							client.Connect("smtp.gmail.com", 587, false);
							client.Authenticate("coder5255@gmail.com", "");
							client.Send(message);
							client.Disconnect(true);
						}
					}
					catch (Exception ex)
					{

					}

				}
			}
		}
	}
}
