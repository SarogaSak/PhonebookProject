using System.Collections.Generic;
using System.Linq;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    /// <summary>
    /// Коллекция пользователей.
    /// </summary>
    class CollectionUsers
    {
        /// <summary>
        /// Объект для работы с базой. Таблица AuthenticationInformation.
        /// </summary>
        readonly AbstractBLModels blUser = new BLUser();

        public List<User> Users { get; set; }

        public CollectionUsers(List<User> users)
        {
            Users = users;
        }

        /// <summary>
        /// Конструктор. Инициализирует коллекцию значениями из базы.
        /// </summary>
        public CollectionUsers()
        {
            Users = blUser.GetListData<List<User>>();
        }

        /// <summary>
        /// Обновляет все записи в в таблице AuthenticationInformation значениями из коллекции.
        /// </summary>
        public void Update(CollectionUsers oldCollection)
        {
            foreach (var user in Users)
            {
                if (user.Id == 0)
                {
                    blUser.InsertData(user);
                }
                else
                {
                    if (!oldCollection.GetUserById(user.Id).Equals(user))
                    {
                        blUser.UpdateData(user);
                    }
                }
            }
        }

        /// <summary>
        /// Вставляет новую запись в таблицу AuthenticationInformation.
        /// </summary>
        public void InsertNew()
        {
            blUser.InsertData(new User());
        }

        /// <summary>
        /// Удаляет запись из таблицы AuthenticationInformation с указанным Id.
        /// </summary>
        /// <param name="id">Id записи для удаления.</param>
        public void DeleteById(int id)
        {
            blUser.DeleteData(id);
            Users.Remove(GetUserById(id));
        }

        private User GetUserById(int id)
        {
            return Users.First(user => user.Id == id);
        }
    }
}
