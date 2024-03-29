﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        bool ExistUser(Expression<Func<T, bool>> filter);

        void AddNew(T entity);

        public IEnumerable<T> GetAll();

        T GetFirstOrDefault(Expression<Func<T, bool>> filter);

        public IEnumerable<T> GetRecordsWhere(Expression<Func<T, bool>> filter);

        void Update(T entity);

        void Save();
        
        public void DeleteField(T entity);
       
    }
}
