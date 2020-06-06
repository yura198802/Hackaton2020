using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.CrmDbModel.ModelDto.Parser;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Interface;
using Hackaton.UniversalAdapter.Adapter.Parser;
using Microsoft.AspNetCore.Http;
using Monica.Core.Constants;

namespace Hackaton.UniversalAdapter.Adapter.Loader
{
    public class LoaderFile : ILoaderFile
    {
        private WordDbContext _wordDbContext;
        private IParserAdapter _parserAdapter;
        private IAiParser _aiParser;
        private string _pathDoc;

        public LoaderFile(WordDbContext wordDbContext, IParserAdapter parserAdapter, IAiParser aiParser)
        {
            _wordDbContext = wordDbContext;
            _parserAdapter = parserAdapter;
            _aiParser = aiParser;
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

        public async Task<DocumentLoader> SaveDocumentLoader(FileRtf fileRtf)
        {
            var documentLoader = new DocumentLoader();
            documentLoader.Name = fileRtf.Caption;
            documentLoader.Category = fileRtf.Category;
            documentLoader.FileSize = fileRtf.FileSize;
            documentLoader.VidDoc = fileRtf.VidDoc;
            _wordDbContext.Add(documentLoader);
            foreach (var item in fileRtf.Items)
            {
                
                var documentItem = new DocumentItem();
                documentItem.DocumentLoader = documentLoader;
                documentItem.Number = item.Number;
                documentItem.IsRootItem = item.IsRoot;
                documentItem.TextContent = item.TextContent;
                item.DocumentItem = documentItem;
                documentItem.Parent = item.ParentId?.DocumentItem;
                _wordDbContext.Add(documentItem);
            }

            await _wordDbContext.SaveChangesAsync();
            return documentLoader;
        }

        public async Task<bool> RunAotParser(DocumentLoader documentLoader)
        {
            var items = _wordDbContext.DocumentItem.Where(f => f.DocumentLoader == documentLoader);
            foreach (var item in items)
            {
                if (item.IsRootItem == true)
                    continue;
                await _aiParser.Parse(item.TextContent, documentLoader, item);
            }

            return true;
        }

    }
}
