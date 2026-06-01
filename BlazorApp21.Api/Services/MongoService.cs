using MongoDB.Driver;
using BlazorApp21.Api.Models;
namespace BlazorApp21.Api.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<UserFavorite> _favorites;
        public MongoService(IMongoClient client)
        {
            var database = client.GetDatabase("BlazorApp21");
            _favorites = database.GetCollection<UserFavorite>("Favorites");
        }
        public async Task SaveFavorite(UserFavorite fav)
        {
            var existing = await _favorites
                .Find(f => f.UserId == fav.UserId)
                .FirstOrDefaultAsync();

            if (existing == null)
                await _favorites.InsertOneAsync(fav);
            else
                await _favorites.ReplaceOneAsync(f => f.UserId == fav.UserId, fav);
        }
        public async Task<UserFavorite?> GetFavorite(string userId)
        {
            return await _favorites
                .Find(f => f.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}