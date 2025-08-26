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
        // 2) GET /random/number/range?min=10&max=50  -> valida min/max
        [HttpGet("number/range")]
        public IActionResult GetRandomNumberInRange([FromQuery] int min, [FromQuery] int max)
        {
            if (min > max)
                return BadRequest("El valor de 'min' debe ser menor o igual a 'max'.");

            int number = _random.Next(min, max + 1); // incluye el máximo
            return Ok(new { value = number, min, max });
        }
        // 3) GET /random/decimal -> 0.0 .. 1.0
        [HttpGet("decimal")]
        public IActionResult GetRandomDecimal()
        {
            double number = _random.NextDouble();
            return Ok(new { value = number });
        }
        [HttpGet("string")]
        public IActionResult GetRandomString([FromQuery] int length = 8)
        {
            if (length < 1 || length > 1024)
            {
                return BadRequest("El parámetro 'length' debe estar entre 1 y 1024.");
            }

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] buffer = new char[length];

            for (int i = 0; i < length; i++)
            {
                buffer[i] = chars[_random.Next(chars.Length)];
            }

            string result = new string(buffer);
            return Ok(new { value = result, length });
        }
        public class RandomCustomRequest
        {
            public string Type { get; set; } = "number"; // "number", "decimal" o "string"
            public int Min { get; set; } = 1;
            public int Max { get; set; } = 100;
            public int Decimals { get; set; } = 2;
            public int Length { get; set; } = 8;
        }
        [HttpPost("custom")]
        public IActionResult GetCustomRandom([FromBody] RandomCustomRequest request)
        {
            if (string.IsNullOrEmpty(request.Type))
                return BadRequest("El campo 'type' es requerido.");

            switch (request.Type.ToLower())
            {
                case "number":
                    if (request.Min > request.Max)
                        return BadRequest("El parámetro 'min' debe ser menor o igual a 'max'.");
            
                    int number = _random.Next(request.Min, request.Max + 1);
                    return Ok(new { result = number });

                case "decimal":
                    double value = _random.NextDouble(); // entre 0 y 1
                    double rounded = Math.Round(value, request.Decimals > 0 ? request.Decimals : 2);
                    return Ok(new { result = rounded });

                case "string":
                    if (request.Length < 1 || request.Length > 1024)
                        return BadRequest("El parámetro 'length' debe estar entre 1 y 1024.");
            
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    char[] buffer = new char[request.Length];
                    for (int i = 0; i < request.Length; i++)
                        buffer[i] = chars[_random.Next(chars.Length)];
            
                    string result = new string(buffer);
                    return Ok(new { result });

                default:
                    return BadRequest("El campo 'type' debe ser 'number', 'decimal' o 'string'.");
            }
        }


    }
}

