using FieldControl.CPlusSync.Core.Commands;
using FieldControl.CPlusSync.Core.Converters;
using FieldControl.CPlusSync.Core.CPlus.Data;
using FieldControl.CPlusSync.Core.CPlus.Queries;
using FieldControl.CPlusSync.Core.FieldControl;
using FieldControl.CPlusSync.Core.Google;
using FieldControl.CPlusSync.Core.Logging;
using FieldControlApi;
using FieldControlApi.Requests.Activities;
using FieldControlApi.Requests.Customers;
using Geocoding;
using Geocoding.Google;
using System;
using System.Collections.Generic;
using FieldControlApi.Configuration;
using FieldControl.CPlusSync.Core.Caching;

namespace FieldControl.CPlusSync.Core
{
    public class SyncController
    {
 
        public void SyncDate(DateTime from, DateTime to) {

            var fieldControlConfiguration = new FieldControlApi.Configuration.AppSettingsConfiguration();
            var client = new Client(fieldControlConfiguration);

            var authenticationCache = new SimpleFileCache("authentication");
            var accessToken = authenticationCache.GetOrPut(() => {
                client.Authenticate();
                return client.AuthenticationToken;
            });

            client.AuthenticationToken = accessToken;

            FileLog.WriteJson(fieldControlConfiguration);
            FileLog.WriteJson(client);

            var cPlusConfiguration = new CPlus.Configurations.AppSettingsConfiguration();
            var cPlusOrdersQuery = new OrdersQuery(cPlusConfiguration);
            var orderDao = new OrderDao(cPlusConfiguration);

            FileLog.WriteJson(cPlusConfiguration);

            var orders = cPlusOrdersQuery.Execute(from, to);
            var activities = client.Execute(new GetActivitiesRequest(from, to));

            FileLog.WriteJson(orders);
            FileLog.WriteJson(activities);

            var geoCoderConfiguration = new AppSettingsGeoCoderConfiguration();
            IGeocoder geocoder = new GoogleGeocoder() {
                ApiKey = geoCoderConfiguration.GoogleKey
            };

            var services = client.Execute(new GetServicesRequest());
            var employees = client.Execute(new GetActiveEmployeesRequest(from));

            var activityConveter = new ActivityConverter(
                services: services,
                employees: employees,
                customerFieldControlService: new CustomerFieldControlService(client, geocoder)
            );

            FileLog.WriteJson(services);
            FileLog.WriteJson(employees);

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
