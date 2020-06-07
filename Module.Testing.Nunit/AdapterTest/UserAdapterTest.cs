using System.Linq;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.UniversalAdapter.Adapter.User;
using NUnit.Framework;

namespace Module.Testing.Nunit.AdapterTest
{
    public class UserAdapterTest : BaseServiceTest<IUserAdapter>
    {
        [Test]
        public async Task AddUser()
        {
            var user = new User()
            {
                Account = "test",
                Name = "test",
                Surname = "test",
                Middlename = "test"
            };
            var result = await Service.AddUser(user);
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task EditUser()
        {
            var user = new User()
            {
                Account = "test",
                Name = "test",
                Surname = "test",
                Middlename = "test",
                Id = 1
            };
            var result = await Service.EditUser(user);
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task RemoveUser()
        {
            var result = await Service.DeleteUser(1);
            Assert.IsTrue(result.Succeeded);
        }


        [Test]
        public async Task GetUser()
        {
            var models = await Service.GetUsers();
            Assert.IsTrue(models.Any());
        }

        [Test]
        public async Task GetUserDocuments()
        {
            var models = await Service.GetUserDocument(1);
            Assert.IsTrue(models.Any());
        }

        [Test]
        public async Task GetDocumentLoader()
        {
            var models = await Service.GetDocumentLoader(null);
            Assert.IsTrue(models.Any());
        }

        [Test]
        public async Task AddUserDocument()
        {
            var models = await Service.AddUserDocument(14,2);
            Assert.IsTrue(models != null);
        }
    }
}
