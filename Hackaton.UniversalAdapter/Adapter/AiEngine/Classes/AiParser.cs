using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Interfaces;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Interface;

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

        public async Task<bool> Parse(string text, DocumentLoader documentLoader, DocumentItem documentItem)
        {
            var mass = text.Trim('.').Split('.');
            foreach (var sentence in mass)
            {
                var models = await _loaderInfoAotRu.LoaderAotModel(sentence);
                await _iAiSentence.SaveDescription(models);
                foreach (var model in models)
                {
                    var aiSentence = await _iAiSentence.Create(model);
                    aiSentence.DocumentLoader = documentLoader;
                    aiSentence.DocumentItem = documentItem;
                }
            }

            await _wordDbContext.SaveChangesAsync();
            return true;
        }

    }
}
