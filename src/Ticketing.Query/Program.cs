using Ticketing.Query.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.Run();