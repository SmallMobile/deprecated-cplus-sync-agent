using System;
using System.Collections.Generic;
using System.Linq;
using FieldControl.CPlusSync.Core.CPlus.Models;
using FC = FieldControlApi.Resources;
using FieldControlApi;
using FieldControlApi.Requests.Customers;
using Geocoding.Google;
using FieldControl.CPlusSync.Core.Google;
using Geocoding;
using FieldControl.CPlusSync.Core.Exceptions;

namespace FieldControl.CPlusSync.Core.FieldControl
{
    public class CustomerFieldControlService : ICustomerFieldControlService
    {
        private readonly Client _client = null;
        private readonly IGeocoder _geoCoder = null;

        public CustomerFieldControlService(Client client, IGeocoder geoCoder)
        {
            _client = client;
            _geoCoder = geoCoder;
        }

        public FC.Customer GetOrCreate(CPlus.Models.Customer cPlusCustomer)
        {
            var customers = _client.Execute(new GetCustomersRequest(nameLike: cPlusCustomer.Name));
            var customer = customers.FirstOrDefault();
       
            if (customer == null)
            {
                customer = new FC.Customer()
                {
                    Name = cPlusCustomer.Name,
                    Email = cPlusCustomer.Email,
                    Phone = cPlusCustomer.Phone,
                    ZipCode = cPlusCustomer.ZipCode,
                    Street = cPlusCustomer.Street,
                    Number = cPlusCustomer.Number,
                    City = cPlusCustomer.City,
                    State = cPlusCustomer.State
                };

                var searchAddress = string.Format("{0}, {1} - {2}, {3} - {4}",
                    customer.Street,
                    customer.Number,
                    customer.City,
                    customer.State,
                    customer.ZipCode
                );
                IEnumerable<Address> addresses = _geoCoder.Geocode(searchAddress);

                if (!addresses.Any()) {
                    throw new LatitudeLongitudeCouldNotBeDecodedException(searchAddress);
                }

                customer.Latitude = Convert.ToDecimal(addresses.First().Coordinates.Latitude);
                customer.Longitude = Convert.ToDecimal(addresses.First().Coordinates.Longitude);

                customer = _client.Execute(new CreateCustomerRequest(customer));
            }

            return customer;
        }
    }
}
