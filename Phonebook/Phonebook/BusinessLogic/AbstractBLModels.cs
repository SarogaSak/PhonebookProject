using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Windows;

namespace Phonebook.BusinessLogic
{
    public abstract class AbstractBLModels
    {
        protected const string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Phonebook.mdb; Jet OLEDB:Database Password=2159820";

        /// <summary>
        /// Получает коллекцию объектов из базы.
        /// </summary>
        /// <typeparam name="T">List&lt;Model&gt;</typeparam>
        public abstract T GetListData<T>();

        /// <summary>
        /// Обновляет запись в таблице.
        /// </summary>
        /// <typeparam name="T">Тип модели данных.</typeparam>
        /// <param name="model">Объект, который нужно обновить в таблице.</param>
        public abstract void UpdateData<T>(T model);

        /// <summary>
        /// Добавляет новую запись в таблицу.
        /// </summary>
        /// <typeparam name="T">Тип модели данных.</typeparam>
        /// <param name="model">Объект, который нужно добавить в таблицу.</param>
        public abstract void InsertData<T>(T model);

        /// <summary>
        /// Удаляет запись из таблицы.
        /// </summary>
        /// <param name="id">Id объекта, который нужно удалить.</param>
        public abstract void DeleteData(int id);

        /// <summary>
        /// Отправляет запрос к базе данных в отдельном потоке.
        /// </summary>
        /// <param name="queryString">Строка запроса.</param>
        protected static void SendQuery(string queryString)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                OleDbCommand oleDbCommand = new OleDbCommand(queryString, connection);
                try
                {
                    connection.Open();
                    oleDbCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    oleDbCommand.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Возвращает значение поля или значение по умолчанию в зависимости от типа данных.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="reader">Датаридер.</param>
        /// <param name="fieldName">Имя поля.</param>
        protected static T GetField<T>(OleDbDataReader reader, string fieldName)
        {
            T temp = default(T);
            try
            {
                temp = (T)reader.GetValue(reader.GetOrdinal(fieldName));
            }
            catch (Exception)
            {
                if (typeof(T) == typeof(int))
                {
                    temp = Activator.CreateInstance<T>();
                }
                if (typeof(T) == typeof(string))
                {
                    temp = (T)(object)"";
                }
            }

            return temp;
        }
    }
}
