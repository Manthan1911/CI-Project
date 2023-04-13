using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Services.Interface
{
	public interface IPasswordService
	{

		public string Encode(string password);

		public string Decode(string EncryptedPassword);

	}
}
