using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.CrmDbModel.ModelCrm.Core;

namespace Hackaton.CrmDbModel.Model.Ai
{
    public class AiWord : BaseModel
    {
        public string Text { get; set; }
        public string NormalizeText { get; set; }
        [ForeignKey("AiSentenceId")]
        public AiSentence AiSentence { get; set; }
        public int? AiSentenceId { get; set; }
        public string Grm { get; set; }
        public string HomNo { get; set; }
    }
}