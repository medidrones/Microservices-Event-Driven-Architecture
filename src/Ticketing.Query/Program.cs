using Ticketing.Query.Application;
using Ticketing.Query.Application.Extensions;
using Ticketing.Query.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterApplicationServices();

var app = builder.Build();

app.MapControllers();

await app.ApplyMigrationsAsync();

app.Run();