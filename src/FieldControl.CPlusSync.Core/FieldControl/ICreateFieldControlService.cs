using FieldControlApi.Resources;

namespace FieldControl.CPlusSync.Core.FieldControl
{
    public interface ICreateFieldControlService
    {
        Service Create(string description);
    }
}
