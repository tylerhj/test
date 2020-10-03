using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;

namespace DiveCRM.Web.DAL.Base
{
    public interface IBaseRepositoryAsync<T>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>添加后的数据实体</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>添加后的数据实体</returns>
        Task<bool> AddOKAsync(T entity);

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateOKAsync(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        Task<bool> ExistAsync(Expression<Func<T, bool>> anyLambda);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        Task<T> FindAsync(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <typeparam name="S">排序</typeparam>
        /// <param name="whereLamdba">查询表达式</param>
        /// <returns></returns>
        Task<List<T>> FindListAsync(Expression<Func<T, bool>> whereLamdba);

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <typeparam name="S">排序</typeparam>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="orderLamdba">排序表达式</param>
        /// <returns></returns>
        Task<List<T>> FindListAsync<S>(Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba);

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <typeparam name="S">排序</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="orderLamdba">排序表达式</param>
        /// <returns></returns>
        Task<Tuple<List<T>, int>> FindPageListAsync<S>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba);
        Task<Tuple<PagedList<T>, int>> FindPageListAsync1<S>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, S>> orderLamdba);
        Task<List<T>> FindListBySQLAsync(string sql, params object[] parameters);

        Task<int> ExecuteBySQLAsync(string sql, params object[] parameters);
    }
}
