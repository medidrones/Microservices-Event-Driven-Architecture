using Ticketing.Query.Application.Extensions;
using Ticketing.Query.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();

await app.ApplyMigrationsAsync();

app.Run();