using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IVolunteeringMissionRepository
    {
        public Mission getCurrentMissionDetails(long? id);
        
        public void saveRating(MissionRating ratingObj);
        
        public void addMissionToFavourite(FavouriteMission favouriteMissionObj);
        
        public void removeFromFavourite(FavouriteMission favouriteMissionObj);
		
        public bool isMissionFavourite(long? userId,long? missionId);

        public List<Comment> getAllComments(long? missionId);

        public void saveComment(Comment comment);
		public List<MissionApplication> getPaginatedRecentVolunteers(long? missionId,int pageNo,int pageSize);

        public List<MissionApplication> getRecentVolunteers(long? missionId);

        public List<Mission> getAllMissions();
        public void addToMissionInvite(MissionInvite obj);

	}
}
