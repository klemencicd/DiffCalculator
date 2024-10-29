using Carter;
using DiffCalculatorApi.Endpoints.Filters;
using DiffCalculatorApi.Repositories.Interfaces;
using DiffCalculatorApi.Services.Interfaces;
using DiffCalculatorApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DiffCalculatorApi.Endpoints;

public class DiffEndpoints() : CarterModule("/v1/diff")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}/right", AddRight)
            .AddEndpointFilter(new ValidationFilter())
            .WithOpenApi();

        app.MapPut("/{id}/left", AddLeft)
            .AddEndpointFilter(new ValidationFilter())
            .WithOpenApi();

        app.MapGet("/{id}", GetDiff)
            .AddEndpointFilter<RequestExceptionFilter>()
            .WithOpenApi();
    }

    public static IResult AddRight(int id, [FromBody]DiffRequestData request, IDiffRepository repository)
    {
        repository.AddRight(id, request.Data!);
        return Results.Ok();
    }

    public static IResult AddLeft(int id, [FromBody] DiffRequestData request, IDiffRepository repository)
    {
        repository.AddLeft(id, request.Data!);
        return Results.Ok();
    }

    public static IResult GetDiff(int id, IDiffCalculator diffCalculator)
    {
        DiffResult diffResult = diffCalculator.Calculate(id);
        return TypedResults.Ok(diffResult);
    }
}
