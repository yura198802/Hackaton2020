using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.Ai;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Interfaces
{
    public interface IAiGroup
    {
        Task<List<AiGroup>> ParseGroups(WordDbContext wordDbContext, Variant variant, List<Word> words,
            AiSentence aiSentence);
    }
}
