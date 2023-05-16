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
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return table.FirstOrDefault(filter);
        }
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await table.FirstOrDefaultAsync(filter);
        }
        public void Insert(T entity)
        {
            table.Add(entity);
        }
        public async Task InsertAsync(T entity)
        {
            await table.AddAsync(entity);
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
