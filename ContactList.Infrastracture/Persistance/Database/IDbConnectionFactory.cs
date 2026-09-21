using System.Data;

namespace ContactList.Infrastracture.Persistance.Database
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
