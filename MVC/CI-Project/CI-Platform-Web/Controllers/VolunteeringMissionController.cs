using CI_Platform_Web.Utilities;
using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository;
using CI_Project.Repository.Repository.Interface;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Org.BouncyCastle.Bcpg;
using System;
using System.Runtime.InteropServices;

namespace CI_Platform_Web.Controllers
{
	enum commentStatus
	{
		APPROVED,
		REJECTED,
		PENDING,
	}
	[Authenticate]
	public class VolunteeringMissionController : Controller
	{
		public readonly IHomeRepository _homeRepository;
		public readonly IMissionMediaRepository _missionMediaRepository;
		public readonly IUserRepository _userRepository;
		public readonly IVolunteeringMissionRepository _volunteeringMissionRepository;
		public readonly IMissionApplication _missionApplicationRepository;
		public VolunteeringMissionController(IMissionMediaRepository missionMediaRepository, IMissionApplication missionApplicationRepository, IHomeRepository homeRepository, IVolunteeringMissionRepository volunteeringMissionRepository, IUserRepository userRepository)
		{
			_homeRepository = homeRepository;
			_volunteeringMissionRepository = volunteeringMissionRepository;
			_userRepository = userRepository;
			_missionMediaRepository = missionMediaRepository;
			_missionApplicationRepository = missionApplicationRepository;
		}

		public IActionResult Index(long? id)
		{
			var userEmailId = HttpContext.Session.GetString("UserEmail");
			if (userEmailId == null)
			{
				return RedirectToAction("Login", "Authentication");
			}

			VolunteeringMissionModel vmModel = new VolunteeringMissionModel();
			vmModel.user = _userRepository.findUser(userEmailId);
			vmModel.MissionModel = convertDataModelToMissionModel(_volunteeringMissionRepository.getCurrentMissionDetails(id), vmModel.user.UserId);
			return View(vmModel);
		}

		public MissionModel convertDataModelToMissionModel(Mission mission, long userId)
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
				StartDate = mission.StartDate,
				EndDate = mission.EndDate,
				OrganizationName = mission.OrganizationName,
				OrganizationDetails = mission.OrganizationDetail,
				MissionType = mission.MissionType,
				MissionSkills = mission.MissionSkill,
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
				MissionMedia = mission.MissionMedia,
				totalVolunteers = _missionApplicationRepository.GetAllMissionApplicationsWithInclude().Where(missionApplication => missionApplication.MissionId == mission.MissionId && missionApplication.ApprovalStatus.Equals("APPROVED")).ToList().Count(),
				isMissionAppliedByCurrentUser = _missionApplicationRepository.GetAllMissionApplicationsWithInclude().FirstOrDefault(missionApplication => missionApplication.MissionId == mission.MissionId && missionApplication.UserId == userId && missionApplication.ApprovalStatus.Equals("APPROVED")) != null ? true : false,
				isMissionApplicationPending = _missionApplicationRepository.GetAllMissionApplicationsWithInclude().FirstOrDefault(missionApplication => missionApplication.MissionId == mission.MissionId && missionApplication.UserId == userId && missionApplication.ApprovalStatus.Equals("PENDING")) != null ? true : false,
				isMissionApplicationDeclined = _missionApplicationRepository.GetAllMissionApplicationsWithInclude().FirstOrDefault(missionApplication => missionApplication.MissionId == mission.MissionId && missionApplication.UserId == userId && missionApplication.ApprovalStatus.Equals("DECLINED")) != null ? true : false,
			};
			if (missionModel.countOfRatingsByPeople != 0)
			{
				missionModel.avgRating = ((missionModel.sumOfRating % missionModel.countOfRatingsByPeople) != 0) ? ((missionModel.sumOfRating / missionModel.countOfRatingsByPeople) + 1) : (missionModel.sumOfRating / missionModel.countOfRatingsByPeople);
			}
			return missionModel;
		}

		public string? getMissionCoverImageUrl(long missionId)
		{
			var media = _missionMediaRepository.GetAllMissionMedia().FirstOrDefault(missionMedia => missionMedia.MissionId == missionId);
			string? missionCoverImageUrl = media?.MediaPath + media?.MediaName + media?.MediaType;
			return missionCoverImageUrl;
		}

		public void postRating(long userId, long missionId, byte rating)
		{

			MissionRating missionRatingObj = new()
			{
				UserId = userId,
				MissionId = missionId,
				Rating = rating
			};
			_volunteeringMissionRepository.saveRating(missionRatingObj);
		}

		public void addToFavourite(long userId, long missionId)
		{
			FavouriteMission favouriteMissionObj = new()
			{
				UserId = userId,
				MissionId = missionId,
			};
			_volunteeringMissionRepository.addMissionToFavourite(favouriteMissionObj);
		}
		public void removeFromFavourite(long userId, long missionId)
		{
			FavouriteMission favouriteMissionObj = new()
			{
				UserId = userId,
				MissionId = missionId,
			};
			_volunteeringMissionRepository.removeFromFavourite(favouriteMissionObj);
		}

		public bool isMissionFavourite(long userId, long missionId)
		{
			return _volunteeringMissionRepository.isMissionFavourite(userId, missionId);
		}


		public IActionResult loadAllComments(long? missionId)
		{
			List<Comment> comments = _volunteeringMissionRepository.getAllComments(missionId).Where(c => c.ApprovalStatus.ToLower() == "approved").ToList();

			List<CommentModel> commentsModel = new List<CommentModel>();
			foreach (Comment comment in comments)
			{
				commentsModel.Add(convertToCommentModel(comment));
			}

			return PartialView("_comments", commentsModel.OrderByDescending(c => c.CreatedAt).ToList());
		}

		public void postComment(long userId, long missionId, string commentText)
		{
			if (!commentText.IsNullOrEmpty())
			{
				Comment comment = new()
				{
					UserId = userId,
					MissionId = missionId,
					CommentText = commentText,
					CreatedAt = DateTime.Now,
					ApprovalStatus = commentStatus.APPROVED.ToString(),
				};

				_volunteeringMissionRepository.saveComment(comment);
			}
		}

		private CommentModel convertToCommentModel(Comment comment)
		{
			CommentModel commentVm = new()
			{
				CommentId = comment.CommentId,
				UserId = comment.UserId,
				User = comment.User,
				MissionId = comment.MissionId,
				Mission = comment.Mission,
				CommentText = comment.CommentText,
				CreatedAt = comment.CreatedAt,
				ApprovalStatus = comment.ApprovalStatus,

			};

			return commentVm;
		}

		public IActionResult getRecentVolunteers(long? missionId, int pageNo, int pageSize)
		{
			pageNo = pageNo <= 0 ? 1 : pageNo;
			pageSize = pageSize <= 0 ? 1 : pageSize;

			int volunteerStartIdx = ((pageNo - 1) * pageSize) + 1;
			int volunteerEndIdx = (volunteerStartIdx + pageSize) - 1;

			RecentVolunteersModel recentVm = new()
			{
				recentVolunteers = _volunteeringMissionRepository.getPaginatedRecentVolunteers(missionId, pageNo, pageSize),
				recentVolunteersCount = _volunteeringMissionRepository.getRecentVolunteers(missionId).Count(),
			};
			if (recentVm.recentVolunteersCount != 0)
			{

				recentVm.indexOfFirstVolunteerOfCurrPage = _volunteeringMissionRepository.getRecentVolunteers(missionId).IndexOf(recentVm.recentVolunteers[0]) + 1;
				recentVm.indexOfLastVolunteerOfCurrPage = _volunteeringMissionRepository.getRecentVolunteers(missionId).IndexOf(recentVm.recentVolunteers[recentVm.recentVolunteers.Count() - 1]) + 1;

			}
			return PartialView("_RecentVolunteers", recentVm);
		}

		public IActionResult getAllRelatedMissions(long missionId, long userId)
		{
			var themeId = _volunteeringMissionRepository.getAllMissions().FirstOrDefault(m => m.MissionId == missionId)?.ThemeId;
			var countryId = _volunteeringMissionRepository.getAllMissions().FirstOrDefault(m => m.MissionId == missionId)?.CountryId;
			var cityId = _volunteeringMissionRepository.getAllMissions().FirstOrDefault(m => m.MissionId == missionId)?.CityId;


			List<MissionModel> topThreeMissions = new List<MissionModel>();
			List<Mission> missions;

			missions = _volunteeringMissionRepository.getAllMissions().Where(m => m.ThemeId == themeId && m.MissionId != missionId).ToList();
			if (missions.Count < 3)
			{
				missions.AddRange(_volunteeringMissionRepository.getAllMissions().Where(m => m.CountryId == countryId && m.MissionId != missionId).ToList());
			}
			if (missions.Count < 3)
			{
				missions.AddRange(_volunteeringMissionRepository.getAllMissions().Where(m => m.CityId == cityId && m.MissionId != missionId).ToList());
			}

			if (missions.Count() > 0)
			{
				foreach (var currMission in missions)
				{
					topThreeMissions.Add(convertDataModelToMissionModel(currMission, userId));
				}
			}
			List<MissionModel> result = topThreeMissions.Skip(1).Take(3).ToList();
			return PartialView("_RelatedMissions", result);
		}

		public IActionResult getAllUsers(long missionId, long userId)
		{
			List<User> users = _userRepository.getAllToRecommendMission().ToList();
			List<UserModel> userVm = new();
			foreach (var user in users)
			{
				userVm.Add(convertToUserModel(user, missionId));
			}
			ViewBag.missionId = missionId;
			ViewBag.userId = userId;

			return PartialView("_RecommentToCoWorkers", userVm.Where(u => u.UserId != userId).ToList());
		}

		private UserModel convertToUserModel(User user, long missionId)
		{
			UserModel userVm = new()
			{
				UserId = user.UserId,
				FirstName = user.FirstName!,
				LastName = user.LastName!,
				EmailId = user.Email,
				missionInvitesTo = user.MissionInviteToUsers,
				missionInvitesFrom = user.MissionInviteFromUsers
			};
			return userVm;
		}

		public void addToMissionInvite(long[] userEmailList, long missionId, long userId)
		{
			User? currentUser = _userRepository.getAllUsers().FirstOrDefault(u => u.UserId == userId);

			var url = Url.Action("Index", "VolunteeringMission", new { id = missionId }, "https");
			string htmlMessage = $"<p style='text-align:center;font-size:2rem'>Your co-worker {currentUser?.FirstName} has recommended a mission to you.</p><p style='text-align:center;font-size:1.5rem'>Click on the link below check mission out</p><hr/>{url}";

			for (int i = 0; i < userEmailList.Length; i++)
			{
				MissionInvite missionInvite = new()
				{
					FromUserId = userId,
					ToUserId = userEmailList[i],
					MissionId = missionId,
					CreatedAt = DateTime.Now,
				};

				_volunteeringMissionRepository.addToMissionInvite(missionInvite);

				var userObj = _userRepository.getAllUsers().FirstOrDefault(u => u.UserId == userEmailList[i]);

				if (userObj != null)
				{
					//send mail
					try
					{
						var message = new MimeMessage();
						message.From.Add(new MailboxAddress("CI-Platform", "coder5255@gmail.com"));
						message.To.Add(new MailboxAddress("User", userObj.Email));
						message.Subject = "CI-Platform Recommended Mission";
						message.Body = new TextPart("html")
						{
							Text = $"<p style='font-size:1rem'>Your co-worker {currentUser?.FirstName} has recommended a mission to you.</p><p style='font-size:0.7rem;'>Click on the link </p>{url}"
						};
						using (var client = new SmtpClient())
						{
							client.Connect("smtp.gmail.com", 587, false);
							client.Authenticate("naruto.shipud2015@gmail.com", "yrxlcdynfxlqwsbx");
							client.Send(message);
							client.Disconnect(true);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
					}

				}
			}

		}
		public IActionResult ApplyMission(long userId, long missionId)
		{
			try
			{
				MissionApplication missionApplication = new MissionApplication()
				{
					UserId = userId,
					MissionId = missionId,
					AppliedAt = DateTime.Now,
					ApprovalStatus = ("pending").ToUpper(),
				};
				_missionApplicationRepository.AddMissionApplication(missionApplication);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				return StatusCode(500);
			}
			return NoContent();
		}

		public IActionResult RetriveMissionApplication(long userId, long missionId)
		{
			try
			{
				MissionApplication? application = _missionApplicationRepository.GetAllMissionApplicationsWithInclude().FirstOrDefault(missionApplication => missionApplication.MissionId == missionId && missionApplication.UserId == userId && missionApplication.ApprovalStatus.Equals("PENDING"));

				if (application != null)
				{
					_missionApplicationRepository.DeleteMissionApplication(application);
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
	}
}
