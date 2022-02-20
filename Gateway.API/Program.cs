using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var fileOcelot = builder.Configuration.GetValue<string>("FileOcelotConfig:File");

builder.WebHost.ConfigureAppConfiguration(
    cfg => cfg.AddJsonFile(Path.Combine("configuration", fileOcelot), 
                            optional: false, reloadOnChange: true)
);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseOcelot().Wait();

app.MapGet("/", () => "Hello World!");

app.Run();
