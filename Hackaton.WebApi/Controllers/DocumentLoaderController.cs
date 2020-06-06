using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.Loader;
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
        public static string ModuleName => @"DocumentLoaderController";

        public DocumentLoaderController(ILoaderFile loaderFile) : base(ModuleName)
        {
            _loaderFile = loaderFile;
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

    }
}
