using Abp.EntityFrameworkCore;
using DiveCRM.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;

namespace DiveCRM.Web.DAL.Base
{
    public class BaseRepositoryAsync<T> : BaseRepository<T>, IBaseRepositoryAsync<T> where T : class
    {
        public BaseRepositoryAsync(IDbContextProvider<DiveCRMDbContext> provider):base(provider)
        {
            
        }

        public async Task<T> AddAsync(T entity)
        {
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Added;
            await _provider.GetDbContext().SaveChangesAsync();
            return entity;
        }

        public async Task<bool> AddOKAsync(T entity)
        {
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Added;
            var result = await _provider.GetDbContext().SaveChangesAsync() ;
            return result > 0;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _provider.GetDbContext().Set<T>().AsNoTracking().CountAsync(predicate);
        }

        public async Task<int> ExecuteBySQLAsync(string sql, params object[] parameters)
        {
            var q = await _provider.GetDbContext().Database.ExecuteSqlCommandAsync(sql, parameters);
            return q;
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> anyLambda)
        {
            return await _provider.GetDbContext().Set<T>().AsNoTracking().AnyAsync(anyLambda);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> whereLambda)
        {
            T _entity = await _provider.GetDbContext().Set<T>().AsNoTracking().FirstOrDefaultAsync<T>(whereLambda);
            return _entity;
        }

        public async Task<List<T>> FindListAsync(Expression<Func<T, bool>> whereLamdba)
        {
            var _list = await _provider.GetDbContext().Set<T>().AsNoTracking().Where<T>(whereLamdba).ToListAsync();
            return _list;
        }

        public async Task<List<T>> FindListAsync<S>(Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba)
        {
            var _list = _provider.GetDbContext().Set<T>().AsNoTracking().Where<T>(whereLamdba);
            if (isAsc) _list = _list.OrderBy<T, S>(orderLamdba);
            else _list = _list.OrderByDescending<T, S>(orderLamdba);
            return await _list.ToListAsync();
        }

        public async Task<List<T>> FindListBySQLAsync(string sql, params object[] parameters)
        {
            var list = _provider.GetDbContext().Set<T>().FromSqlRaw<T>(sql, parameters);
            return await list.ToListAsync();
        }

        public async Task<Tuple<PagedList<T>,int>> FindPageListAsync1<S>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba)
        {
            var _list = _provider.GetDbContext().Set<T>().AsNoTracking().Where<T>(whereLamdba);
            int totalRecord = _list.Count();
            PagedList<T> resultList = null;
            if (isAsc) resultList = await _list.OrderBy<T, S>(orderLamdba).ToPagedListAsync(pageIndex, pageSize);
            else resultList = await _list.OrderByDescending<T, S>(orderLamdba).ToPagedListAsync(pageIndex, pageSize);
            
            return  new Tuple<PagedList<T>, int>(resultList, totalRecord);
        }

        public async Task<Tuple<List<T>, int>> FindPageListAsync<S>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba)
        {
            var _list = _provider.GetDbContext().Set<T>().AsNoTracking().Where<T>(whereLamdba);
            int totalRecord = _list.Count();
            if (isAsc) _list = _list.OrderBy<T, S>(orderLamdba);
            else _list = _list.OrderByDescending<T, S>(orderLamdba);
            List<T> result = await _list.Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize).ToListAsync();

            return new Tuple<List<T>, int>(result,totalRecord);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _provider.GetDbContext().Set<T>().Attach(entity);
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Modified;
            await _provider.GetDbContext().SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateOKAsync(T entity)
        {
            _provider.GetDbContext().Set<T>().Attach(entity);
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Modified;
            return await _provider.GetDbContext().SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _provider.GetDbContext().Set<T>().Attach(entity);
            _provider.GetDbContext().Entry<T>(entity).State = EntityState.Deleted;
            int result = await _provider.GetDbContext().SaveChangesAsync();
            return result > 0;
        }
    }
}
