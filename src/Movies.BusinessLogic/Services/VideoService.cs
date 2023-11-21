
using Microsoft.AspNetCore.Http;

namespace Movies.BusinessLogic;

public class VideoService : IVideoService
{
    public void DeleteVideo(string videoName)
    {
        File.Delete(Utils.PathToSaveFiles + videoName);
    }

    public async Task<string> SaveVideoAsync(IFormFile video)
    {
        string uniqueVideoName = GetUniqueVideoName(video.FileName);

        using var fileStream = new FileStream(Path.Combine(Utils.PathToSaveFiles, uniqueVideoName), FileMode.Create);

        await video.CopyToAsync(fileStream);
       
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
