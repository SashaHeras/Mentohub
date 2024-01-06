using System.Linq.Expressions;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        public IQueryable<TEntity> GetAll();
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);

        public Task<TEntity> AddAsync(TEntity entity);

        public TEntity Add(TEntity entity);

        public TEntity Update(TEntity entity);

        void UpdateList(List<TEntity> entity);

        void AddList(List<TEntity> entity);

        public Task<TEntity> UpdateAsync(TEntity entity);

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression);
    }
}
