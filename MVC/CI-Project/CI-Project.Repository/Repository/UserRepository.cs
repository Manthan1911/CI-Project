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

        public bool addUser(User user)
        {
            try
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating user!" + ex);
            }
        }

        public User findUser(string email)
        {
            return _db.Users.Where(user => user.Email.Equals(email)).First();
        }

        public User findUser(int? id)
        {
            return _db.Users.Where(user => user.UserId == id).First();
        }

        public IEnumerable<User> getAllUsers()
        {
            return _db.Users;
        }

        public bool updatePassword(User user)
        {
            try
            {
                _db.Update(user);
                _db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception("Some error occured while saving!" + ex);
            }
            return true;
        }

        public bool validateEmail(string email)
        {
            return _db.Users.Any(u => u.Email.Equals(email));    
        }

        public bool validateUser(string email, string password)
        {
            return _db.Users.Any(u => u.Email.Equals(email) && u.Password.Equals(password));
        }
    }
}
