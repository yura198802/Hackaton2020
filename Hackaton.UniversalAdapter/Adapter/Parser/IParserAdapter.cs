using System.Threading.Tasks;
using Hackaton.CrmDbModel.ModelDto.Parser;

namespace Hackaton.UniversalAdapter.Adapter.Parser
{
    public interface IParserAdapter
    {
        /// <summary>
        /// Метод пуска парсера
        /// </summary>
        Task<FileRtf> ParseDocument(byte[] fileContent);
    }
}
