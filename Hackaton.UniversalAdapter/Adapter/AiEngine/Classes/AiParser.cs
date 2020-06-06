using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Interfaces;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Classes
{
    public class AiParser : IAiParser
    {
        private readonly ILoaderInfoAotRu _loaderInfoAotRu;
        private readonly WordDbContext _wordDbContext;
        private readonly IAiSentence _iAiSentence;

        public AiParser(ILoaderInfoAotRu loaderInfoAotRu, WordDbContext wordDbContext, IAiSentence iAiSentence)
        {
            _loaderInfoAotRu = loaderInfoAotRu;
            _wordDbContext = wordDbContext;
            _iAiSentence = iAiSentence;
        }

        public async Task<bool> Parse(string text)
        {
            var models = await _loaderInfoAotRu.LoaderAotModel(text);
            await _iAiSentence.SaveDescription(models);
            foreach (var model in models)
            {
                await _iAiSentence.Create(model);
                await _wordDbContext.SaveChangesAsync();
            }
            return true;
        }

    }
}
