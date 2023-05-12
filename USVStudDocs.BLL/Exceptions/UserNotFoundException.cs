using System;

namespace USVStudDocs.BLL.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base (message)
        {
        }
    }
}