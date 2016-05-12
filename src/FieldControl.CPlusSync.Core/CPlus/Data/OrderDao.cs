using FieldControl.CPlusSync.Core.CPlus.Configurations;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldControl.CPlusSync.Core.CPlus.Data
{
    public class OrderDao
    {
        private readonly IConfiguration _configuration = null;

        public OrderDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ChangeStatus(Order order)
        {
            var sqlCommand = @"UPDATE os_ordemservico SET CODSTATUS = (SELECT CODSTATUS FROM os_status WHERE status = '" + order.StatusName + "') WHERE (numos = 1)";

            using (FbConnection connection = new FbConnection(_configuration.ConnnectionString))
            {
                connection.Open();

                using (FbCommand command = new FbCommand(sqlCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
