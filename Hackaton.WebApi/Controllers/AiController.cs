using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.AiWord;
using Microsoft.AspNetCore.Mvc;
using Monica.Core.Controllers;

namespace Hackaton.WebApi.Controllers
{
    /// <summary>
    /// Основной контроллер для авторизации пользователей в системе
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class AiController : BaseController
    {
        private IAiWordAdapter _aiWord;
        public static string ModuleName => @"AiController";

        public AiController(IAiWordAdapter aiWord) : base(ModuleName)
        {
            _aiWord = aiWord;
        }

        
        /// <summary>
        /// тест работы платформы
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetInfoParagraph(int userId)
        {
            return Tools.CreateResult(true, "", await _aiWord.GetInfoDocument(userId));
        }

        /// <summary>
        /// тест работы платформы
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetDocumentInfo()
        {
            return Tools.CreateResult(true, "", _aiWord.GetDocumentInfo());
        }

    }

}
