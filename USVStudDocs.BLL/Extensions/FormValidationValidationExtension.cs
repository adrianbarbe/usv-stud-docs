using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using USVStudDocs.Models.Shared;

namespace USVStudDocs.BLL.Extensions;

public static class FormValidationExceptionExtensions
{
    public static List<FormValidationInfo> CreateErrorsList(this IList<ValidationFailure> errors)
    {
        return errors.Select(err => new FormValidationInfo
            {
                Field = err.PropertyName,
                Message = err.ErrorMessage,
            })
            .ToList();
    }
}