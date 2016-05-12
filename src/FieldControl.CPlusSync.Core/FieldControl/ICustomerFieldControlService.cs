using FieldControlApi.Resources;

namespace FieldControl.CPlusSync.Core.FieldControl
{
    public interface ICustomerFieldControlService
    {
        Customer GetOrCreate(CPlus.Models.Customer customer);
    }
}
