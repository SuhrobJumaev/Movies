

using Microsoft.AspNetCore.Http;

namespace Movies.BusinessLogic;

public interface IVideoService
{
    Task<string> SaveVideoAsync(IFormFile video,CancellationToken token = default);
    void DeleteVideo(string videoName);
}
