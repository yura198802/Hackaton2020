using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.CrmDbModel.ModelDto.Parser;
using Microsoft.AspNetCore.Http;

namespace Hackaton.UniversalAdapter.Adapter.Loader
{
    public interface ILoaderFile
    {
        Task<bool> LoadFileByDatabase(string file);
        Task<bool> RemoveFile(string file);
        Task<bool> StoreFile(IFormFile formFile, string fileName);
        Task<DocumentLoader> SaveDocumentLoader(FileRtf fileRtf);
        Task<bool> RunAotParser(DocumentLoader documentLoader);
    }
}
