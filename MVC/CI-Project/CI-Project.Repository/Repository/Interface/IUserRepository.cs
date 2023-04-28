using CI_Project.Entities.DataModels;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IUserRepository
    {
        public IEnumerable<User> getAllUsers();

        public Boolean validateEmail(string email);
        public Boolean validateUser(string email,string password );

        public Boolean IsUserAdmin(string email);
        public List<Admin> GetAllAdmin();

        public User findUser(string email);

        public User findUser(int? id);
        public User findUser(long? id);

        public PasswordReset findUserByToken(string token);


        public Boolean updatePassword(User user);

        public void updateUser(User user);
        public Boolean addUser(User user);
        public void deleteUser(User user);


        public void addResetPasswordToken(PasswordReset passwordResetObj);

        public void removeResetPasswordToken(PasswordReset obj);

		public List<Story> getAllStoriesOfCurrentUser(long userId);

        public IEnumerable<User> getAllToRecommendMission();

        public void saveContactUsData(ContactU conatactUsObj);

	}
}
