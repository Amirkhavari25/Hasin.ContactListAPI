using ContactList.Application.Contracts.Persistance;
using ContactList.Domain.Entities;
using ContactList.Infrastracture.Persistance.Database;
using ContactList.Infrastracture.Persistance.Database.Dapper.Models;
using ContactList.Infrastracture.Persistance.QueryModels;
using System.Data;

namespace ContactList.Infrastracture.Persistance.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IDatabaseExecuter _databaseExecuter;

        public ContactRepository(
            IDatabaseExecuter databaseExecuter)
        {
            _databaseExecuter = databaseExecuter;
        }

        public async Task AddAsync(Contact contact, CancellationToken ct)
        {
            const string sql = """
            INSERT INTO Contacts
            (
                Id,
                FirstName,
                LastName,
                PhoneNumber,
                Tag,
                UserId,
                CreateDate,
                UpdateDate,
                IsDeleted
            )
            VALUES
            (
                @Id,
                @FirstName,
                @LastName,
                @PhoneNumber,
                @Tag,
                @UserId,
                @CreateDate,
                @UpdateDate,
                @IsDeleted
            );
            """;

            var parameters = new DapperParameterCollection()
                .Add("@Id", contact.Id, DbType.Guid)
                .Add("@FirstName", contact.FirstName, DbType.String)
                .Add("@LastName", contact.LastName, DbType.String)
                .Add("@PhoneNumber", contact.PhoneNumber.Value, DbType.String)
                .Add("@Tag", contact.Tag, DbType.String)
                .Add("@UserId", contact.UserId, DbType.Guid)
                .Add("@CreateDate", contact.CreateDate, DbType.DateTime2)
                .Add("@UpdateDate", contact.UpdateDate, DbType.DateTime2)
                .Add("@IsDeleted", contact.IsDeleted, DbType.Boolean);

            await _databaseExecuter.ExecuteAsync(sql, parameters, cancellationToken: ct);
        }

        public async Task DeleteAsync(Guid id, Guid userId, CancellationToken ct)
        {
            const string sql = """
            UPDATE Contacts
            SET
                IsDeleted = 1,
                UpdateDate = @UpdateDate
            WHERE Id = @Id
              AND UserId = @UserId
              AND IsDeleted = 0;
            """;

            var parameters = new DapperParameterCollection()
                .Add("@Id", id, DbType.Guid)
                .Add("@UserId", userId, DbType.Guid)
                .Add("@UpdateDate", DateTime.UtcNow, DbType.DateTime2);

            await _databaseExecuter.ExecuteAsync(sql, parameters, cancellationToken: ct);
        }

        public async Task<List<Contact>> GetAllByUserIdAsync(
            Guid userId,
            CancellationToken ct)
        {
            const string sql = """
            SELECT
                Id,
                FirstName,
                LastName,
                PhoneNumber,
                Tag,
                UserId,
                CreateDate,
                UpdateDate,
                IsDeleted
            FROM Contacts
            WHERE UserId = @UserId
              AND IsDeleted = 0
            ORDER BY CreateDate DESC;
            """;

            var parameters = new DapperParameterCollection()
                .Add("@UserId", userId, DbType.Guid);

            var models = await _databaseExecuter.QueryAsync<ContactQueryModel>(sql, parameters, cancellationToken: ct);

            return [.. models.Select(MapToDomain)];
        }

        public async Task<Contact?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct)
        {
            const string sql = """
            SELECT
                Id,
                FirstName,
                LastName,
                PhoneNumber,
                Tag,
                UserId,
                CreateDate,
                UpdateDate,
                IsDeleted
            FROM Contacts
            WHERE Id = @Id
              AND UserId = @UserId
              AND IsDeleted = 0;
            """;

            var parameters = new DapperParameterCollection()
                .Add("@Id", id, DbType.Guid)
                .Add("@UserId", userId, DbType.Guid);

            var model = await _databaseExecuter.QuerySingleOrDefaultAsync<ContactQueryModel>(sql, parameters, cancellationToken: ct);

            return model is null ? null : MapToDomain(model);
        }

        public async Task UpdateAsync(Contact contact, CancellationToken ct)
        {
            const string sql = """
            UPDATE Contacts
            SET
                FirstName = @FirstName,
                LastName = @LastName,
                PhoneNumber = @PhoneNumber,
                Tag = @Tag,
                UpdateDate = @UpdateDate
            WHERE Id = @Id
              AND UserId = @UserId
              AND IsDeleted = 0;
            """;

            var parameters = new DapperParameterCollection()
                .Add("@Id", contact.Id, DbType.Guid)
                .Add("@FirstName", contact.FirstName, DbType.String)
                .Add("@LastName", contact.LastName, DbType.String)
                .Add("@PhoneNumber", contact.PhoneNumber.Value, DbType.String)
                .Add("@Tag", contact.Tag, DbType.String)
                .Add("@UserId", contact.UserId, DbType.Guid)
                .Add("@UpdateDate", contact.UpdateDate, DbType.DateTime2);

            await _databaseExecuter.ExecuteAsync(sql, parameters, cancellationToken: ct);
        }

        private static Contact MapToDomain(ContactQueryModel model)
        {
            return new Contact(
                model.Id,
                model.FirstName,
                model.LastName,
                new PhoneNumber(model.PhoneNumber),
                model.Tag,
                model.UserId);
        }
    }
}
