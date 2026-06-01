using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorApp21.Api.Models
{
    public class UserFavorite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string FavoriteWeather { get; set; } = string.Empty;
    }
}