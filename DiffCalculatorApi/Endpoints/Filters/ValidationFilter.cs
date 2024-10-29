using System.Buffers.Text;

namespace DiffCalculatorApi.Endpoints.Filters;

public class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dataParam = context.GetArgument<DiffRequestData>(1);
        if (string.IsNullOrWhiteSpace(dataParam.Data)) return Results.BadRequest("Input cannot be null or empty");
        if (!Base64.IsValid(dataParam.Data)) return Results.BadRequest("The input is not a valid Base - 64 string");

        return await next(context);
    }
}
