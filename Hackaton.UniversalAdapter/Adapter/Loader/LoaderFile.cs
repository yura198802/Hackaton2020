using System.IO;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model;
using Hackaton.UniversalAdapter.Adapter.Parser;
using Microsoft.AspNetCore.Http;
using Monica.Core.Constants;

namespace Hackaton.UniversalAdapter.Adapter.Loader
{
    public class LoaderFile : ILoaderFile
    {
        private WordDbContext _wordDbContext;
        private IParserAdapter _parserAdapter;
        private string _pathDoc;

        public LoaderFile(WordDbContext wordDbContext, IParserAdapter parserAdapter)
        {
            _wordDbContext = wordDbContext;
            _parserAdapter = parserAdapter;
            _pathDoc = Path.Combine(GlobalSettingsApp.CurrentAppDirectory, "Documents");
        }


        public async Task<bool> LoadFileByDatabase(string file)
        {
            var fileByte = await File.ReadAllBytesAsync(file);
            var model = _parserAdapter.ParseDocument(fileByte);
            return true;
        }

        public Task<bool> RemoveFile(string file)
        {
            var pathCopy = Path.Combine(Path.GetDirectoryName(file), "Loaded");
            if (!Directory.Exists(pathCopy))
                Directory.CreateDirectory(pathCopy);
            File.Copy(file, Path.Combine(pathCopy, Path.GetFileName(file)));
            File.Delete(file);
            return Task.FromResult(true);
        }

        public async Task<bool> StoreFile(IFormFile formFile, string fileName)
        {
            if (!Directory.Exists(_pathDoc))
                Directory.CreateDirectory(_pathDoc);
            var fileFullPath = Path.Combine(_pathDoc, fileName);
            using (var fs = new FileStream(fileFullPath, FileMode.OpenOrCreate))
            {
                await formFile.CopyToAsync(fs);
            }

            return true;
        }
    }
}
