using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model.LoadDocument;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Interface
{
    public interface IAiParser
    {
        Task<bool> Parse(string text, DocumentLoader documentLoader, DocumentItem documentItem);
    }
}
