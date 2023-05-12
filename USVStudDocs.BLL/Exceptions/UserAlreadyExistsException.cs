﻿using System;

namespace USVStudDocs.BLL.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message) : base (message)
        {
        }
    }
}