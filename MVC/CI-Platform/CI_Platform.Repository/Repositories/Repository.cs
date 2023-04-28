using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CiPlatformContext _db;
        public DbSet<T> dbSet;
        public Repository(CiPlatformContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void AddNew(T entity)
        {
            dbSet.Add(entity);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
        public bool ExistUser(Expression<Func<T, bool>> filter)
        {
            bool exist = dbSet.Any(filter);
            return exist;
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault()!;
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public IEnumerable<T> GetRecordsWhere(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query;
        }

        public void DeleteField(T entity)
        {
            dbSet.Remove(entity);
        }
    }

}
