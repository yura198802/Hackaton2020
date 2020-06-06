using Hackaton.CrmDbModel.ModelCrm;
using Monica.Core.DataBaseUtils;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model;

namespace Module.Testing.NUnit
{
    public class BaseEfTest
    {
        private WordDbContext _crmDbContext;
        public BaseEfTest()
        {
            var mock = new Mock<IDataBaseMain>();
            mock.Setup(main => main.ConntectionString).Returns(
                "Server=localhost;Port=3306;Database=monicacrm;User Id=RassvetAis;Password=RassvetLine6642965;TreatTinyAsBoolean=true;");
            _crmDbContext = new WordDbContext(mock.Object);
        }
        [Test]
        public void GetTaskAsync()
        {
            Console.WriteLine("Test");
            
        }
    }
}
