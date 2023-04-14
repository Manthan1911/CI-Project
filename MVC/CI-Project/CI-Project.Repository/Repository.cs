using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CI_Project.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CIProjectDbContext _db;
        internal DbSet<T> table;
        public Repository(CIProjectDbContext db)
        {
            _db = db;
            table = _db.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return table.FirstOrDefault(filter);
        }

        public void Insert(T entity)
        {
            table.Add(entity);
        }

        public void Delete(T entity)
        {
            table.Remove(entity);
        }

        public void Update(T entity)
        {
            table.Update(entity);
        }

    }
}
