namespace ContactList.Infrastracture.Persistance.Database.Dapper.Models
{
    public sealed class DapperParameterCollection
    {
        private readonly List<DapperParameter> _parameters = [];

        public IReadOnlyCollection<DapperParameter> Parameters =>
            _parameters;

        public DapperParameterCollection Add(
            DapperParameter parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter);

            _parameters.Add(parameter);

            return this;
        }

        public DapperParameterCollection Add(
            string name,
            object? value,
            System.Data.DbType? dbType = null,
            System.Data.ParameterDirection direction =
                System.Data.ParameterDirection.Input,
            int? size = null,
            byte? precision = null,
            byte? scale = null)
        {
            _parameters.Add(
                new DapperParameter(
                    name,
                    value,
                    dbType,
                    direction,
                    size,
                    precision,
                    scale));

            return this;
        }
    }
}
