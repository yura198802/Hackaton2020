using System.Collections.Generic;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.ModelDto.AiWord;

namespace Hackaton.UniversalAdapter.Adapter.AiWord
{

    public interface IAiWordAdapter
    {
        Task<IEnumerable<InfoDocument>> GetInfoDocument(int userId);

        List<InfoDocument> GetDocumentInfo();
    }
}
