using FleetSaaS.API.Extensions;
var builder = WebApplication.CreateBuilder(args);

//logging
builder.ConfigureSerilog();

//configure services
builder.ConfigureServices();

var app = builder.Build();

//all middlewares
app.ConfigureMiddleware();

app.Run();
