namespace FieldControl.CPlusSync.Core.Exceptions
{
    public class EmployeeNotFoundException : System.ApplicationException
    {
        public EmployeeNotFoundException(string employeeName) 
            : base(string.Format("Could not found employee with name {0}", employeeName))
        {

        }
    }
}
