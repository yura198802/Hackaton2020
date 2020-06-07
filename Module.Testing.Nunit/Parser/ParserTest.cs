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
        private string _pathDir = string.Empty;

        public ParserTest()
        {
           // var i = 0;
            _path = @"C:\Users\Vasily\Downloads\case_28\ROSATOM\датасет\data\002.rtf";
            _pathDir = @"C:\Users\Vasily\Downloads\case_28\ROSATOM\датасет\data\";
           // Directory.GetFiles(_pathDir);
        }

        [Test]
        public async Task ParserRtf_Normal()
        {
            var loaderFile = AutoFac.Resolve<ILoaderFile>();
            foreach (var f in Directory.GetFiles(_pathDir))
            {
                var file = await File.ReadAllBytesAsync(f);
                var fileRtf = await Service.ParseDocument(file);
                var documentLoader = await loaderFile.SaveDocumentLoader(fileRtf);
                await loaderFile.RunAotParser(documentLoader);
                Assert.IsNotNull(fileRtf);
            }
        }
        [Test]
        public async Task ParserRtf()
        {
            foreach (var f in Directory.GetFiles(_pathDir))
            {
                var file = await File.ReadAllBytesAsync(f);
                var fileRtf = await Service.ParseDocument(file);
                Assert.IsNotNull(fileRtf);
            }
        }
    }
}
