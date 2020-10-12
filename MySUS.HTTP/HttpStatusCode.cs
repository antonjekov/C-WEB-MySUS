using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.HTTP
{
    public enum HttpStatusCode
    {
        Ok = 200,
        MovedPermanently = 301,
        Found = 302,
        TemporaryRedirect = 307,
        NotFound = 404,
        ServerError = 500,
        UnprocessableEntity = 422
    }
}
