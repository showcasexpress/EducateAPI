namespace EducateAPI.LoadGPTService
{
    public interface IGPTClient
    {
        Task<string> GetResponseAsync(string prompt);
    }
}
