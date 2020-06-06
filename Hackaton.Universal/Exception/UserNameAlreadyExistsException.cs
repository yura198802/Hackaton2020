using Monica.Core.Exceptions;

namespace Hackaton.Universal.Exception
{
    /// <summary>
    /// Исключение о том что пользователь уже существует
    /// </summary>
    public class UserNameAlreadyExistsException : UserMessageException
    {
        public UserNameAlreadyExistsException() : base("Пользователь с таким логином уже существует")
        {
        }

        public UserNameAlreadyExistsException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
