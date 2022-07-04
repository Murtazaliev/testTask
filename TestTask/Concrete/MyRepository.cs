using Microsoft.EntityFrameworkCore;
using TestTaskDomain.Models;
using TestTaskDomain.Abstract;

namespace TestTaskDomain.Concrete
{
    public class MyRepository : IRepository
    {
        private readonly MyContext _context;

        public MyRepository(MyContext context)
        {
            _context = context;
        }
        public IQueryable<CompanySubdivision> CompanySubdivisions => _context.CompanySubdivisions;
        #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Универсальные методы CRUD ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

        /// <summary>
        /// Создание универсального метода вставки
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// Запись нескольких строк в БД
        /// </summary>
        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (TEntity entity in entities)
            {
                _context.Entry(entity).State = EntityState.Added;
            }
        }

        /// <summary>
        /// Универсальный метод обновления
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var transaction = _context.Database.BeginTransaction();
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Универсальный метод обновления
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) 
            where TEntity : class
        {
            using var transaction = _context.Database.BeginTransaction();
            foreach (TEntity entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            _context.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Универсальный метод удаления данных
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Remove<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var transaction = _context.Database.BeginTransaction();
            _context.Entry<TEntity>(entity).State = EntityState.Deleted;
            _context.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Универсальный метод удаления данных
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            using var transaction = _context.Database.BeginTransaction();
            foreach (TEntity entity in entities)
            {
                _context.Entry<TEntity>(entity).State = EntityState.Deleted;
            }
            _context.SaveChanges();
            transaction.Commit();   
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Transaction()
        {
           
        }
        #endregion
    }
}
