namespace FieldControl.CPlusSync.Core.Exceptions
{
    public class ServiceNotFoundException : System.ApplicationException
    {
        public ServiceNotFoundException(string serviceName) 
            : base(string.Format("Could not found service with name {0}", serviceName))
        {

        }
    }
}
