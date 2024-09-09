using EducateAPI.LoadGPTService.Interfaces;
using OpenAI.Chat;
using System.ClientModel;

namespace EducateAPI.LoadGPTService
{
    public class GPTClient : IGPTClient
    {
        private readonly IConfiguration _config;

        public GPTClient(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            try
            {
                // Flag to display generated content to console
                var debug = true;
                var responseOutput = string.Empty;

                var key = Environment.GetEnvironmentVariable("OPEN_API_KEY") ?? _config["OPEN_API_KEY"];

                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key), "Please configure Secret key");

                if (string.IsNullOrEmpty(prompt))
                    throw new ArgumentNullException(nameof(prompt), "No prompt input");

                ChatClient client = new(model: "gpt-4o-mini", key);

                var updates = client.CompleteChatStreamingAsync(prompt);

                Console.WriteLine("$[Assistant]:");
                await foreach (var update in updates)
                {
                    foreach (var updatePart in update.ContentUpdate)
                    {
                        if (debug) Console.Write(updatePart.Text);
                        responseOutput += updatePart.Text;
                    }
                }

                return responseOutput;
            }
            catch(Exception ex)
            {
                // TODO: Integrate Logging service.
                Console.WriteLine($"Exception in [GPTClient][GetResponseAsync] :{ex.Message}");
                throw;
            }
        }
    }
}
