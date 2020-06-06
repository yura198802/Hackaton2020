using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.CrmDbModel.ModelCrm.Core;

namespace Hackaton.CrmDbModel.Model.LoadDocument
{
    public class UserDocument : BaseModel
    {
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? DocumentLoaderId { get; set; }
        [ForeignKey("DocumentLoaderId")]
        public DocumentLoader DocumentLoader { get; set; }
    }
}
