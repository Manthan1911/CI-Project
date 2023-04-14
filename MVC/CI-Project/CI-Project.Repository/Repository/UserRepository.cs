using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly CIProjectDbContext _db;

		public UserRepository(CIProjectDbContext db):base(db)
		{
			_db = db;
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

		public User findUser(long? id)
		{
			return _db.Users.FirstOrDefault(user => user.UserId == id)!;
		}
		public PasswordReset findUserByToken(string token)
		{
			return _db.PasswordResets.Where(row => row.Token == token).First();
		}

		public IEnumerable<User> getAllUsers()
		{
			return _db.Users;
		}

		public IEnumerable<User> getAllToRecommendMission()
		{
			return _db.Users.Include(u => u.MissionInviteFromUsers).Include(u => u.MissionInviteToUsers);
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
			//var DecryptedPassword = _pasword.Decode(user.Password);
			//return _db.Users.Any(u => u.Email.Equals(email) && DecryptedPassword.Equals(password));
			return false;
		}

		public List<Story> getAllStoriesOfCurrentUser(long userId)
		{
			return _db.Stories.Where(s => s.UserId == userId).ToList();
		}
	}
}
