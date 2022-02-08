using MongoDB.Driver;
using static API.Demo.Docker.Entities.Products;

namespace API.Demo.Docker.Data
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
