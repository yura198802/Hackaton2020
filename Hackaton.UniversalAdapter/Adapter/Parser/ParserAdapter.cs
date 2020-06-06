using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.ModelDto.Parser;

namespace Hackaton.UniversalAdapter.Adapter.Parser
{

    public class ParserAdapterAdapter : IParserAdapter
    {
        private string _path = string.Empty;
        Reader _reader = new Reader();
        List<ItemDto> items = new List<ItemDto>();

        public Task<FileRtf> ParseDocument(byte[] fileContent)
        {
            var fileString = Encoding.UTF8.GetString(fileContent);
            return Task.FromResult(new FileRtf());
        }
        
        internal void ParseDoc()
        {
            var stringForParse =  _reader.ReadDoc();
        }
        private void CreateItem(string content)
        {
            
        }
    }
}
