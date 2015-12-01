using System.Collections.Generic;
using System.Data.OleDb;
using Phonebook.Models;

namespace Phonebook.BusinessLogic
{
    class BLUser : AbstractBLModels
    {
        /// <summary>
        /// Получает список всех пользователей. 
        /// </summary>
        public override T GetListData<T>()
        {
            List<User> users = new List<User>();
            const string command = "SELECT * FROM AuthenticationInformation ORDER BY UserName";
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand oleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = oleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    users.Add(new User(
                        GetField<int>(dataReader, "Id"),
                        GetField<string>(dataReader, "UserName"),
                        GetField<int>(dataReader, "AccessLevel")));
                }
            }
            finally
            {
                oleDbCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return (T) (object) users;
        }

        /// <summary>
        /// Обновляет данные пользователя.
        /// </summary>
        /// <param name="model">Пользователь</param>
        public override void UpdateData<T>(T model)
        {
            var user = (User) (object) model;
            string updateString =
                string.Format(
                    "update AuthenticationInformation set UserName='{0}', AccessLevel={1} where Id={2}",
                    user.Name, user.AccessLevel, user.Id);
            SendQuery(updateString);
        }

        /// <summary>
        /// Добавляет нового пользователя в таблицу AuthenticationInformation.
        /// </summary>
        /// <param name="model">Пользователь</param>
        public override void InsertData<T>(T model)
        {
            var user = (User) (object) model;
            string insertString =
                string.Format(
                    "insert into AuthenticationInformation (UserName, AccessLevel) values('{0}',{1})",
                    user.Name, user.AccessLevel);
            SendQuery(insertString);
        }

        /// <summary>
        /// Удаляет пользователя из базы по ID.
        /// </summary>
        /// <param name="id">ID пользователя.</param>
        public override void DeleteData(int id)
        {
            string deleteString = string.Format("delete from AuthenticationInformation where id={0}", id);
            SendQuery(deleteString);
        }
    }
}
