using System;
using System.Data.SqlClient;

namespace Phonebook.Models
{
    /// <summary>
    /// Пользователи.
    /// </summary>
    class User
    {
        private const int DefaultAccessLevel = 3;

        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Уровень доступа, 0 - админ, 1 - высокий, 2-средний, 3- минимальный; по умолчанию = 3
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// MD5 хэш.
        /// </summary>
        public string MD5Hash { get; set; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="accessLevel">Уровень доступа</param>
        public User(int id, string name, int accessLevel)
        {
            Id = id;
            Name = name;
            AccessLevel = accessLevel;
        }

        public User()
        {
            Id = 0;
            Name = Environment.UserDomainName + "/" + Environment.UserName;
            AccessLevel = DefaultAccessLevel;
        }

        /// <summary>
        /// Сравнивает переданный объект с текущим.
        /// </summary>
        public bool Equals(User user)
        {
            return (Name.Equals(user.Name) && AccessLevel == user.AccessLevel);
        }
    }
}
