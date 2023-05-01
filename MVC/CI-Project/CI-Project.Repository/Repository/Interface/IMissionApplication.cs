using CI_Project.Entities.DataModels;
using Org.BouncyCastle.Asn1.Crmf;

namespace CI_Project.Repository.Repository.Interface
{
	public interface IMissionApplication
	{
		public List<MissionApplication> GetAllMissionApplicationsWithInclude();
		public MissionApplication GetMissionApplicationById(long missionApplicationId);

		public void AddMissionApplication(MissionApplication missionApplication);
		public void UpdaateMissionApplication(MissionApplication missionApplication);
	}
}
