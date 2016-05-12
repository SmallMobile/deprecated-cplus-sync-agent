namespace FieldControl.CPlusSync.Core.Exceptions
{
    public class LatitudeLongitudeCouldNotBeDecodedException : System.ApplicationException
    {
        public LatitudeLongitudeCouldNotBeDecodedException(string address) 
            : base(string.Format("Could not find lat/lng to address: {0}", address))
        {

        }
    }
}
