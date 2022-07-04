using TestTaskDomain.Models;

namespace TestTaskDomain.Abstract
{
    public interface IRepository
    {
        IQueryable<CompanySubdivision> CompanySubdivisions { get; }
        #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Универсальные методы CRUD  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entityes) where TEntity : class;
        void Save();
        void Transaction();
        #endregion
    }
}
