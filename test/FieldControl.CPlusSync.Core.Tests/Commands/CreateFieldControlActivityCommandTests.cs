using FieldControl.CPlusSync.Core.Commands;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FieldControlApi.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldControl.CPlusSync.Core.Tests.Commands
{
    [TestFixture]
    public class CreateFieldControlActivityCommandTests
    {
        [Test]
        public void FilterNotCreatedOrders_ShouldReturnsOrdersWhichAreNotInActivities()
        {
            var command = new CreateFieldControlActivityCommand(
                orders: new List<Order>() {
                    new Order() { Identifier = "Um" },
                    new Order() { Identifier = "dois" },
                    new Order() { Identifier = "TRES" },
                },
                activities: new List<Activity>() {
                    new Activity() { Identifier = "um" },
                    new Activity() { Identifier = "Dois" },
                },
                conveter: null,
                fieldControlClient: null
            );

            var orders = command.FilterNotCreatedOrders();

            Assert.AreEqual(1, orders.Count());
            Assert.AreEqual("TRES", orders.ElementAt(0).Identifier);
        }
    }
}
