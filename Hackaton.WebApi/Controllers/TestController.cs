using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monica.Core.Controllers;
using Monica.Crm.WebApi.ModelsArgs;

namespace Hackaton.WebApi.Controllers
{
    /// <summary>
    /// Основной контроллер для авторизации пользователей в системе
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : BaseController
    {

        public static string ModuleName => @"TestController";

        public TestController() : base(ModuleName) { }

        
        /// <summary>
        /// тест работы платформы
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> TestPlatform()
        {
            return Tools.CreateResult(true, "", new UserArgs() { Login = "Test", Password = "Test"});
        }

    }
}
