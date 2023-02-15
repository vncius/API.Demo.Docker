using MongoDB.Driver;
using static Catalog.API.Entities.Products;

namespace Catalog.API.Data
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
