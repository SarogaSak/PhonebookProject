using System.Collections.Generic;
using System.Data.OleDb;
using Phonebook.Models;

namespace Phonebook.BusinessLogic
{
    class BLJob : AbstractBLModels
    {
        /// <summary>
        /// Получает список всех должностей.
        /// </summary>
        public override T GetListData<T>()
        {
            List<Job> jobs = new List<Job>();
            
            const string command = "select * from Jobs";
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand oleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = oleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    jobs.Add(new Job(
                        GetField<int>(dataReader, "Id"),
                        GetField<string>(dataReader, "Name"),
                        GetField<int>(dataReader, "SortOrder")));
                }
            }
            finally
            {
                oleDbCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return (T)(object)jobs;
        }

        /// <summary>
        /// Обновляет данные должности.
        /// </summary>
        /// <param name="model">Должность</param>
        public override void UpdateData<T>(T model)
        {
            var job = (Job) (object) model;
            string updateString = string.Format("update Jobs set Name='{0}', SortOrder={1} where Id={2}",
                job.Name, job.SortOrder, job.Id);
            SendQuery(updateString);
        }

        /// <summary>
        /// Добавляет пустую запись в таблицу Jobs и возвращает Id этой записи.
        /// </summary>
        public override void InsertData<T>(T model)
        {
            var job = (Job)(object)model;
            string insertString = string.Format("insert into Jobs (Name, SortOrder) values('{0}',{1})", 
                job.Name, job.SortOrder);
            SendQuery(insertString);
        }

        /// <summary>
        /// Удаляет должность из базы по Id.
        /// </summary>
        /// <param name="id">Id должности для удаления.</param>
        public override void DeleteData(int id)
        {
            string deleteString = string.Format("delete from Jobs where id={0}", id);
            SendQuery(deleteString);
        }
    }
}
