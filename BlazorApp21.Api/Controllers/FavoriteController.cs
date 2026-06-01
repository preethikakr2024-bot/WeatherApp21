using BlazorApp21.Api.Models;
using BlazorApp21.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp21.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly MongoService _mongo;

        public FavoriteController(MongoService mongo)
        {
            _mongo = mongo;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] UserFavorite fav)
        {
            if (fav == null)
                return BadRequest("Favorite cannot be null");

            await _mongo.SaveFavorite(fav);
            return Ok(new { message = "Saved successfully" });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("UserId is required");

            var data = await _mongo.GetFavorite(userId);
            return Ok(data);
        }
    }
}