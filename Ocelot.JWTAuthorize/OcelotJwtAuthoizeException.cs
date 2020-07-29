using System;

namespace Ocelot.JwtAuthorize
{
    /// <summary>
    /// OcelotJwtAuthoizeException
    /// </summary>
    public class OcelotJwtAuthoizeException : ApplicationException
    {
        public OcelotJwtAuthoizeException(string message) :
            base(message)
        { }

        public OcelotJwtAuthoizeException() : base() { }
    }
}
