using System.Collections.Generic;
using System.Data.OleDb;
using Phonebook.Models;

namespace Phonebook.BusinessLogic
{
    class BLEnterprise : AbstractBLModels
    {
        /// <summary>
        /// Получает список всех предприятий.
        /// </summary>
        public override T GetListData<T>()
        {
            List<Enterprise> enterprises = new List<Enterprise>();

            const string command = "SELECT Enterprises.*, Curators.FIO " +
                                   "FROM Curators RIGHT JOIN Enterprises ON Curators.Id=Enterprises.IdCurator " +
                                   "ORDER BY IdCurator;";
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand oleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = oleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    enterprises.Add(new Enterprise(
                        GetField<int>(dataReader, "Id"),
                        GetField<string>(dataReader, "Name"),
                        GetField<string>(dataReader, "Address"),
                        GetField<int>(dataReader, "SortOrder"),
                        GetField<int>(dataReader, "IdCurator"),
                        GetField<string>(dataReader, "FIO")));
                }
            }
            finally
            {
                oleDbCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return (T) (object) enterprises;
        }

        /// <summary>
        /// Обновляет данные предприятия.
        /// </summary>
        /// <param name="model">Предприятие.</param>
        public override void UpdateData<T>(T model)
        {
            var enterprise = (Enterprise) (object) model;
            string updateString =
                string.Format(
                    "update Enterprises set Name='{0}', Address='{1}', SortOrder={2}, IdCurator={3} where Id={4}",
                    enterprise.Name, enterprise.Address, enterprise.SortOrder, enterprise.IdCurator, enterprise.Id);
            SendQuery(updateString);
        }

        /// <summary>
        /// Добавляет пустую запись в таблицу Enterprises и возвращает Id этой записи.
        /// </summary>
        /// <param name="model">Предприятие.</param>
        public override void InsertData<T>(T model)
        {
            var enterprise = (Enterprise) (object) model;
            string insertString =
                string.Format(
                    "insert into Enterprises (Name, Address, SortOrder, IdCurator) values('{0}','{1}',{2}),{3}",
                    enterprise.Name, enterprise.Address, enterprise.SortOrder, enterprise.IdCurator);
            SendQuery(insertString);
        }

        /// <summary>
        /// Удаляет предприятие из базы по Id.
        /// </summary>
        /// <param name="id">Id предприятия для удаления.</param>
        public override void DeleteData(int id)
        {
            string deleteString = string.Format("delete from Enterprises where id={0}", id);
            SendQuery(deleteString);
        }
    }
}
