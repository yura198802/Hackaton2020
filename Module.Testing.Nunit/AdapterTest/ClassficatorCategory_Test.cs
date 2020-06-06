using Hackaton.AI.EngineAI.Interfaces;
using NUnit.Framework;

namespace Module.Testing.Nunit.AdapterTest
{
    public class ClassficatorCategory_Test : BaseServiceTest<ICategotyClassifiction>
    {
        [Test]
        public void RecogonizeMsg_Normal()
        {
            //var body = "Тут сообщение которое написал мужичище. Причем жалобное нах...";
            var body = "У меня в воде плавают инородные предметы";
            var result = Service.GetCategoryMessage(body);
            Assert.IsNotEmpty(result);
        }
    }
}
