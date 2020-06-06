using System.Threading.Tasks;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Interface
{
    public interface IAiParser
    {
        Task<bool> Parse(string text);
    }
}
