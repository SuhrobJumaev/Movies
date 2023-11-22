

using Microsoft.AspNetCore.Http;

namespace Movies.BusinessLogic;

public interface IVideoService
{
    Task<string> SaveVideoAsync(IFormFile video);
    void DeleteVideo(string videoName);
}
