using FieldControl.CPlusSync.Core.Commands;
using FieldControl.CPlusSync.Core.Converters;
using FieldControl.CPlusSync.Core.CPlus.Data;
using FieldControl.CPlusSync.Core.CPlus.Queries;
using FieldControl.CPlusSync.Core.FieldControl;
using FieldControl.CPlusSync.Core.Google;
using FieldControlApi;
using FieldControlApi.Requests.Activities;
using FieldControlApi.Requests.Customers;
using Geocoding;
using Geocoding.Google;
using System;
using System.Collections.Generic;

namespace FieldControl.CPlusSync.Core
{
    public class SyncController
    {
        public void SyncDate(DateTime date) {

            var fieldControlConfiguration = new FieldControlApi.Configuration.AppSettingsConfiguration();
            var client = new Client(fieldControlConfiguration);
            client.Authenticate();

            var cPlusConfiguration = new CPlus.Configurations.AppSettingsConfiguration();
            var cPlusOrdersQuery = new OrdersQuery(cPlusConfiguration);
            var orderDao = new OrderDao(cPlusConfiguration);
            
            var orders = cPlusOrdersQuery.Execute(date);
            var activities = client.Execute(new GetActivitiesRequest(date));

            var geoCoderConfiguration = new AppSettingsGeoCoderConfiguration();
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = geoCoderConfiguration.GoogleKey };

            var services = client.Execute(new GetServicesRequest());
            var employees = client.Execute(new GetActiveEmployeesRequest(date));
            var activityConveter = new ActivityConverter(
                services: services,
                employees: employees,
                createFieldControlService: new CreateFieldControlService(client),
                customerFieldControlService: new CustomerFieldControlService(client, geocoder)
            );

            var commands = new List<ICommand>() {
                new CreateFieldControlActivityCommand(orders, activities, activityConveter, client),
                new UpdateCPlusOrderStatusCommand(orders, activities, orderDao)
            };

            commands.ForEach(command => {
                command.Run();
            });
        }
    }
}
