using FieldControlApi;
using FieldControlApi.Requests.Customers;
using FieldControlApi.Resources;

namespace FieldControl.CPlusSync.Core.FieldControl
{
    public class CreateFieldControlService : ICreateFieldControlService
    {
        private readonly Client _client = null;

        public CreateFieldControlService(Client client)
        {
            _client = client;
        }

        public Service Create(string description) {

            var service = new Service()
            {
                Description = description,
                Duration = 10
            };

            return _client.Execute(new CreateServiceRequest(service));
        }
    }
}
