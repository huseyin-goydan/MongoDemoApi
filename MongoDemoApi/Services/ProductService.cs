using MongoDemoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MongoDemoApi.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _products;
    public ProductService(IOptions<MongoDemoApi.Models.MongoDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _products = database.GetCollection<Product>(settings.Value.CollectionName);
    }

    public async Task<List<Product>> GetAsync() =>
        await _products.Find(_ => true).ToListAsync();

    public async Task<Product?> GetAsync(string id) =>
        await _products.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product newProduct) =>
        await _products.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _products.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _products.DeleteOneAsync(x => x.Id == id);
}
