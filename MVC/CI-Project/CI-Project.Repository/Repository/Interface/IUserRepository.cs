using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IUserRepository
    {
        public IEnumerable<User> getAllUsers();

        public Boolean validateEmail(string email);
        public Boolean validateUser(string email,string password );

        public User findUser(string email);

        public User findUser(int? id);

        public Boolean updatePassword(User user);

        public Boolean addUser(User user);

    }
}
