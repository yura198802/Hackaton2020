using System.ComponentModel.DataAnnotations;
using Hackaton.CrmDbModel.ModelDto;

// ReSharper disable once CheckNamespace
namespace Hackaton.AI.DataModel
{
    public class h_message 
    {
        [Key]
        public int Sysid { get; set; }
        public Category Categor { get; set; }
        public Kind KindOf { get; set; }
        public string TextType { get; set; }
        public string TextCategory { get; set; }
        public string TextKind { get; set; }
        public string Vector { get; set; }
    }
}
