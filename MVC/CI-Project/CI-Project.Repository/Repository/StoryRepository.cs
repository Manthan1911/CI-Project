using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
	public class StoryRepository : IStoryRepository
	{
		private readonly CIProjectDbContext _db;

		public StoryRepository(CIProjectDbContext db)
		{
			_db = db;
		}

		public List<Story> getAllStories()
		{
			return _db.Stories
			.Include(s => s.User)
			.Include(s => s.Mission)
			.ThenInclude(s => s.Theme)
			.Include(s => s.StoryMedia)
			.ToList();
		}
		public Story? getStory(long storyId)
		{
			return _db.Stories.Include(s => s.User).Include(s => s.StoryMedia).FirstOrDefault(s => s.StoryId == storyId);
		}
		public StoryMedium getStoryMedia(long storyId)
		{
			return _db.StoryMedia.FirstOrDefault(sm => sm.StoryId == storyId)!;
		}

		public List<StoryMedium> getAllMediaOfStory(long? storyId)
		{
			return _db.StoryMedia.Where(sm => sm.StoryId == storyId).ToList();
		}
		public List<MissionModel> getAllAppliedMissionsOfCurrentUser(long userId)
		{
			var missionIds = _db.MissionApplications.Where(ma => ma.UserId == userId).Select(ma => ma.MissionId).ToList();

			List<MissionModel> missions = new List<MissionModel>();

			foreach (var currMissionId in missionIds)
			{
				Mission mission = _db.Missions.FirstOrDefault(m => m.MissionId.Equals(currMissionId))!;
				missions.Add(new MissionModel()
				{
					MissionId = mission.MissionId,
					Title = mission.Title,
				});
			}

			return missions;
		}

		public Story getDraftedStory(long userId)
		{
			return _db.Stories.FirstOrDefault(s => (s.UserId == userId) && (s.Status.ToLower().Equals("draft")))!;
		}

		public int saveStory(Story story)
		{
			_db.Add(story);
			var isStorySaved = _db.SaveChanges();
			return isStorySaved;
		}
		public int updateStory(Story story)
		{
			_db.Update(story);
			var isStoryUpdated = _db.SaveChanges();
			return isStoryUpdated;
		}
		public int saveStoryMedia(StoryMedium storyMedia)
		{
			_db.Add(storyMedia);
			var isStoryMediaSaved = _db.SaveChanges();
			return isStoryMediaSaved;
		}

		public void deleteAllMediaOfStory(List<StoryMedium> storyMedia)
		{
			_db.RemoveRange(storyMedia);
			_db.SaveChanges();
		}

		public void deleteStory(Story story)
		{
			_db.Stories.Remove(story);
			_db.SaveChanges();
		}

		public IEnumerable<User> getAllUsersToRecommendStory()
		{
			return _db.Users.Include(u => u.StoryInviteToUsers).Include(u => u.StoryInviteFromUsers);
		}

		public void addDataToStoryInvite(StoryInvite storyInvite)
		{
			_db.StoryInvites.Add(storyInvite);
			_db.SaveChanges();
		}

		public void incrementStoryView(long storyId)
		{
			Story? story = _db.Stories.FirstOrDefault(s => s.StoryId == storyId);
			if (story != null)
			{
				story.Views += 1; 
				_db.Stories.Update(story);
				_db.SaveChanges();
			}
		}

		public List<StoryInvite> GetAllStoryInvites()
		{
			return _db.StoryInvites.ToList();
		}

		public void DeleteAllStoryInvite(long storyId)
		{
			List<StoryInvite> invites = GetAllStoryInvites().Where(storyInvite => storyInvite.StoryId == storyId).ToList();
			_db.StoryInvites.RemoveRange(invites);
			_db.SaveChanges();	
		}
	}
}
