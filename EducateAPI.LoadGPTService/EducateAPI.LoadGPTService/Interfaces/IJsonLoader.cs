namespace EducateAPI.LoadGPTService.Interfaces
{
    public interface IJsonLoader
    {
        public Task LoadAsync(string path);
    }
}
