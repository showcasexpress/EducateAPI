using Microsoft.AspNetCore.Mvc;

namespace EducateAPI.LoadGPTService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GPTClientController : ControllerBase
    {
        private readonly GPTClient _gptClient;
        public GPTClientController(GPTClient gptClient)
        {
            _gptClient = gptClient;
        }

        [HttpGet("{prompt}")]
        public async Task<IActionResult> GetResponse(string prompt)
        {
            try
            {
                return await Task.FromResult(Ok(await _gptClient.GetResponseAsync(prompt)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating text: {ex.Message}");
            }
        }
    }
}