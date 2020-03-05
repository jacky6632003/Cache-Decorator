using CacheDecorator.Common;
using CacheDecorator.Repository.Entities;
using CacheDecorator.Repository.Helper;
using CacheDecorator.Repository.Interface;
using CoreProfiler;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheDecorator.Repository.Implement
{
    public class FooRepository : IFooRepository
    {
        private readonly IDatabaseHelper _databaseHelper;

        public FooRepository(IDatabaseHelper databaseHelper)
        {
            this._databaseHelper = databaseHelper;
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentException">id</exception>
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var stepName = $"{nameof(FooRepository)}.DeleteAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (id.Equals(Guid.Empty))
                {
                    throw new ArgumentException(nameof(id));
                }

                var exists = await this.IsExistsAsync(id);
                if (exists.Equals(false))
                {
                    return new Result(false)
                    {
                        Message = "資料不存在"
                    };
                }

                using (var conn = (_databaseHelper.GetConnection(_databaseHelper.WLDOConnectionString)))
                {
                    var sqlCommand = new StringBuilder();
                    sqlCommand.AppendLine("begin tran");
                    sqlCommand.AppendLine("DELETE FROM [dbo].[Foo] ");
                    sqlCommand.AppendLine("WHERE FooId = @FooId ");
                    sqlCommand.AppendLine("commit");

                    var parameters = new DynamicParameters();
                    parameters.Add("FooId", id);

                    var executeResult = await conn.ExecuteAsync
                    (

                        sql: sqlCommand.ToString(),
                        param: parameters
                    );

                    IResult result = new Result(false);

                    if (executeResult.Equals(1))
                    {
                        result.Success = true;
                        result.AffectRows = executeResult;
                        return result;
                    }

                    result.Message = "資料刪除錯誤";
                    return result;
                }
            }
        }

        /// <summary>
        /// 以 Id 取得資料
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Foo.
        /// </returns>
        public async Task<FooModel> GetAsync(Guid id)
        {
            var stepName = $"{nameof(FooRepository)}.GetAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (id.Equals(Guid.Empty))
                {
                    throw new ArgumentNullException(nameof(id));
                }
                using (var conn = (_databaseHelper.GetConnection(_databaseHelper.WLDOConnectionString)))
                {
                    var sqlCommand = new StringBuilder();
                    sqlCommand.AppendLine("SELECT [FooId] ");
                    sqlCommand.AppendLine("      ,[Name] ");
                    sqlCommand.AppendLine("      ,[Description] ");
                    sqlCommand.AppendLine("      ,[Enable] ");
                    sqlCommand.AppendLine("      ,[CreateTime] ");
                    sqlCommand.AppendLine("      ,[UpdateTime] ");
                    sqlCommand.AppendLine("  FROM [dbo].[Foo] ");
                    sqlCommand.AppendLine("  Where ");
                    sqlCommand.AppendLine("    FooId = @FooId ;");

                    var parameters = new DynamicParameters();
                    parameters.Add("FooId", id);

                    var query = await conn.QueryFirstOrDefaultAsync<FooModel>
                    (

                        sql: sqlCommand.ToString(),
                        param: parameters
                    );

                    return query;
                }
            }
        }

        /// <summary>
        /// 取得指定範圍與數量的資料
        /// </summary>
        /// <param name="from">The from.</param>
        /// <param name="size">The size.</param>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns>
        /// List&lt;Foo&gt;.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// from
        /// or
        /// size
        /// </exception>
        public async Task<List<FooModel>> GetCollectionAsync(int @from, int size, bool displayAll = false)
        {
            var stepName = $"{nameof(FooRepository)}.GetCollectionAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (from <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(from));
                }

                if (size <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(size));
                }

                var totalCount = await this.GetTotalCountAsync(displayAll);
                if (totalCount.Equals(0))
                {
                    return new List<FooModel>();
                }

                if (from > totalCount)
                {
                    return new List<FooModel>();
                }
                using (var conn = (_databaseHelper.GetConnection(_databaseHelper.WLDOConnectionString)))
                {
                    var sqlCommand = new StringBuilder();
                    sqlCommand.AppendLine(@"select ");
                    sqlCommand.AppendLine("     [FooId] ");
                    sqlCommand.AppendLine("    ,[Name] ");
                    sqlCommand.AppendLine("    ,[Description] ");
                    sqlCommand.AppendLine("    ,[Enable] ");
                    sqlCommand.AppendLine("    ,[CreateTime] ");
                    sqlCommand.AppendLine("    ,[UpdateTime] ");
                    sqlCommand.AppendLine("FROM [dbo].[Foo] ");

                    if (displayAll.Equals(false))
                    {
                        sqlCommand.AppendLine("  where Enable = @Enable ");
                    }

                    sqlCommand.AppendLine(@"ORDER BY CreateTime DESC ");
                    sqlCommand.AppendLine(@"  OFFSET @OFFSET ROWS ");
                    sqlCommand.AppendLine(@"  FETCH NEXT @FETCH ROWS only; ");

                    var parameters = new DynamicParameters();

                    if (displayAll.Equals(false))
                    {
                        parameters.Add("Enable", true);
                    }

                    var pageSize = size < 10 || size > 100 ? 100 : size;
                    var start = @from <= 0 ? 1 : @from;

                    parameters.Add("OFFSET", start - 1);
                    parameters.Add("FETCH", pageSize);

                    var query = await conn.QueryAsync<FooModel>
                    (

                        sql: sqlCommand.ToString(),
                        param: parameters
                    );

                    var models = query.Any() ? query.ToList() : new List<FooModel>();
                    return models;
                }
            }
        }

        /// <summary>
        /// 取得資料總數
        /// </summary>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns>
        /// System.Int32.
        /// </returns>
        public async Task<int> GetTotalCountAsync(bool displayAll = false)
        {
            var stepName = $"{nameof(FooRepository)}.GetTotalCountAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                var sqlCommand = " SELECT count(p.FooId) FROM [Foo] p WITH (NOLOCK) ";

                if (displayAll.Equals(false))
                {
                    sqlCommand += " where p.Enable = @Enable ";

                    var parameters = new DynamicParameters();
                    parameters.Add("Enable", true);

                    using (var conn = (_databaseHelper.GetConnection(_databaseHelper.WLDOConnectionString)))
                    {
                        var queryResult = await conn.QueryFirstOrDefaultAsync<int>
                       (

                           sql: sqlCommand,
                           param: parameters
                       );

                        return queryResult;
                    }
                }
                using (var conn = (_databaseHelper.GetConnection(_databaseHelper.WLDOConnectionString)))
                {
                    var queryResult = await conn.QueryFirstOrDefaultAsync<int>
                   (

                       sql: sqlCommand
                   );

                    return queryResult;
                }
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        public async Task<IResult> InsertAsync(FooModel model)
        {
            var stepName = $"{nameof(FooRepository)}.InsertAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (model.EqualNull())
                {
                    throw new ArgumentNullException(nameof(model));
                }

                using (var conn = (_databaseHelper.GetConnection(_databaseHelper.WLDOConnectionString)))
                {
                    var sqlCommand = new StringBuilder();
                    sqlCommand.AppendLine("begin tran");
                    sqlCommand.AppendLine("INSERT INTO [dbo].[Foo]");
                    sqlCommand.AppendLine("(");
                    sqlCommand.AppendLine("  [FooId],");
                    sqlCommand.AppendLine("  [Name],");
                    sqlCommand.AppendLine("  [Description],");
                    sqlCommand.AppendLine("  [Enable],");
                    sqlCommand.AppendLine("  [CreateTime],");
                    sqlCommand.AppendLine("  [UpdateTime] ");
                    sqlCommand.AppendLine(")");
                    sqlCommand.AppendLine("VALUES");
                    sqlCommand.AppendLine("(");
                    sqlCommand.AppendLine("  @FooId,");
                    sqlCommand.AppendLine("  @Name,");
                    sqlCommand.AppendLine("  @Description,");
                    sqlCommand.AppendLine("  @Enable,");
                    sqlCommand.AppendLine("  @CreateTime,");
                    sqlCommand.AppendLine("  @UpdateTime ");
                    sqlCommand.AppendLine(");");
                    sqlCommand.AppendLine("commit");

                    var parameters = new DynamicParameters();
                    parameters.Add("FooId", model.FooId);

                    parameters.Add("Name", model.Name.IsNullOrWhiteSpace()
                                       ? string.Empty
                                       : model.Name.Trim().Length > 50
                                           ? model.Name.Trim().Substring(0, 50)
                                           : model.Name.Trim());

                    parameters.Add("Description", model.Description.IsNullOrWhiteSpace()
                                       ? string.Empty
                                       : model.Description.Trim().Length > 100
                                           ? model.Description.Trim().Substring(0, 100)
                                           : model.Description.Trim());

                    parameters.Add("Enable", model.Enable);
                    parameters.Add("CreateTime", model.CreateTime);
                    parameters.Add("UpdateTime", model.UpdateTime);

                    var executeResult = await conn.ExecuteAsync
                   (

                       sql: sqlCommand.ToString(),
                       param: parameters
                   );

                    IResult result = new Result(false);

                    if (executeResult.Equals(1))
                    {
                        result.Success = true;
                        result.AffectRows = executeResult;
                        return result;
                    }

                    result.Message = "資料新增錯誤";
                    return result;
                }
            }
        }

        /// <summary>
        /// 以 FooId 確認資料是否存在
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public async Task<bool> IsExistsAsync(Guid id)
        {
            var stepName = $"{nameof(FooRepository)}.IsExistsAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (id.Equals(Guid.Empty))
                {
                    throw new ArgumentNullException(nameof(id));
                }

                using (var conn = (_databaseHelper.GetConnection(_databaseHelper.WLDOConnectionString)))
                {
                    var sqlCommand = new StringBuilder();
                    sqlCommand.AppendLine(@" SELECT count(p.FooId) FROM [Foo] p ");
                    sqlCommand.AppendLine(@" WHERE ");
                    sqlCommand.AppendLine(@" p.FooId = @FooId ");

                    var parameters = new DynamicParameters();
                    parameters.Add("FooId", id);

                    var query = await conn.QueryFirstOrDefaultAsync<int>
                    (

                        sql: sqlCommand.ToString(),
                        param: parameters
                    );

                    var result = query > 0;
                    return result;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public async Task<IResult> UpdateAsync(FooModel model)
        {
            var stepName = $"{nameof(FooRepository)}.UpdateAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (model.EqualNull())
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var exists = await this.IsExistsAsync(model.FooId);
                if (exists.Equals(false))
                {
                    return new Result(false)
                    {
                        Message = "資料不存在"
                    };
                }
                using (var conn = (_databaseHelper.GetConnection(_databaseHelper.WLDOConnectionString)))
                {
                    var sqlCommand = new StringBuilder();
                    sqlCommand.AppendLine(" begin tran ");
                    sqlCommand.AppendLine(" Update [dbo].[Foo] ");
                    sqlCommand.AppendLine(" SET ");
                    sqlCommand.AppendLine(" Name = @Name ,");
                    sqlCommand.AppendLine(" Description = @Description ,");
                    sqlCommand.AppendLine(" Enable = @Enable ,");
                    sqlCommand.AppendLine(" UpdateTime = @UpdateTime ");
                    sqlCommand.AppendLine(" WHERE FooId = @FooId ");
                    sqlCommand.AppendLine(" commit ");

                    var parameters = new DynamicParameters();

                    parameters.Add("FooId", model.FooId);

                    parameters.Add("Name", model.Name.IsNullOrWhiteSpace()
                                       ? string.Empty
                                       : model.Name.Trim().Length > 50
                                           ? model.Name.Trim().Substring(0, 50)
                                           : model.Name.Trim());

                    parameters.Add("Description", model.Description.IsNullOrWhiteSpace()
                                       ? string.Empty
                                       : model.Description.Trim().Length > 100
                                           ? model.Description.Trim().Substring(0, 100)
                                           : model.Description.Trim());

                    parameters.Add("Enable", model.Enable);

                    parameters.Add("UpdateTime", model.UpdateTime);

                    var executeResult = await conn.ExecuteAsync
                    (

                        sql: sqlCommand.ToString(),
                        param: parameters
                    );

                    IResult result = new Result(false);

                    if (executeResult.Equals(1))
                    {
                        result.Success = true;
                        result.AffectRows = executeResult;
                        return result;
                    }

                    result.Message = "資料更新錯誤";
                    return result;
                }
            }
        }
    }
}