using Microsoft.AspNetCore.Mvc;

namespace Random_Numbers_API.Controllers
{
    [ApiController]
    [Route("random")]
    public class RandomController : ControllerBase
    {
        private readonly Random _random = new Random();

        [HttpGet("number")]
        public IActionResult GetRandomNumber()
        {
            int number = _random.Next(1, 101); // Genera número aleatorio entre 1 y 100
            return Ok(new { value = number });
        }
    }
}