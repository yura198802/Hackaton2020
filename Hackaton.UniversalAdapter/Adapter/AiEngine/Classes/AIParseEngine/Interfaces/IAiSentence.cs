using Hackaton.CrmDbModel.Model.Ai;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Interfaces
{
    public interface IAiSentence
    {
         Task<AiSentence> Create(AotModel model);
         Task SaveDescription(List<AotModel> models);
    }
}
