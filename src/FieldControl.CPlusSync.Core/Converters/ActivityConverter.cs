﻿using System.Collections.Generic;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FieldControl.CPlusSync.Core.Exceptions;
using FieldControlApi.Resources;
using FieldControl.CPlusSync.Core.FieldControl;
using System.Linq;
using FC = FieldControlApi.Resources;
using System;

namespace FieldControl.CPlusSync.Core.Converters
{
    public class ActivityStatusConverter
    {
        public virtual string GetStringStatus(ActivityStatus status)
        {
            if (status == ActivityStatus.Scheduled) return "agendada";
            if (status == ActivityStatus.InProgress) return "em andamento";
            if (status == ActivityStatus.Done) return "concluída";
            if (status == ActivityStatus.Reported) return "reportada como problema";
            if (status == ActivityStatus.Canceled) return "cancelada";

            throw new ActivityConverterException(status.ToString());
        }

        public virtual ActivityStatus GetStatusByName(string statusName)
        {
            statusName = statusName.ToLowerInvariant().Trim();

            if (string.IsNullOrEmpty(statusName))
            {
                return ActivityStatus.Scheduled;
            }

            if (statusName == "agendada") return ActivityStatus.Scheduled;
            if (statusName == "em andamento") return ActivityStatus.InProgress;
            if (statusName == "concluída") return ActivityStatus.Done;
            if (statusName == "reportada como problema") return ActivityStatus.Reported;
            if (statusName == "cancelada") return ActivityStatus.Canceled;

            throw new ActivityConverterException(statusName);
        }
    }

    public class ActivityConverter
    {
        private readonly List<Service> _services = null;
        private readonly List<Employee> _employees = null;
        private readonly ICustomerFieldControlService _customerFieldControlService = null;

        public ActivityConverter(List<Service> services,
                                 List<Employee> employees,
                                 ICustomerFieldControlService customerFieldControlService)
        {
            _services = services;
            _employees = employees;
            _customerFieldControlService = customerFieldControlService;
        }

        public virtual int GetEmployeeIdByName(string employeeName)
        {
            var employee = _employees.FirstOrDefault(e => e.Name.ToLowerInvariant().Trim() == employeeName.ToLowerInvariant().Trim());
            if (employee == null)
            {
                throw new EmployeeNotFoundException(employeeName);
            }

            return employee.Id;
        }

        public virtual int GetServiceIdByName(string serviceName)
        {
            var service = _services.FirstOrDefault(e => e.Description.ToLowerInvariant().Trim() == serviceName.ToLowerInvariant().Trim());
            if (service == null)
            {
                throw new ServiceNotFoundException(serviceName);
            }

            return service.Id;
        }

        private FC.Customer GetCustomer(CPlus.Models.Customer customer)
        {
            return _customerFieldControlService.GetOrCreate(customer);
        }

        public virtual Activity ConvertFrom(Order order)
        {
            var customer = GetCustomer(order.Customer);

            var activity = new Activity(customer)
            {
                Identifier = order.Identifier,
                Description = order.Description,
                ScheduledTo = order.ScheduledDate,
                FixedStartTime = order.ScheduledTime.Substring(0, 5),
                Duration = (order.Duration * 60),
                Status = new ActivityStatusConverter().GetStatusByName(order.StatusName),
                EmployeeId = GetEmployeeIdByName(order.EmployeeName),
                ServiceId = GetServiceIdByName(order.ServiceName),
                CustomerId = customer.Id
            };

            if (activity.Duration <= 0)
            {
                activity.Duration = 1;
            }

            return activity;
        }


    }
}
