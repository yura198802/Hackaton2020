using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.Loader;
using Hackaton.UniversalAdapter.Adapter.Parser;
using Hackaton.WebApi.ModelsArgs;
using Microsoft.AspNetCore.Mvc;
using Monica.Core.Controllers;

namespace Hackaton.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DocumentLoaderController : BaseController
    {
        private readonly ILoaderFile _loaderFile;
        private IParserAdapter _parserAdapter;
        public static string ModuleName => @"DocumentLoaderController";

        public DocumentLoaderController(ILoaderFile loaderFile, IParserAdapter parserAdapter) : base(ModuleName)
        {
            _loaderFile = loaderFile;
            _parserAdapter = parserAdapter;
        }

        /// <summary>
        /// Функция загружает файл 
        /// </summary>
        /// <param name="modelInputUploadFile"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UploadFile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostUploadFile([FromForm][Required]ModelUploadFile modelInputUploadFile)
        {
            await _loaderFile.StoreFile(modelInputUploadFile.Files, modelInputUploadFile.FileName);
            return Tools.CreateResult(true, "", true);
        }

        /// <summary>
        /// Функция загружает файл 
        /// </summary>
        /// <param name="modelInputUploadFile"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UploadFielMoment")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadFielMoment([FromForm][Required]ModelUploadFile modelInputUploadFile)
        {
            using (var fs = new MemoryStream())
            {
                await modelInputUploadFile.Files.CopyToAsync(fs);
                var models = await _parserAdapter.ParseDocument(fs.ToArray());
                var documentLoader = await _loaderFile.SaveDocumentLoader(models);
                await _loaderFile.RunAotParser(documentLoader);
                return Tools.CreateResult(true, "", documentLoader.Id);
            }
        }

    }
}
