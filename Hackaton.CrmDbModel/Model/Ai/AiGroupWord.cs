using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.CrmDbModel.ModelCrm.Core;

namespace Hackaton.CrmDbModel.Model.Ai
{
    public class AiGroupWord : BaseModel
    {
        [ForeignKey("AiWordId")]
        public AiWord AiWord { get; set; }
        public int? AiWordId { get; set; }
        [ForeignKey("AiGroupId")]
        public AiGroup AiGroup { get; set; }
        public int? AiGroupId { get; set; }
    }
}