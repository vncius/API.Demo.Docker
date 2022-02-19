using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var connectionRedis = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
var urlDiscountGrpc = builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(opt => opt.Configuration = connectionRedis);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    opt => opt.Address = new Uri(urlDiscountGrpc)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
