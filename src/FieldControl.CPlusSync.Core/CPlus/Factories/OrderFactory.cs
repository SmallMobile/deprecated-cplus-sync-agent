using FieldControl.CPlusSync.Core.CPlus.Models;
using FirebirdSql.Data.FirebirdClient;

namespace FieldControl.CPlusSync.Core.CPlus.Factories
{
    public class OrderSimpleFactory
    {
        public static Order Create(FbDataReader reader)
        {
            return new Order()
            {
                Identifier = reader.GetString(1),
                ScheduledDate = reader.GetDateTime(2),
                ScheduledTime = reader.GetString(3),
                Duration = reader.GetInt32(4),
                Description = reader.GetString(5),
                EmployeeName = reader.GetString(6),
                StatusName = reader.GetString(7),
                ServiceName = reader.GetString(8),

                Customer = new Customer()
                {
                    Name = reader.GetString(9),
                    Street = reader.GetString(10),
                    Number = reader.GetString(11),
                    City = reader.GetString(12),
                    State = reader.GetString(13),
                    ZipCode = reader.GetString(14),
                    Phone = reader.GetString(15),
                    Email = reader.GetString(16),
                }
            };
        }
    }
}
