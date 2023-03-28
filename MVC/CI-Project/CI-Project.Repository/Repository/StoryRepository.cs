using CI_Project.Entities.DataModels;
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
				.ToList();
		}

		public StoryMedium getStoryMedia(long storyId)
		{
				return _db.StoryMedia.FirstOrDefault(sm => sm.StoryId == storyId)!;
		}
	}
}
