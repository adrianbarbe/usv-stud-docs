using System;

namespace USVStudDocs.BLL.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message) : base (message)
        {
        }
    }
}