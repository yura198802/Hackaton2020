using System.IO;
using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.Loader;
using Hackaton.UniversalAdapter.Adapter.Parser;
using Monica.Core.Utils;
using NUnit.Framework;

namespace Module.Testing.Nunit.Parser
{
    public class ParserTest : BaseServiceTest<IParserAdapter>
    {
        private string _path = string.Empty;

        public ParserTest()
        {
            _path = @"C:\Users\opv\Desktop\ROSATOM\data\001.rtf";
        }

        [Test]
        public async Task ParserRtf_Normal()
        {
            var loaderFile = AutoFac.Resolve<ILoaderFile>();
            var file = await File.ReadAllBytesAsync(_path);
            var fileRtf = await Service.ParseDocument(file);
            var documentLoader = await loaderFile.SaveDocumentLoader(fileRtf);
            await loaderFile.RunAotParser(documentLoader);
            Assert.IsNotNull(fileRtf);
        }
    }
}
