using Microsoft.AspNetCore.Http;

namespace MambaExam.Utilities;

public static class Extension
{
    public static bool CheckSizeFile(this IFormFile file, int size)
    {
        return file.Length / 1024 <= size;
    }

    public static bool CheckFormat(this IFormFile file, string format)
    {
        return file.ContentType.Contains(format);
    }

    public static async Task<string> CopyFileAsync(this IFormFile file, string wwwroot,params string[] folders)
    {
        var resultpath = wwwroot;
        var filename = Guid.NewGuid().ToString() + file.FileName;

        foreach (var folder in folders)
        {
            resultpath = Path.Combine(resultpath, folder);
        }
        resultpath = Path.Combine(resultpath, filename);
        using (FileStream stream = new FileStream(resultpath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return filename;
    }
}
