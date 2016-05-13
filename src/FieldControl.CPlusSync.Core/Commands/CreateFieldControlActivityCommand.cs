using FieldControl.CPlusSync.Core.Converters;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FieldControl.CPlusSync.Core.Logging;
using FieldControlApi;
using FieldControlApi.Exceptions;
using FieldControlApi.Requests;
using FieldControlApi.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldControl.CPlusSync.Core.Commands
{
    public class CreateFieldControlActivityCommand : ICommand
    {
        private readonly IEnumerable<Order> _orders = null;
        private readonly IEnumerable<Activity> _activites = null;
        private readonly ActivityConverter _conveter = null;
        private readonly Client _fieldControlClient = null;

        public CreateFieldControlActivityCommand(IEnumerable<Order> orders,
                                                 IEnumerable<Activity> activities,
                                                 ActivityConverter conveter,
                                                 Client fieldControlClient)
        {
            _orders = orders;
            _conveter = conveter;
            _activites = activities;
            _fieldControlClient = fieldControlClient;
        }

        public List<Order> FilterNotCreatedOrders()
        {
            var ordersIds = _orders.Select(c => c.Identifier.ToLowerInvariant().Trim()).ToList();
            var activitiesIds = _activites.Select(c => c.Identifier.ToLowerInvariant().Trim()).ToList();
            var notCreatedIds = ordersIds.Except(activitiesIds).ToList();
            var notCreatedOrders = _orders.Where(c => notCreatedIds.Contains(c.Identifier.ToLowerInvariant().Trim())).ToList();
            return notCreatedOrders;
        }

        public void Run()
        {
            var orders = FilterNotCreatedOrders();

            FileLog.WriteLine("Enviando ordens para o Field Control");
            FileLog.WriteJson(orders);

            foreach (var order in orders)
            {
                try
                {
                    var activity = _conveter.ConvertFrom(order);

                    FileLog.WriteJson(order);
                    FileLog.WriteJson(activity);

                    var request = new CreateActivityRequest(activity);
                    _fieldControlClient.Execute(request);
                }
                catch (RequestErrorException fex)
                {
                    FileLog.WriteLine("Field Control Api Error " + fex.Message);
                    FileLog.WriteLine(fex.ResponseBody);
                }
                catch (ApplicationException aex)
                {
                    FileLog.WriteLine("Sync App Error " + aex.Message);
                    FileLog.WriteLine(aex.StackTrace);
                }
                catch (Exception ex)
                {
                    FileLog.WriteLine("Generic Error " + ex.Message);
                    FileLog.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}

