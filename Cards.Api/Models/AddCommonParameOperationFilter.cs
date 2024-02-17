using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cards.Api.Models
{
    public class AddCommonParameOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && !descriptor.ControllerName.ToLower().Contains("auth"))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "ZUMO-API-VERSION",
                    In = ParameterLocation.Query,
                    Description = "ZUMO-API-VERSION",
                    Required = true
                });
            }

        }
    }
}
