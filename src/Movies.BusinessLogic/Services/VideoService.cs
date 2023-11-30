
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;

namespace Movies.BusinessLogic;

public class VideoService : IVideoService
{
    private string _pathToWwwRoot;
    public VideoService(IWebHostEnvironment hostEnvironment)
    {
        _pathToWwwRoot = hostEnvironment.WebRootPath;
    }
    public void DeleteVideo(string videoName)
    {
        File.Delete(_pathToWwwRoot + videoName);
    }

    public async Task<string> SaveVideoAsync(IFormFile video, CancellationToken token = default)
    {
        string uniqueVideoName = GetUniqueVideoName(video.FileName);

        await Console.Out.WriteLineAsync(_pathToWwwRoot);
        using var fileStream = new FileStream(Path.Combine(_pathToWwwRoot, uniqueVideoName), FileMode.Create);

        await video.CopyToAsync(fileStream, token);

        await fileStream.FlushAsync();

        return uniqueVideoName;
    }

    private string GetUniqueVideoName(string fileName)
    {
        fileName = Path.GetFileName(fileName);
        return Path.GetFileNameWithoutExtension(fileName)
                  + Guid.NewGuid().ToString().Substring(0, 6)
                  + Path.GetExtension(fileName);
    }

    
}
