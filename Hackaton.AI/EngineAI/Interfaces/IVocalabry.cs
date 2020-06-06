using System.Collections.Generic;
using Hackaton.CrmDbModel.ModelDto;

namespace Hackaton.AI.EngineAI.Interfaces
{
    public interface IVocalabry
    {
        List<WordNotPersistent> GetNonPersistentVocalabry();
    }
}
