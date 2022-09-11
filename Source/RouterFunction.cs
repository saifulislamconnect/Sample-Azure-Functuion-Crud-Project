using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;

namespace TingTango.Source
{
    /// <summary>
    /// Router function calling concrete implementation based on the version expressed in query string or accept header.
    /// Simple implementation, should be replaced once/if HttpTrigger can have header/query string based routing
    /// </summary>
    public static class RouterFunction
    {
        [FunctionName(nameof(RouterList))]
        public static IActionResult RouterList(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contact")]
            HttpRequest req,
            ILogger log,
            IDictionary<string, string> headers,
            IDictionary<string, string> query)
        {
            switch (ResolveVersion(headers, query))
            {
                case "1":
                    return v1.Functions.ContactFunction.ContactList(req, log);
                /*case "2":
                    return v2.Functions.ContactFunction.V1List(req, log);*/
                default:
                    return new BadRequestObjectResult("Invalid version");
            }            
        }

        private static string ResolveVersion(IDictionary<string, string> headers, IDictionary<string, string> query)
        {
            if (headers.TryGetValue("Accept", out var acceptHeader))
            {
                if (acceptHeader.Equals("application/api-v1+json", StringComparison.InvariantCultureIgnoreCase))
                    return "1";

                if (acceptHeader.Equals("application/api-v2+json", StringComparison.InvariantCultureIgnoreCase))
                    return "2";
            }

            if (query.TryGetValue("version", out var queryStringVersion))
            {
                if (float.TryParse(queryStringVersion, out _))
                {
                    return queryStringVersion;
                }                
            }

            return null;
        }
    }
}
