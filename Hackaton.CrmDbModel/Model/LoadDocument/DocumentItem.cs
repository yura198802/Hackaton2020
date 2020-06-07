using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.CrmDbModel.ModelCrm.Core;

namespace Hackaton.CrmDbModel.Model.LoadDocument
{
    public class DocumentItem : BaseModel
    {
        public string Number { get; set; }
        public string TextContent { get; set; }

        [ForeignKey("ParentId")]
        public DocumentItem Parent { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("DocumentLoaderId")]
        public DocumentLoader DocumentLoader { get; set; }
        public int? DocumentLoaderId { get; set; }

        public bool? IsRootItem { get; set; }


        [ForeignKey("ParagraphId")]
        public DocumentItem Paragraph { get; set; }
        public int? ParagraphId { get; set; }
    }
}