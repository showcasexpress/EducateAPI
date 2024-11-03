using EducateAPI.LoadGPTService.Interfaces;
using OpenAI.Chat;
using System.ClientModel;

namespace EducateAPI.LoadGPTService
{
    public class GPTClient : IGPTClient
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;
        public GPTClient(IConfiguration config, IHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            try
            {
                var responseOutput = string.Empty;

                var key = Environment.GetEnvironmentVariable("OPEN_API_KEY") ?? _config["OPEN_API_KEY"];

                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key), "Please configure Secret key");

                if (string.IsNullOrEmpty(prompt))
                    throw new ArgumentNullException(nameof(prompt), "No prompt input");

                ChatClient client = new(model: "gpt-4o-mini", key);

                var updates = client.CompleteChatStreamingAsync(prompt);

                if (_env.IsDevelopment())
                    Console.WriteLine("$[Assistant]:");

                await foreach (var update in updates)
                {
                    foreach (var updatePart in update.ContentUpdate)
                    {
                        // Output callback to console in Development Environment
                        if (_env.IsDevelopment()) Console.Write(updatePart.Text);
                        responseOutput += updatePart.Text;
                    }
                }

                return responseOutput;
            }
            catch (Exception ex)
            {
                // TODO: Integrate Logging service.
                Console.WriteLine($"Exception in [GPTClient][GetResponseAsync] :{ex.Message}");
                throw;
            }
        }
    }
}
