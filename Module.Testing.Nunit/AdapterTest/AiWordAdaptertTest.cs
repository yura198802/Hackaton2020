using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Interface;
using Hackaton.UniversalAdapter.Adapter.AiWord;
using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;

namespace Module.Testing.Nunit.AdapterTest
{
    public class AiWordAdaptertTest : BaseServiceTest<IAiWordAdapter>
    {

        [Test]
        public async Task GetAotRu()
        {
            var models = await Service.GetInfoDocument(1);
            Assert.IsTrue(models.Any());
        }



       
    }
}
