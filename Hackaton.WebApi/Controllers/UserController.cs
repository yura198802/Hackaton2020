using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.UniversalAdapter.Adapter.User;
using Microsoft.AspNetCore.Mvc;
using Monica.Core.Controllers;

namespace Hackaton.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private IUserAdapter _userAdapter;
        public static string ModuleName => @"AiController";

        public UserController(IUserAdapter userAdapter) : base(ModuleName)
        {
            _userAdapter = userAdapter;
        }
        
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetUsers()
        {
            return Tools.CreateResult(true, "", await _userAdapter.GetUsers());
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddUser(User user)
        {
            return Tools.CreateResult(true, "", await _userAdapter.AddUser(user));
        }

        [HttpPost]
        public async Task<IActionResult> GetUserAsync(int userId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.GetUserModel(userId));
        }
        
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetUserDocument(int userId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.GetUserDocument(userId));
        }
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetDocumentLoader(int? userId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.GetDocumentLoader(userId)); 
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteUserDocument(int userDocId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.DeleteUserDocument(userDocId));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddUserDocument(int userId, int profId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.AddUserDocument(profId, userId));
        }



    }
}
