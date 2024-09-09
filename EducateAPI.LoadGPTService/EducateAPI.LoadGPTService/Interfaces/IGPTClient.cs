namespace EducateAPI.LoadGPTService.Interfaces
{
    public interface IGPTClient
    {
        Task<string> GetResponseAsync(string prompt);
    }
}
