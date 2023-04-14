using System.Linq.Expressions;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter);

        public void Insert(T entity);

        public void Update(T entity);

        public void Delete(T entity);

    }
}
