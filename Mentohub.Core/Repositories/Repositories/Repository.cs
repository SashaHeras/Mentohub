using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace Mentohub.Core.Repositories.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ProjectContext repositoryContext;

        // Конструктор
        public Repository(ProjectContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return repositoryContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($" We can`t get entites!!!\n {ex.Message}");
            }
        }

        #region Simple add/update

        public TEntity Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Add)} entity must not be null!!!");
            }

            repositoryContext.Add(entity);
            repositoryContext.SaveChanges();

            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} entity must not be null!!!");
            }

            repositoryContext.Update(entity);
            repositoryContext.SaveChanges();

            return entity;
        }

        public void UpdateList(List<TEntity> entity)
        {
            foreach(var e in entity)
            {
                repositoryContext.Update(e);
            }

            repositoryContext.SaveChanges();
        }

        public void AddList(List<TEntity> entity)
        {
            foreach (var e in entity)
            {
                repositoryContext.Add(e);
            }

            repositoryContext.SaveChanges();
        }

        #endregion

        #region Async/await add/update

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null!!!");
            }

            try
            {
                await repositoryContext.AddAsync(entity);
                await repositoryContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null!!!");
            }

            try
            {
                repositoryContext.ChangeTracker.Clear();
                repositoryContext.Update(entity);
                await repositoryContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        #endregion

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return repositoryContext.Set<TEntity>().Where(expression).FirstOrDefault();
        }
    }
}
