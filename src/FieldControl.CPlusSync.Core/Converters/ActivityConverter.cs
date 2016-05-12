using System.Collections.Generic;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FieldControl.CPlusSync.Core.Exceptions;
using FieldControlApi.Resources;
using FieldControl.CPlusSync.Core.FieldControl;
using System.Linq;

namespace FieldControl.CPlusSync.Core.Converters
{
    public class ActivityStatusConverter
    {
        public virtual string GetStringStatus(ActivityStatus status)
        {
            if (status == ActivityStatus.Scheduled) return "agendado";
            if (status == ActivityStatus.InProgress) return "em andamento";
            if (status == ActivityStatus.Done) return "concluído";
            if (status == ActivityStatus.Reported) return "reportado como problema";
            if (status == ActivityStatus.Canceled) return "cancelada";

            throw new ActivityConverterException(status.ToString());
        }

        public virtual ActivityStatus GetStatusByName(string statusName)
        {
            statusName = statusName.ToLowerInvariant().Trim();

            if (statusName == "agendado") return ActivityStatus.Scheduled;
            if (statusName == "em andamento") return ActivityStatus.InProgress;
            if (statusName == "concluído") return ActivityStatus.Done;
            if (statusName == "reportado como problema") return ActivityStatus.Reported;
            if (statusName == "cancelada") return ActivityStatus.Canceled;

            throw new ActivityConverterException(statusName);
        }
    }

    public class ActivityConverter
    {
        private readonly List<Service> _services = null;
        private readonly List<Employee> _employees = null;
        private readonly ICreateFieldControlService _createFieldControlService = null;

        public ActivityConverter(List<Service> services, 
                                 List<Employee> employees,
                                 ICreateFieldControlService createFieldControlService)
        {
            _services = services;
            _employees = employees;
            _createFieldControlService = createFieldControlService;
        }

        public virtual int GetEmployeeIdByName(string employeeName)
        {
            var employee = _employees.FirstOrDefault(e => e.Name.ToLowerInvariant().Trim() == employeeName.ToLowerInvariant().Trim());
            if (employee == null) {
                throw new EmployeeNotFoundException(employeeName);
            }

            return employee.Id;
        }

        public virtual int GetServiceIdByName(string serviceName)
        {
            var service = _services.FirstOrDefault(e => e.Description.ToLowerInvariant().Trim() == serviceName.ToLowerInvariant().Trim());
            if (service == null) {
                service = _createFieldControlService.Create(serviceName);
            }

            return service.Id;
        }

        public virtual Activity ConvertFrom(Order order) {

            return new Activity() {
                Identifier = order.Identifier,
                Description = order.Description,
                ScheduledTo = order.ScheduledDate,
                FixedStartTime = order.ScheduledTime.Substring(0, 5),
                Duration = (order.Duration * 60),
                Status = new ActivityStatusConverter().GetStatusByName(order.StatusName),
                EmployeeId = GetEmployeeIdByName(order.EmployeeName),
                ServiceId = GetServiceIdByName(order.ServiceName)
            };
        }
    }
}
