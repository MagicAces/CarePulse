using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace backend.Filters
{
    public class SecretKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionDescriptor = context.ApiDescription.ActionDescriptor;
            
            if (actionDescriptor.RouteValues["controller"] == "Account" && actionDescriptor.RouteValues["action"] == "Register")
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Secret-Key",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    Description = "Optional secret key for non-patient users"
                });
            }
        }
    }
}