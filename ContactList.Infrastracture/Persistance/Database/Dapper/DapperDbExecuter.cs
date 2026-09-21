using ContactList.Infrastracture.Persistance.Database.Dapper.Models;
using Dapper;
using System.Data;

namespace ContactList.Infrastracture.Persistance.Database.Dapper
{
    public sealed class DapperDbExecuter : IDatabaseExecuter
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DapperDbExecuter(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default)
        {
            using IDbConnection connection = await CreateDbConnection(cancellationToken);

            var command = CreateCommand(
                sql,
                parameters,
                commandType,
                cancellationToken);

            return await connection.QueryAsync<T>(command);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default)
        {
            using IDbConnection connection = await CreateDbConnection(cancellationToken);

            var command = CreateCommand(
                sql,
                parameters,
                commandType,
                cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<T>(command);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default)
        {
            using IDbConnection connection = await CreateDbConnection(cancellationToken);

            var command = CreateCommand(
                sql,
                parameters,
                commandType,
                cancellationToken);

            return await connection.QueryFirstOrDefaultAsync<T>(command);
        }

        public async Task<int> ExecuteAsync(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default)
        {
            using IDbConnection connection = await CreateDbConnection(cancellationToken);

            var command = CreateCommand(
                sql,
                parameters,
                commandType,
                cancellationToken);

            return await connection.ExecuteAsync(command);
        }

        public async Task<T?> ExecuteScalarAsync<T>(
            string sql,
            DapperParameterCollection? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default)
        {
            using IDbConnection connection = await CreateDbConnection(cancellationToken);

            var command = CreateCommand(
                sql,
                parameters,
                commandType,
                cancellationToken);

            return await connection.ExecuteScalarAsync<T>(command);
        }

        #region Private methods
        private static CommandDefinition CreateCommand(
            string sql,
            DapperParameterCollection? parameters,
            CommandType commandType,
            CancellationToken cancellationToken)
        {
            return new CommandDefinition(
                commandText: sql,
                parameters: BuildParameters(parameters),
                commandType: commandType,
                cancellationToken: cancellationToken);
        }

        private static DynamicParameters BuildParameters(
            DapperParameterCollection? parameters)
        {
            var dynamicParameters = new DynamicParameters();

            if (parameters is null)
                return dynamicParameters;

            foreach (var parameter in parameters.Parameters)
            {
                dynamicParameters.Add(
                    parameter.Name,
                    parameter.Value,
                    parameter.DbType,
                    parameter.Direction,
                    parameter.Size,
                    parameter.Precision,
                    parameter.Scale);
            }

            return dynamicParameters;
        }

        private async Task<IDbConnection> CreateDbConnection(CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.CreateConnection();

            await OpenConnectionAsync(connection, cancellationToken);
            return connection;
        }
        private static async Task OpenConnectionAsync(
            IDbConnection connection,
            CancellationToken cancellationToken)
        {
            if (connection.State == ConnectionState.Open)
                return;

            if (connection is System.Data.Common.DbConnection dbConnection)
            {
                await dbConnection.OpenAsync(cancellationToken);
                return;
            }

            connection.Open();
        }
        #endregion
    }
}
