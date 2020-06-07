using System;
using System.Collections.Generic;
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
            //_testString =
            //    @"Вносить на рассмотрение руководства предложения по совершенствованию работы, связанной с предусмотренными настоящей инструкцией обязанностями.";

            //_testString =
            //    @"Обеспечивает надлежащее оформление заключаемых договоров и контрактов, других необходимых документов, в том числе страховых  и экспортных лицензий.";


            _testString =
           @"Согласовывает их с руководством учреждения (организации), осуществляет контроль за их выполнением. Уведомляет членов приемных и экзаменационных комиссий, аспирантов и соискателей о времени и месте проведения экзаменов.";
        }

        [Test]
        public async Task GetAotRu()
        {
            var models = await Service.Parse(_testString, null, null);
            Assert.IsTrue(models);
        }



       
    }
}
