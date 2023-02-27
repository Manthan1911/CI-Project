using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IUserRepository
    {
        public IEnumerable<User> getAllUsers();

        public Boolean validateEmail(string email);
        public Boolean validateUser(string email,string password );


    }
}
