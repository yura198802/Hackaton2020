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
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetUsers()
        {
            return Tools.CreateResult(true, "", await _userAdapter.GetUsers());
        }
        
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddUser(User user)
        {
            return Tools.CreateResult(true, "", await _userAdapter.AddUser(user));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> EditUser(User user)
        {
            return Tools.CreateResult(true, "", await _userAdapter.EditUser(user));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.DeleteUser(userId));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetUserDocument(int userId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.GetUserDocument(userId));
        }
        
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteUserDocument(int userDocId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.DeleteUserDocument(userDocId));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddUserDocument(int userId, int docId)
        {
            return Tools.CreateResult(true, "", await _userAdapter.AddUserDocument(userId, docId));
        }



    }
}
