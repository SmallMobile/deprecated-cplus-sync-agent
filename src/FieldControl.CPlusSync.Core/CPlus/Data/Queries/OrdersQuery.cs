using FieldControl.CPlusSync.Core.CPlus.Configurations;
using FieldControl.CPlusSync.Core.CPlus.Factories;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;

namespace FieldControl.CPlusSync.Core.CPlus.Queries
{
    public class OrdersQuery
    {
        private readonly IConfiguration _configuration = null;

        public OrdersQuery(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string sqlQuery = @"select
                                 os_ordemservico.flagexterno        as externa
                                ,os_ordemservico.numos              as numero
                                ,os_ordemservico.dataagenda         as dataagendada
                                ,os_ordemservico.horaagenda         as horaagendada
                                ,os_ordemservico.prazoatend         as duracao
                                ,os_ordemservico.ocorrencia         as descricao
                                ,os_tecnico.tecnico                 as tecnico
                                ,os_status.status
                                ,produto.nomeprod                   as tipoatividade
                                ,cliente.NOMECLI                    as cliente
                                ,cliente.ENDERECO
                                ,cliente.NUMEROLOGRADOURO
                                ,cliente.CIDADE
                                ,cliente.ESTADO
                                ,cliente.CEP
                                ,cliente.TELEFONE
                                ,cliente.EMAIL
                            from
                                os_ordemservico

                                inner join os_prodserv 
                                inner join produto on produto.codprod = os_prodserv.codprod 
                                                   on os_prodserv.codos = os_ordemservico.codos
                                inner join os_tecnico on os_tecnico.codtec = os_ordemservico.codtec
                                inner join os_status on os_status.codstatus = os_ordemservico.codstatus
                                inner join cliente on cliente.codcli = os_ordemservico.codcli";


        public List<Order> Execute(DateTime scheduledDate)
        {
            var orders = new List<Order>();

            using (FbConnection connection = new FbConnection(_configuration.ConnnectionString))
            {
                connection.Open();

                using (FbCommand command = new FbCommand(sqlQuery, connection))
                {
                    using (FbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(OrderSimpleFactory.Create(reader));
                        }
                    }
                }
            }

            return orders;
        }
    }
}
