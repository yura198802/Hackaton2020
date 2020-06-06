namespace Monica.Crm.WebApi.ModelsArgs
{
    /// <summary>
    /// Модель для получения параметров от клиента с логином и пароля пользователя для авторизации
    /// </summary>
    public class UserArgs
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
