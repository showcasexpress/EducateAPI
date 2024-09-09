using EducateAPI.LoadGPTService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EducateAPI.LoadGPTService.Controllers
{
    [ApiController]
    [Route("api/v1/gpt")]
    public class GPTClientController : ControllerBase
    {
        private readonly IGPTClient _gptClient;
        public GPTClientController(IGPTClient gptClient)
        {
            _gptClient = gptClient;
        }

        [HttpGet("{prompt}")]
        public async Task<IActionResult> GetResponse(string prompt)
        {
            try
            {
                return Ok(await _gptClient.GetResponseAsync(prompt));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating text: {ex.Message}");
            }
        }
    }
}