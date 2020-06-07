using Hackaton.CrmDbModel.ModelCrm.Core;

namespace Hackaton.CrmDbModel.Model.LoadDocument
{
    public class DocumentLoader : BaseModel
    {
        public string Name { get; set; }
        public string VidDoc { get; set; }
        public string Category { get; set; }
        public int FileSize { get; set; }
        public string  ProfName { get; set; }
    }
}
