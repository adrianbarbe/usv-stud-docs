using System;
using System.Collections.Generic;
using USVStudDocs.Models.Shared;

namespace USVStudDocs.BLL.Exceptions
{
    public class ValidationFormException : Exception
    {
        public ValidationFormException(List<FormValidationInfo> error)
        {
            Errors = error;
        }

        public List<FormValidationInfo> Errors { get; }
    }
}