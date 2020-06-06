using System.Collections.Generic;
using Hackaton.CrmDbModel.ModelDto;

namespace Hackaton.AI.EngineAI.Interfaces
{
    public interface IBigramm
    {
        EncodingWord GetFirstBigramm(IEnumerable<WordNotPersistent> vocalabryFirstBigramm, ref string tmpWordSource);
        List<EncodingWord> GetEndBigramms(IEnumerable<WordNotPersistent> vocalabryEndBigramm, ref string tmpSourceWord);
    }
}
