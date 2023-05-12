using System;

namespace USVStudDocs.BLL.Exceptions
{
    public class UnauthenticatedException : Exception
    {
        public UnauthenticatedException(string message) : base (message)
        {
        }
    }
}