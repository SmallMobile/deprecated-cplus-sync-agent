namespace FieldControl.CPlusSync.Core.Exceptions
{
    public class ActivityConverterException : System.ApplicationException
    {
        public ActivityConverterException(string statusName) 
            : base(string.Format("Could not convert status from name {0}", statusName))
        {

        }
    }
}
