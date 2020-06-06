using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.Ai;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Interfaces;
using System.Linq;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Classes
{
    public class AiGroupEngine : IAiGroup
    {
        public async Task<List<AiGroup>> ParseGroups(WordDbContext wordDbContext, Variant variant, List<Word> words, AiSentence aiSentence)
        {
            List<AiGroup> result = new List<AiGroup>();
            var gropups = variant.Groups.ToList();
            int i = 0;
            foreach (var item in gropups)
            {
                AiGroup aiGroup = new AiGroup();
                aiGroup.AiDescription = await FindOrAddAiDescription(wordDbContext, item.Descr);
                aiGroup.Last = (int)item.Last;
                aiGroup.Start = (int)item.Start;
                aiGroup.IsSubj = item.IsSubj;
                aiGroup.IsGoup = item.IsGroup;
                item.AiGroup = aiGroup;
                SetParent(gropups, item, i == 0 ? -1 : i);
                result.Add(aiGroup);
                aiGroup.AiSentence = aiSentence;
                wordDbContext.Add(aiGroup);
                CreateAiWordsGroup(words, aiGroup, wordDbContext);
                i++;
            }
            //SetParentIdGroups(result);
            return result;
        }

        private void CreateAiWordsGroup(List<Word> words, AiGroup aiGroup, WordDbContext wordDbContext)
        {
            var cols = words.Skip(aiGroup.Start).Take(aiGroup.Last+1);
            foreach (var word in cols)
            {
                var aiGroupWord = new AiGroupWord();
                aiGroupWord.AiWord = word.AiWord;
                aiGroupWord.AiGroup = aiGroup;
                wordDbContext.Add(aiGroupWord);
            }
        }

        protected async Task<AiDescription> FindOrAddAiDescription(WordDbContext wordDbContext, string name)
        {
            AiDescription description = await wordDbContext.AiDescription.FirstOrDefaultAsync(f => f.Name == name);
            if (description == null)
            {
                description = new AiDescription() { Name = name };
                wordDbContext.AiDescription.Add(description);
                wordDbContext.Add(description);
            }
            return description;
        }

        protected virtual void SetParent(List<Group> groups, Group current, int pos)
        {
            if (pos == -1)
                return;
            if (pos == 0)
            {
                if (current.AiGroup != null)
                    current.AiGroup.Parent = groups[0].AiGroup;
                return;
            }
            var gOld = groups[pos - 1];
            if (gOld.Last >= current.Last && current.AiGroup != null)
                current.AiGroup.Parent = gOld.AiGroup;
            else SetParent(groups, current, pos - 1);
        }

        protected virtual async void SetParentIdGroups(List<AiGroup> aiGroups)
        {
            GetMaxLentghGroup(aiGroups);

        }
        protected virtual void GetMaxLentghGroup(IEnumerable<AiGroup> aiGroups)
        {
            var res = aiGroups.OrderByDescending(f => f.Last - f.Start);
            foreach (var item in res)
            {
                var target = aiGroups.Where(f => (f.Start >= item.Start && f.Last <= item.Last) && f != item);
                if (target == null || target.Count() == 0)
                    continue;
                else
                {
                    target.ToList().ForEach(f => f.Parent = item);
                    GetMaxLentghGroup(target);
                    continue;
                }

            }
        }
    }
}
