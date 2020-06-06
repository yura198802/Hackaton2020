using System;
using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Classes;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Interface;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace Module.Testing.Nunit.AdapterTest
{
    public class ConnectAotTest : BaseServiceTest<IAiParser>
    {
        private string _testString;

        public ConnectAotTest()
        {
            _testString =
                @"Вносить на рассмотрение руководства предложения по совершенствованию работы, связанной с предусмотренными настоящей инструкцией обязанностями.";


           // _testString =
             //@"Осуществляет контроль за рациональным оформлением помещений, следит за обновлением и состоянием рекламы в помещениях и на здании";
        }

        [Test]
        public async Task GetAotRu()
        {
            var models = await Service.Parse(_testString);
            Assert.IsTrue(models);
        }
    }
}
