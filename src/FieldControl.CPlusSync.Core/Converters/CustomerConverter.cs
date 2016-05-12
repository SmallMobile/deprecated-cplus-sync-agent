using FieldControlApi.Resources;

namespace FieldControl.CPlusSync.Core.Converters
{
    public class CustomerConverter
    {
        public Customer ConvertFrom(CPlus.Models.Customer customer)
        {
            return new Customer() {
                Name = customer.Name,
                City = customer.City,
                Email = customer.Email,
                Number = customer.Number,
                Phone = customer.Phone,
                State = customer.State,
                Street = customer.Street,
                ZipCode = customer.ZipCode,
            };
        }
    }
}
