﻿using Microsoft.AspNetCore.Http;

namespace Marlin.Core
{
    public class ApiOutput
    {
        public ApiOutput(string data = null, int statusCode = StatusCodes.Status200OK, string contentType = "application/json")
        {
            Response = data ?? string.Empty;
            StatusCode = statusCode;
            ContentType = contentType;
        }

        public string Response { get; }
        public int StatusCode { get; }
        public string ContentType { get; }
    }
}
