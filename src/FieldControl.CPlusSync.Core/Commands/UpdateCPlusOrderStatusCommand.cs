using FieldControl.CPlusSync.Core.Converters;
using FieldControl.CPlusSync.Core.CPlus.Data;
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
    public class UpdateCPlusOrderStatusCommand : ICommand
    {
        private readonly IEnumerable<Order> _orders = null;
        private readonly IEnumerable<Activity> _activities = null;
        private readonly ActivityStatusConverter _statusConverter = null;

        public UpdateCPlusOrderStatusCommand(IEnumerable<Order> orders,
                                             IEnumerable<Activity> activities,
                                             OrderDao orderDao)
        {
            _orders = orders;
            _activities = activities;
            _statusConverter = new ActivityStatusConverter();
        }

        private Order GetOrder(Activity activity) {
            return _orders.FirstOrDefault(o => o.Identifier.ToLowerInvariant().Trim() == activity.Identifier.ToLowerInvariant().Trim());
        }

        public void Run()
        {
            foreach (var activity in _activities) {
                var order = GetOrder(activity);
                if (order == null) {
                    continue;
                }

                //_statusConverter.
                    
            }
        }
    }
}

