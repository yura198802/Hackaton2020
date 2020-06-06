namespace Hackaton.CrmDbModel.Extension
{
    /// <summary>
    /// Список ошибок обращения к БД
    /// </summary>
    public enum ErrorCode
    {
        NotUser = 1,
        NoCode =2
    }
    /// <summary>
    /// Тип компонентов
    /// </summary>
    public enum TypeControl
    {
        TextEdit,
        DateEdit,
        ComboBox,
        MultiLine,
        NumericEdit,
        FormSelect,
        CheckBox,
        Details
    }
    /// <summary>
    /// Тип уровня доступа к элементам
    /// </summary>
    public enum TypeAccec
    {
        Full = 3,
        ReadOnly = 2,
        NoAccess = 1
    }
    /// <summary>
    /// Тип профиля для полей модели. Обязателно или не обязательно
    /// </summary>
    public enum TypeProfileForm
    {
        Required =2,
        Advisably =1
    }

    public enum TypeField { List = 1, Edit = 2, ListAndEdit = 3 }
}
