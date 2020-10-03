using Abp.Application.Services;
using Abp.EntityFrameworkCore;
using DiveCRM.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;

using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Webdiyer;
using Webdiyer.AspNetCore;

namespace DiveCRM.Web.DAL.Base
{
    public class BaseRepository<T> : BaseClass, IApplicationService, IBaseRepository<T> where T: class
    {
        public IDbContextProvider<DiveCRMDbContext> _provider;
        public BaseRepository(IDbContextProvider<DiveCRMDbContext> provider)
        {
            _provider = provider;
        }

        public virtual T Add(T entity)
        {
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Added;
            _provider.GetDbContext().SaveChanges();
            return entity;
        }

        public virtual bool AddOK(T entity)
        {
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Added;
            return _provider.GetDbContext().SaveChanges() > 0;            
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return _provider.GetDbContext().Set<T>().AsNoTracking().Count(predicate);
        }

        public virtual T Update(T entity)
        {
            _provider.GetDbContext().Set<T>().Attach(entity);
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Modified;
            _provider.GetDbContext().SaveChanges();
            return entity;
        }

        public virtual bool UpdateOK(T entity)
        {
            _provider.GetDbContext().Set<T>().Attach(entity);
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Modified;
            return _provider.GetDbContext().SaveChanges() > 0;
        }

        public virtual bool Delete(T entity)
        {
            _provider.GetDbContext().Set<T>().Attach(entity);
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Deleted;
           
            return _provider.GetDbContext().SaveChanges() > 0;
        }

        public virtual bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return _provider.GetDbContext().Set<T>().AsNoTracking().Any(anyLambda);
        }

        public virtual T Find(Expression<Func<T, bool>> whereLambda)
        {
            T _entity = _provider.GetDbContext().Set<T>().AsNoTracking().FirstOrDefault<T>(whereLambda);
            return _entity;
        }

        public virtual IQueryable<T> FindList(Expression<Func<T, bool>> whereLamdba)
        {
            var _list = _provider.GetDbContext().Set<T>().AsNoTracking().Where<T>(whereLamdba);
            return _list;
        }

        public virtual IQueryable<T> FindList<S>(Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba)
        {
            var _list = _provider.GetDbContext().Set<T>().AsNoTracking().Where<T>(whereLamdba);
            if (isAsc) _list = _list.OrderBy<T, S>(orderLamdba);
            else _list = _list.OrderByDescending<T, S>(orderLamdba);
            return _list;
        }

        public virtual IQueryable<T> FindPageList<S>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba)
        {
            var _list = _provider.GetDbContext().Set<T>().AsNoTracking().Where<T>(whereLamdba);
            totalRecord = _list.Count();
            if (isAsc) _list = _list.OrderBy<T, S>(orderLamdba).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            else _list = _list.OrderByDescending<T, S>(orderLamdba).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            return _list;
        }
        public virtual PagedList<T> FindPageList1<S>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba)
        {
            var _list = _provider.GetDbContext().Set<T>().AsNoTracking().Where<T>(whereLamdba);
            totalRecord = _list.Count();
            PagedList<T> resultList = null;
            if (isAsc) resultList = _list.OrderBy<T, S>(orderLamdba).ToPagedList(pageIndex,pageSize);
            else resultList = _list.OrderByDescending<T, S>(orderLamdba).ToPagedList(pageIndex, pageSize);
            return resultList;
        }

        public virtual int ExecuteBySQL(string sql, params object[] parameters)
        {
            var q = _provider.GetDbContext().Database.ExecuteSqlCommand(sql,parameters);
            return q;
        }

        public List<T> FindListBySQL<S>(string sql, params object[] parameters)
        {
            var list = _provider.GetDbContext().Set<T>().FromSqlRaw<T>(sql, parameters).ToList();
            return list;
        }

        T IBaseRepository<T>.Add(T entity)
        {
            throw new NotImplementedException();
        }

        bool IBaseRepository<T>.AddOK(T entity)
        {
            throw new NotImplementedException();
        }

        int IBaseRepository<T>.Count(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        T IBaseRepository<T>.Update(T entity)
        {
            throw new NotImplementedException();
        }

        bool IBaseRepository<T>.UpdateOK(T entity)
        {
            throw new NotImplementedException();
        }

        bool IBaseRepository<T>.Delete(T entity)
        {
            throw new NotImplementedException();
        }

        bool IBaseRepository<T>.Exist(Expression<Func<T, bool>> anyLambda)
        {
            throw new NotImplementedException();
        }

        T IBaseRepository<T>.Find(Expression<Func<T, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        IQueryable<T> IBaseRepository<T>.FindList(Expression<Func<T, bool>> whereLamdba)
        {
            throw new NotImplementedException();
        }

        IQueryable<T> IBaseRepository<T>.FindList<S>(Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba)
        {
            throw new NotImplementedException();
        }

        IQueryable<T> IBaseRepository<T>.FindPageList<S>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba)
        {
            throw new NotImplementedException();
        }

    }
}
