using Azure;
using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CI_Project.Repository.Repository
{
	public class VolunteeringMissionRepository : IVolunteeringMissionRepository
	{
		public readonly IHomeRepository? _homeRepository;
		public readonly CIProjectDbContext _db;


		public VolunteeringMissionRepository(IHomeRepository homeRepository, CIProjectDbContext db)
		{
			_homeRepository = homeRepository;
			_db = db;
		}

		public Mission getCurrentMissionDetails(long? id)
		{
			return _db.Missions
				.Include(m => m.GoalMissions)
				.Include(m => m.MissionApplications)
				.Include(m => m.MissionMedia)
				.Include(m => m.FavouriteMissions)
				.Include(m => m.MissionRatings)
				.Include(m => m.MissionSkills).ThenInclude(sk => sk.Skill)
				.Include(m => m.Theme)
				.Include(m => m.City)
				.Include(m => m.Country)
				.FirstOrDefault(m => m.MissionId == id)!;
		}

		public void saveRating(MissionRating ratingObj)
		{
			var ratingMissionObj = _db.MissionRatings.FirstOrDefault(r => r.MissionId == ratingObj.MissionId && ratingObj.UserId == r.UserId);
			if (ratingMissionObj != null)
			{
				ratingMissionObj.Rating = ratingObj.Rating;
				_db.Update(ratingMissionObj);
			}
			else
			{
				_db.Add(ratingObj);
			}
			_db.SaveChanges();
		}

		public void addMissionToFavourite(FavouriteMission favouriteMissionObj)
		{
			var favObj = _db.FavouriteMissions.FirstOrDefault(f => favouriteMissionObj.UserId == f.UserId && favouriteMissionObj.MissionId == f.MissionId);
			if (favObj == null)
			{
				_db.Add(favouriteMissionObj);
			}
			_db.SaveChanges();
		}
		public void removeFromFavourite(FavouriteMission favouriteMissionObj)
		{
			var favObj = _db.FavouriteMissions.FirstOrDefault(f => favouriteMissionObj.UserId == f.UserId && favouriteMissionObj.MissionId == f.MissionId);
			if (favObj != null)
			{
				_db.Remove(favObj);
			}
			_db.SaveChanges();
		}

		public bool isMissionFavourite(long? userId, long? missionId)
		{
			var missions = _db.FavouriteMissions.FirstOrDefault(f => f.UserId == userId && f.MissionId == missionId);
			if (missions != null)
			{
				return true;
			}
			return false;
		}

		public List<Comment> getAllComments(long? missionId)
		{
			List<Comment> comments = _db.Comments.Include(m => m.User).Where(c => c.MissionId == missionId).ToList();
			return comments;
		}

		public void saveComment(Comment comment)
		{
			if (comment != null)
			{
				_db.Comments.Add(comment);
			}
			_db.SaveChanges();
		}

		public List<MissionApplication> getPaginatedRecentVolunteers(long? missionId, int pageNo, int pageSize)
		{
			return _db.MissionApplications
				.Where(application => application.MissionId == missionId)
				.Skip((pageNo - 1) * pageSize)
				.Take(pageSize)
				.Include(application => application.User)
				.ToList();
		}

		public List<MissionApplication> getRecentVolunteers(long? missionId)
		{
			return _db.MissionApplications
				.Where(application => application.MissionId == missionId)
					.ToList();
		}
		public List<Mission> getAllMissions()
		{
			return _db.Missions
				.Include(m => m.GoalMissions)
				.Include(m => m.MissionApplications)
				.Include(m => m.MissionMedia)
				.Include(m => m.FavouriteMissions)
				.Include(m => m.MissionRatings)
				.Include(m => m.MissionSkills).ThenInclude(sk => sk.Skill)
				.Include(m => m.Theme)
				.Include(m => m.City)
				.Include(m => m.Country)
				.ToList();

		}

		public void addToMissionInvite(MissionInvite obj)
		{
			_db.MissionInvites.Add(obj);
			_db.SaveChanges();
		}
	}
}
