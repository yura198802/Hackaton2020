using Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Interfaces;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.Ai;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Classes
{
    public class AiSentenceEngine : IAiSentence
    {
        private IAiGroup _aiGroup;
        private WordDbContext _wordDbContext;
        
        public AiSentenceEngine(IAiGroup aiGroup, WordDbContext wordDbContext)
        {
            _aiGroup = aiGroup;
            _wordDbContext = wordDbContext;
        }

        public virtual async Task<AiSentence>  Create(AotModel model)//TO:DO Тут должен быть массив
        {
            try
            {
                AiSentence aiSentence = new AiSentence();
                string result = string.Empty;
                int i = 0;
                foreach (var word in model.Words)
                {
                    result += word.Str + " ";
                    var aiWord = new AiWord();
                    aiWord.AiSentence = aiSentence;
                    aiWord.Text = word.Str;
                    aiWord.NormalizeText = word.Homonyms[0];
                    aiWord.Grm = string.Join("", model.Variants[0].Units[i].Grm);
                    aiWord.HomNo = model.Variants[0].Units[i].HomNo.ToString();
                    word.AiWord = aiWord;
                    _wordDbContext.Add(aiWord);
                    i++;
                }
                aiSentence.Text = result;
                _wordDbContext.Add(aiSentence);
                foreach (var modelVariant in model.Variants)
                {
                    await _aiGroup.ParseGroups(_wordDbContext, modelVariant, model.Words.ToList(), aiSentence);
                }
                return aiSentence;
            }
            catch(Exception ex)
            {
                throw new Exception("Не удалось создать предложение по словам полсе синтаксического разбора, ошибка -" + ex.Message);
            }
        }

        public async Task SaveDescription(List<AotModel> models)
        {
            foreach (var model in models)
            {
                foreach (var modelVar in model.Variants)
                {
                    foreach (var modelVarGroup in modelVar.Groups)
                    {
                        var description = await _wordDbContext.AiDescription.FirstOrDefaultAsync(f => f.Name == modelVarGroup.Descr);
                        if (description != null)
                            continue;
                        description = new AiDescription { Name = modelVarGroup.Descr};
                        _wordDbContext.Add(description);
                        await _wordDbContext.SaveChangesAsync();
                    }
                }
            }


        }
    }
}
