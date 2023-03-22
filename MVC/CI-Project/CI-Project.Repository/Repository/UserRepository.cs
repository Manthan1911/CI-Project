using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly CIProjectDbContext _db;
        private readonly IPassword _pasword;

        public UserRepository(CIProjectDbContext db, IPassword pasword)
        {
            _db = db;
            _pasword = pasword;
        }

        public bool addUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        public void addResetPasswordToken(PasswordReset passwordResetObj)
        {
            bool isAlreadyGenerated = _db.PasswordResets.Any(u => u.Email.Equals(passwordResetObj.Email));
            if (isAlreadyGenerated)
            {
                _db.Update(passwordResetObj);

            }
            else
            {
                _db.Add(passwordResetObj);
            }
            _db.SaveChanges();
        }

        public void removeResetPasswordToken(PasswordReset obj)
        {
            _db.Remove(obj);
            _db.SaveChanges();
        }

        public User findUser(string email)
        {
            return _db.Users.Where(user => user.Email.Equals(email)).First();
        }

        public User findUser(int? id)
        {
            return _db.Users.Where(user => user.UserId == id).First();
        }

        public PasswordReset findUserByToken(string token)
        {
            return _db.PasswordResets.Where( row => row.Token == token).First();
        }

        public IEnumerable<User> getAllUsers()
        {
            return _db.Users;
        }

        public bool updatePassword(User user)
        {
            _db.Update(user);
            _db.SaveChanges();
            return true;
        }

        public bool validateEmail(string email)
        {
            return _db.Users.Any(u => u.Email.Equals(email));    
        }

        public bool validateUser(string email, string password)
        {
            var user = findUser(email);
            var DecryptedPassword = _pasword.Decode(user.Password);
            return _db.Users.Any(u => u.Email.Equals(email) && DecryptedPassword.Equals(password));
        }
    }
}
