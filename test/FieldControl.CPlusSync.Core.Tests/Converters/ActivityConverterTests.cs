using AssertProperties;
using FieldControl.CPlusSync.Core.Converters;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FieldControlApi.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FieldControl.CPlusSync.Core.Tests.Converters
{
    /*
        [✓] C-Plus Order should be converted to Field Control Activity 
    */

    [TestFixture]
    public class ActivityConverterTests
    {
        [Test]
        public void ConvertFrom_GivenAnOrder_ShouldBeConvertedAsExpected()
        {
            var services = new List<Service>() {
                new Service() { Id = 1, Description = "Limpeza" },
                new Service() { Id = 2, Description = "Instalacao" },
            };

            var employees = new List<Employee>() {
                new Employee() { Id = 3, Name = "Luiz Freneda" },
                new Employee() { Id = 5, Name = "Eduardo Santos" },
            };

            var activity = new ActivityConverter(services, employees, null).ConvertFrom(new Order()
            {
                Identifier = "OS 123",
                Description = "Order description",
                Duration = 1,
                ScheduledDate = new DateTime(2015, 5, 15),
                ScheduledTime = "09:00:00",
                StatusName = "Agendado",

                ServiceName = "Limpeza",
                EmployeeName = "Luiz Freneda",
            });

            activity
                .AssertProperties()
                    .EnsureThat(c => c.Identifier).ShouldBe("OS 123")
                    .And(c => c.Description).ShouldBe("Order description")
                    .And(c => c.Duration).ShouldBe(60)
                    .And(c => c.ScheduledTo).ShouldBe(new DateTime(2015, 5, 15))
                    .And(c => c.TimeFixed).ShouldBe(true)
                    .And(c => c.FixedStartTime).ShouldBe("09:00")
                    .And(c => c.Status).ShouldBe(FieldControlApi.Resources.ActivityStatus.Scheduled)
                    .And(c => c.ServiceId).ShouldBe(1)
                    .And(c => c.EmployeeId).ShouldBe(3)
                .Assert();
        }
    }
}
