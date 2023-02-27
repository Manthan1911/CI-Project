using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly CIProjectDbContext _db;

        public UserRepository(CIProjectDbContext db)
        {
            _db = db;
        }

        public IEnumerable<User> getAllUsers()
        {
            return _db.Users;
        }

        public bool validateEmail(string email)
        {
            return _db.Users.Any(u => u.Email == email);    
        }

        public bool validateUser(string email, string password)
        {
            return _db.Users.Any(u => u.Email == email && u.Password == password);
        }
    }
}
