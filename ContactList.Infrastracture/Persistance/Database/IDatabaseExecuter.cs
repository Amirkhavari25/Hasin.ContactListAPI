using ContactList.Infrastracture.Persistance.Database.Dapper.Models;
using System.Data;

namespace ContactList.Infrastracture.Persistance.Database
{
    public interface IDatabaseExecuter
    {
        Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default);

        Task<T?> QuerySingleOrDefaultAsync<T>(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default);

        Task<T?> QueryFirstOrDefaultAsync<T>(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default);

        Task<int> ExecuteAsync(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default);

        Task<T?> ExecuteScalarAsync<T>(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default);
    }
}
