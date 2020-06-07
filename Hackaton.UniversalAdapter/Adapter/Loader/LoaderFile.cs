using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.CrmDbModel.ModelDto.Parser;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Interface;
using Hackaton.UniversalAdapter.Adapter.Parser;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Monica.Core.Constants;
using Monica.Core.DataBaseUtils;
using MySql.Data.MySqlClient;

namespace Hackaton.UniversalAdapter.Adapter.Loader
{
    public class LoaderFile : ILoaderFile
    {
        private WordDbContext _wordDbContext;
        private IParserAdapter _parserAdapter;
        private IAiParser _aiParser;
        private IDataBaseMain _dataBaseMain;
        private string _pathDoc;

        public LoaderFile(WordDbContext wordDbContext, IParserAdapter parserAdapter, IAiParser aiParser, IDataBaseMain dataBaseMain)
        {
            _wordDbContext = wordDbContext;
            _parserAdapter = parserAdapter;
            _aiParser = aiParser;
            _dataBaseMain = dataBaseMain;
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
            documentLoader.ProfName = fileRtf.ProfName;
            await _wordDbContext.AddAsync(documentLoader);
            var query = fileRtf.Items.Where(f => f.ParentId == null);
            foreach (var item in query)
            {
                var documentItem = new DocumentItem();
                documentItem.DocumentLoader = documentLoader;
                documentItem.Number = item.Number;
                documentItem.IsRootItem = item.IsRoot;
                documentItem.TextContent = item.TextContent;
                await _wordDbContext.AddAsync(documentItem);
                await CreateModel(fileRtf.Items, item, documentItem, documentItem, documentLoader);
            }
            
            await _wordDbContext.SaveChangesAsync();
            
            var queryA = await _wordDbContext.DocumentItem.Where(f => f.DocumentLoaderId == documentLoader.Id).ToListAsync();
            foreach (var item in queryA.Where(f => f.ParentId == null))
            {
                await UpdateParagraph(queryA, item, item);

            }

            return documentLoader;
        }
        private async Task UpdateParagraph(List<DocumentItem> items, DocumentItem parent, DocumentItem paragraph)
        {
            var query = items.Where(f => f.ParentId == parent.Id);
            foreach (var item in query)
            {
                using (var connection = new MySqlConnection(_dataBaseMain.ConntectionString))
                {
                    await connection.ExecuteAsync(
                        $"Update documentItem set paragraphId = {paragraph.Id} where Id = {item.Id}");
                }
                await UpdateParagraph(items, item, paragraph);

            }
        }


        private async Task CreateModel(List<ItemDto> items, ItemDto parentId, DocumentItem parent, DocumentItem paragraph, DocumentLoader loader)
        {
            var query = items.Where(f => f.ParentId == parentId);
            foreach (var item in query)
            {
                var documentItem = new DocumentItem();
                documentItem.DocumentLoader = loader;
                documentItem.Number = item.Number;
                documentItem.IsRootItem = item.IsRoot;
                documentItem.TextContent = item.TextContent;
                documentItem.Parent = parent;
                documentItem.Paragraph = paragraph;
                await _wordDbContext.AddAsync(documentItem);
                await CreateModel(items, item, documentItem, paragraph, loader);

            }
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
