using FieldControl.CPlusSync.Core.Converters;
using FieldControl.CPlusSync.Core.CPlus.Data;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FieldControl.CPlusSync.Core.Logging;
using FieldControlApi.Exceptions;
using FieldControlApi.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FieldControl.CPlusSync.Core.Commands
{
    public class UpdateCPlusOrderStatusCommand : ICommand
    {
        private readonly IEnumerable<Order> _orders = null;
        private readonly IEnumerable<Activity> _activities = null;
        private readonly ActivityStatusConverter _statusConverter = null;
        private readonly OrderDao _orderDao = null;

        public UpdateCPlusOrderStatusCommand(IEnumerable<Order> orders,
                                             IEnumerable<Activity> activities,
                                             OrderDao orderDao)
        {
            _orders = orders;
            _activities = activities;
            _orderDao = orderDao;
            _statusConverter = new ActivityStatusConverter();
        }

        private Order GetOrder(Activity activity)
        {
            return _orders.FirstOrDefault(o => o.Identifier.ToLowerInvariant().Trim() == activity.Identifier.ToLowerInvariant().Trim());
        }

        private bool StillSameStatus(Activity activity, Order order)
        {
            var fieldControlStatus = _statusConverter.GetStringStatus(activity.Status);
            return fieldControlStatus.ToLowerInvariant().Trim() == order.StatusName.ToLowerInvariant().Trim();
        }

        public void Run()
        {
            FileLog.WriteLineIf("Atualizando atividades no C-Plus");
            FileLog.WriteJson(_activities);

            foreach (var activity in _activities)
            {
                try
                {
                    var order = GetOrder(activity);

                    FileLog.WriteJson(activity);
                    FileLog.WriteJson(order);

                    if (order == null || StillSameStatus(activity, order))
                    {
                        continue;
                    }

                    order.StatusName = _statusConverter.GetStringStatus(activity.Status);
                    _orderDao.ChangeStatus(order);

                }
                catch (Exception ex)
                {
                    FileLog.WriteError("Generic Error " + ex.Message);
                    FileLog.WriteError(ex.StackTrace);
                }
            }
        }
    }
}

