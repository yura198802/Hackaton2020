namespace Hackaton.CrmDbModel.ModelDto.Core
{
    /// <summary>
    /// Базовая модель для Dto классов
    /// </summary>
    public class BaseModelDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
