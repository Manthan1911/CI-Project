using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface IStoryRepository
	{
		public List<Story> getAllStories();
		public StoryMedium getStoryMedia(long storyId);

		public List<MissionModel> getAllAppliedMissionsOfCurrentUser(long userId);

		public List<StoryMedium> getAllMediaOfStory(long? storyId);
		
		public void deleteAllMediaOfStory(List<StoryMedium> storyMedia);


		public Story getDraftedStory(long userId);

		public int saveStory(Story story);

		public int updateStory(Story story);

		public int saveStoryMedia(StoryMedium storyMedia);

		public Story? getStory(long storyId);
		public void deleteStory(Story story);

		public IEnumerable<User> getAllUsersToRecommendStory();
		public void addDataToStoryInvite(StoryInvite storyInvite);
		public void incrementStoryView(long storyId);
	}
}
