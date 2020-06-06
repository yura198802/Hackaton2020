using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.CrmDbModel.ModelCrm.Core;

namespace Hackaton.CrmDbModel.Model.Ai
{
    public class AiGroup : BaseModel
    {
        [ForeignKey("AiDescriptionId")]
        public AiDescription AiDescription { get; set; }
        public int? AiDescriptionId { get; set; }
        [ForeignKey("AiSentenceId")]
        public AiSentence AiSentence { get; set; }
        public int? AiSentenceId { get; set; }
        [ForeignKey("ParentId")]
        public AiGroup Parent { get; set; }
        public int? ParentId { get; set; }
        public bool IsGoup { get; set; }
        public bool IsSubj { get; set; }
        public int Last { get; set; }
        public int Start { get; set; }
    }
}
