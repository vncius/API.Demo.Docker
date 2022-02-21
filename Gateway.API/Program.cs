using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var fileOcelot = builder.Configuration.GetValue<string>("FileOcelotConfig:File");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddOcelot(builder.Configuration);

builder.WebHost.ConfigureAppConfiguration(
    cfg => cfg.AddJsonFile(Path.Combine("configuration", fileOcelot), 
                            optional: false, reloadOnChange: true)
);


var app = builder.Build();

app.UseSwaggerForOcelotUI();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();
