using Hackaton.AI.DataModel;
using Hackaton.CrmDbModel.Model.Ai;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.CrmDbModel.ModelCrm.Core;
using Microsoft.EntityFrameworkCore;
using Monica.Core.DataBaseUtils;

namespace Hackaton.CrmDbModel.Model
{
    public class WordDbContext : BaseDbContext
    {
        public DbSet<h_word> h_word { get; set; }
        public DbSet<h_message> h_message { get; set; }
        public DbSet<AiDescription> AiDescription { get; set; }
        public DbSet<AiGroup> AiGroup { get; set; }
        public DbSet<AiSentence> AiSentence { get; set; }
        public DbSet<AiGroupWord> AiGroupWord { get; set; }
        public DbSet<AiWord> AiWord { get; set; }
        public DbSet<DocumentLoader> DocumentLoader { get; set; }
        public DbSet<DocumentItem> DocumentItem { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserDocument> UserDocument { get; set; }

        public WordDbContext(IDataBaseMain dataBaseMain) : base(dataBaseMain)
        {

        }
    }
}
