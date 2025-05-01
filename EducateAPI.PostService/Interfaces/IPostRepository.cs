
namespace EducateAPI.PostService.Interfaces
{
    public interface IPostRepository
    {
        public IEnumerable<DTOs.Post> GetAllPosts();
        public IEnumerable<DTOs.Post> GetAllUnseenPosts(Guid user);
        public void AddPost(DTOs.Post post);
        public void UpdatePost(DTOs.Post post);
        public void DeletePost(Guid postId);
        public void AddPosts(IEnumerable<DTOs.Post> posts);
    }
}
