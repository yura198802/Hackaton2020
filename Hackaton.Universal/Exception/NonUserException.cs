using Monica.Core.Exceptions;

namespace Hackaton.Universal.Exception
{
    /// <summary>
    /// Исключение о том, что не удалось найти пользователя в IdentityServer4
    /// </summary>
    public class NonUserException : UserMessageException
    {
        /// <summary>
        /// Конструктор с сообщением
        /// </summary>
        public NonUserException() : base("Не удалось определить пользователя")
        {
        }

        /// <summary>
        /// Конструктор с сообщением и базовым Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NonUserException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
