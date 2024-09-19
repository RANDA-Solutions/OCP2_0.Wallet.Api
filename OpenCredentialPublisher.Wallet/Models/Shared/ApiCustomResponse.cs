using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public class ApiCustomResponse: ApiResponse
    {
        public object Result { get; }
        public IEnumerable<string> Errors { get; }

        public ApiCustomResponse(int statusCode, object result, string message = null, string redirectUrl = null)
            : base(statusCode, message, redirectUrl)
        {
            Result = result;

        }

        public ApiCustomResponse(int statusCode, ModelStateDictionary modelState)
            : base(statusCode)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();

        }

    }
}