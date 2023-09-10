namespace Mentohub.Core.Repositories.Intefaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        public IQueryable<TEntity> GetAll();

        public Task<TEntity> AddAsync(TEntity entity);

        public Task<TEntity> UpdateAsync(TEntity entity);
    }
}
