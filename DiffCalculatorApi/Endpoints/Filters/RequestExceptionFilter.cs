using DiffCalculatorApi.Exceptions;

namespace DiffCalculatorApi.Endpoints.Filters;

public class RequestExceptionFilter(ILogger<RequestExceptionFilter> _logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Error -> {Message}", ex.Message);
            return Results.NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error -> {Message}", ex.Message);
            return Results.BadRequest();
        }
    }
}
