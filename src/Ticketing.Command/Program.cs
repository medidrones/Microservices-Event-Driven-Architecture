using Ticketing.Command.Application;
using Ticketing.Command.Features.Apis;
using Ticketing.Command.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.RegisterMinimalApis();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapMinimalApiEndpoints();

app.Run();
