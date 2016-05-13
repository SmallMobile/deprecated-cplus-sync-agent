using FieldControl.CPlusSync.Core.CPlus.Models;
using FirebirdSql.Data.FirebirdClient;

namespace FieldControl.CPlusSync.Core.CPlus.Factories
{
    public class OrderSimpleFactory
    {
        public static string DefaultIfNull(FbDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetString(colIndex);
            }
            else {
                return string.Empty;
            }
        }

        public static Order Create(FbDataReader reader)
        {
            return new Order()
            {
                Identifier = DefaultIfNull(reader, 1),
                ScheduledDate = reader.GetDateTime(2),
                ScheduledTime = reader.GetString(3),
                Duration = reader.GetInt32(4),
                Description = DefaultIfNull(reader, 5),
                EmployeeName = DefaultIfNull(reader, 6),
                StatusName = DefaultIfNull(reader, 7),
                ServiceName = DefaultIfNull(reader, 8),

                Customer = new Customer()
                {
                    Name = DefaultIfNull(reader, 9),
                    Street = DefaultIfNull(reader, 10),
                    Number = DefaultIfNull(reader, 11),
                    City = DefaultIfNull(reader, 12),
                    State = DefaultIfNull(reader, 13),
                    ZipCode = DefaultIfNull(reader, 14),
                    Phone = DefaultIfNull(reader, 15),
                    Email = DefaultIfNull(reader, 16),
                }
            };
        }
    }
}
