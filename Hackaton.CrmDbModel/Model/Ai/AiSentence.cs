using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.CrmDbModel.ModelCrm.Core;

namespace Hackaton.CrmDbModel.Model.Ai
{
    public class AiSentence : BaseModel
    {
        public int? DocumentLoaderId { get; set; }
        [ForeignKey("DocumentLoaderId")]
        public DocumentLoader DocumentLoader { get; set; }
        public int? DocumentItemId { get; set; }
        [ForeignKey("DocumentItemId")]
        public DocumentItem DocumentItem { get; set; }
        public string Text { get; set; }
    }
}
