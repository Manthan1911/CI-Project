using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace CI_Platform_Web.Controllers
{
    public class VolunteeringMissionController : Controller
    {
        public readonly IHomeRepository _homeRepository;
        public readonly IUserRepository _userRepository;
        public readonly IVolunteeringMissionRepository _volunteeringMissionRepository;
        public VolunteeringMissionController(IHomeRepository homeRepository, IVolunteeringMissionRepository volunteeringMissionRepository, IUserRepository userRepository)
        {
            _homeRepository = homeRepository;
            _volunteeringMissionRepository = volunteeringMissionRepository;
            _userRepository = userRepository;
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
            vmModel.MissionModel = HomeController.convertDataModelToMissionModel(_volunteeringMissionRepository.getCurrentMissionDetails(id));

            return View(vmModel);
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
            return _volunteeringMissionRepository.isMissionFavourite(userId,missionId);
        }


        public IActionResult loadAllComments(long? missionId)
        {
            List<Comment> comments = _volunteeringMissionRepository.getAllComments(missionId);

            List<CommentModel> commentsModel = new List<CommentModel>();
            foreach (Comment comment in comments)
            {
                commentsModel.Add(convertToCommentModel(comment));
            }

            return PartialView("_comments",commentsModel);
        }

        public void postComment(long userId ,long missionId,string commentText)
        {
            if (!commentText.IsNullOrEmpty())
            {
				Comment comment = new()
				{
					UserId = userId,
					MissionId = missionId,
					CommentText = commentText,
					CreatedAt = DateTime.Now,
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
                MissionId= comment.MissionId,
                Mission = comment.Mission,
                CommentText = comment.CommentText,
                CreatedAt= comment.CreatedAt,
                ApprovalStatus= comment.ApprovalStatus,

            };

            return commentVm;
		}
	}
}
