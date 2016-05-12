using FieldControl.CPlusSync.Core.Converters;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FieldControlApi;
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
            foreach (var order in FilterNotCreatedOrders())
            {
                var activity = _conveter.ConvertFrom(order);
                var request = new CreateActivityRequest(activity);

                _fieldControlClient.Execute(request);
            }
        }
    }
}

