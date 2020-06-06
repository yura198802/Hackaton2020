using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hackaton.UniversalAdapter.Adapter.Loader
{
    public interface ILoaderFile
    {
        Task<bool> LoadFileByDatabase(string file);
        Task<bool> RemoveFile(string file);
        Task<bool> StoreFile(IFormFile formFile, string fileName);
    }
}
