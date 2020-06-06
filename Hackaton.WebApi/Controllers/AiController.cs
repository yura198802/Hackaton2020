using System.Threading.Tasks;
using Hackaton.AI.EngineAI.Interfaces;
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
        private readonly ICategotyClassifiction _categotyClassifiction;
        public static string ModuleName => @"AiController";

        public AiController(ICategotyClassifiction categotyClassifiction) : base(ModuleName)
        {
            _categotyClassifiction = categotyClassifiction;
        }

        
        /// <summary>
        /// тест работы платформы
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> RecogonizeMsg(string message)
        {
            return Tools.CreateResult(true, "", _categotyClassifiction.GetCategoryMessage(message));
        }

    }
}
