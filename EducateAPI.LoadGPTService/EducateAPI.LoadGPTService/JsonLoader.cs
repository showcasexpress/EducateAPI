using EducateAPI.LoadGPTService.Interfaces;

namespace EducateAPI.LoadGPTService
{
    public class JsonLoader : IJsonLoader
    {
        private readonly string _filePath;

        public JsonLoader(string filePath)
        {
            _filePath = filePath;
        }

        /* TODO : Periodically load this function
         * Iterate on the json objects, and send objects to adequate service queues based on their types.
         */

        public Task LoadAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
