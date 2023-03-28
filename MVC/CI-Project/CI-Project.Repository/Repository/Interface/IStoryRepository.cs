using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface IStoryRepository
	{
		public List<Story> getAllStories();
		public StoryMedium getStoryMedia(long storyId);
	}
}
