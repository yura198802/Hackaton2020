using Hackaton.CrmDbModel.ModelCrm.Core;

namespace Hackaton.CrmDbModel.Model.LoadDocument
{
    public class User : BaseModel
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }
    }
}
