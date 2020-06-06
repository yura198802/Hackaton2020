using System.IO;
using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.Parser;
using NUnit.Framework;

namespace Module.Testing.Nunit.Parser
{
    public class ParserTest : BaseServiceTest<IParserAdapter>
    {
        private string _path;

        public ParserTest()
        {
            _path = "TODO";
        }

        [Test]
        public async Task ParserRtf_Normal()
        {
            var file = await File.ReadAllBytesAsync(_path);
            var fileRtf = await Service.ParseDocument(file);
            Assert.IsNotNull(fileRtf);
        }
    }
}
