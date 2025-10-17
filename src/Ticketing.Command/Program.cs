using MediatR;
using Ticketing.Command.Application;
using Ticketing.Command.Infrastructure;
using static Ticketing.Command.Features.Tickets.TicketCreate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/ticket", async (IMediator mediator, TicketCreateRequest request, CancellationToken cancellationToken) =>
{
    var command = new TicketCreateCommand(request);
    var result = await mediator.Send(command, cancellationToken);

    return Results.Ok(result);
}).WithName("CreateTicket");

app.Run();
