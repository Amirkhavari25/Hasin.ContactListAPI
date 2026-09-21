using System.Data;

namespace ContactList.Infrastracture.Persistance.Database.Dapper.Models
{
    public sealed class DapperParameter
    {
        public string Name { get; }
        public object? Value { get; }
        public DbType? DbType { get; }
        public ParameterDirection Direction { get; }
        public int? Size { get; }
        public byte? Precision { get; }
        public byte? Scale { get; }

        public DapperParameter(
            string name,
            object? value,
            DbType? dbType = null,
            ParameterDirection direction = ParameterDirection.Input,
            int? size = null,
            byte? precision = null,
            byte? scale = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name;
            Value = value;
            DbType = dbType;
            Direction = direction;
            Size = size;
            Precision = precision;
            Scale = scale;
        }
    }
}
