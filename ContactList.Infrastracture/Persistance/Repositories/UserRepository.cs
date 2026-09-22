using ContactList.Application.Contracts.Persistance;
using ContactList.Domain.Entities;
using ContactList.Infrastracture.Persistance.Database;
using ContactList.Infrastracture.Persistance.Database.Dapper.Models;
using ContactList.Infrastracture.Persistance.QueryModels;
using System.Data;

namespace ContactList.Infrastracture.Persistance.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IDatabaseExecuter _databaseExecuter;

        public UserRepository(IDatabaseExecuter databaseExecuter)
        {
            _databaseExecuter = databaseExecuter;
        }

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            const string sql = """
            INSERT INTO Users
            (
                Id,
                Email,
                Username,
                Mobile,
                PasswordHash,
                CreateDate,
                UpdateDate,
                IsDeleted
            )
            VALUES
            (
                @Id,
                @Email,
                @Username,
                @Mobile,
                @PasswordHash,
                @CreateDate,
                @UpdateDate,
                @IsDeleted
            );
            """;

            var parameters = new DapperParameterCollection()
                .Add("@Id", user.Id, DbType.Guid)
                .Add("@Email", user.Email.Value, DbType.String)
                .Add("@Username", user.Username, DbType.String)
                .Add("@Mobile", user.Mobile.Value, DbType.String)
                .Add("@PasswordHash", user.PasswordHash, DbType.String)
                .Add("@CreateDate", user.CreateDate, DbType.DateTime2)
                .Add("@UpdateDate", user.UpdateDate, DbType.DateTime2)
                .Add("@IsDeleted", user.IsDeleted, DbType.Boolean);

            await _databaseExecuter.ExecuteAsync(
                sql,
                parameters,
                cancellationToken: ct);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            const string sql = """
            SELECT
                Id,
                Email,
                Username,
                Mobile,
                PasswordHash,
                CreateDate,
                UpdateDate,
                IsDeleted
            FROM Users
            WHERE Email = @Email
              AND IsDeleted = 0;
            """;

            var parameters = new DapperParameterCollection()
                .Add("@Email", email, DbType.String);

            var model =
                await _databaseExecuter.QuerySingleOrDefaultAsync<UserQueryModel>(
                    sql,
                    parameters,
                    cancellationToken: ct);

            return model is null
                ? null
                : MapToDomain(model);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            const string sql = """
            SELECT
                Id,
                Email,
                Username,
                Mobile,
                PasswordHash,
                CreateDate,
                UpdateDate,
                IsDeleted
            FROM Users
            WHERE Id = @Id
              AND IsDeleted = 0;
            """;

            var parameters = new DapperParameterCollection()
                .Add("@Id", id, DbType.Guid);

            var model =
                await _databaseExecuter.QuerySingleOrDefaultAsync<UserQueryModel>(
                    sql,
                    parameters,
                    cancellationToken: ct);

            return model is null
                ? null
                : MapToDomain(model);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            const string sql = """
            SELECT
                Id,
                Email,
                Username,
                Mobile,
                PasswordHash,
                CreateDate,
                UpdateDate,
                IsDeleted
            FROM Users
            WHERE Username = @Username
              AND IsDeleted = 0;
            """;

            var parameters = new DapperParameterCollection()
                .Add("@Username", username, DbType.String);

            var model =
                await _databaseExecuter.QuerySingleOrDefaultAsync<UserQueryModel>(
                    sql,
                    parameters,
                    cancellationToken: ct);

            return model is null
                ? null
                : MapToDomain(model);
        }

        private static User MapToDomain(UserQueryModel model)
        {
            return new User(
                model.Id,
                new Email(model.Email),
                model.Username,
                new PhoneNumber(model.Mobile),
                model.PasswordHash);
        }
    }
}
