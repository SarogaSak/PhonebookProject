using System.Collections.Generic;
using System.Data.OleDb;
using Phonebook.Models;

namespace Phonebook.BusinessLogic
{
    class BLPerson : AbstractBLModels
    {
        /// <summary>
        /// Получает список всех сотрудников.
        /// </summary>
        /// <typeparam name="T">"ListPerson"</typeparam>
        /// <returns></returns>
        public override T GetListData<T>()
        {
            List<Person> persons = new List<Person>();

            const string command =
                "SELECT  Personnel.*, Jobs.Name, Depts.Name " +
                "FROM Jobs INNER JOIN (Depts INNER JOIN Personnel ON Depts.Id = Personnel.IdDept) ON Jobs.Id = Personnel.IdJob;";
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand oleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = oleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    persons.Add(new Person(
                        GetField<int>(dataReader, "Id"),
                        GetField<string>(dataReader, "Surname"),
                        GetField<string>(dataReader, "Personnel.Name"),
                        GetField<string>(dataReader, "Secondname"),
                        GetField<int>(dataReader,"IdJob"),
                        GetField<string>(dataReader,"Jobs.Name"),
                        GetField<int>(dataReader,"IdDept"),
                        GetField<string>(dataReader,"Depts.Name"),
                        GetField<string>(dataReader, "CellNumbers"),
                        GetField<string>(dataReader, "LandlineNumbers"),
                        GetField<string>(dataReader, "InternalNumbers"),
                        GetField<string>(dataReader, "Photo"),
                        GetField<string>(dataReader, "Email")));
                }
            }
            finally
            {
                oleDbCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return (T) (object) persons;
        }

        /// <summary>
        /// Обновляет данные сотрудника.
        /// </summary>
        /// <param name="model">Сотрудник</param>
        public override void UpdateData<T>(T model)
        {
            var person = (Person) (object) model;
            string updateString =
                string.Format(
                    "update Personnel set Surname='{1}', Name='{2}', Secondname='{3}', IdJob={4}, IdDept={5}, CellNumbers='{6}', LandlineNumbers='{7}', InternalNumbers='{8}', Photo='{9}', Email='{10}' where Id={0}",
                    person.Id, 
                    person.Surname, 
                    person.Name, 
                    person.SecondName, 
                    person.IdJob, 
                    person.IdDept, 
                    person.CellNumbers,
                    person.LandlineNumbers, 
                    person.InternalNumbers, 
                    person.Photo, 
                    person.Email);
            SendQuery(updateString);
        }

        /// <summary>
        /// Добавляет нового сотрудника.
        /// </summary>
        /// <typeparam name="T">"Person"</typeparam>
        /// <param name="model">Сотрудник.</param>
        /// <returns></returns>
        public override void InsertData<T>(T model)
        {
            var person = (Person) (object) model;
            string insertString =
                string.Format(
                    "insert into Personnel (Surname, Name, Secondname, IdJob, IdDept, CellNumbers, LandlineNumbers, InternalNumbers, Photo, Email)" +
                    "values('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}','{8}','{9}')",
                    person.Surname,
                    person.Name,
                    person.SecondName,
                    person.IdJob,
                    person.IdDept,
                    person.CellNumbers,
                    person.LandlineNumbers,
                    person.InternalNumbers,
                    person.Photo,
                    person.Email);
            SendQuery(insertString);
        }

        /// <summary>
        /// Удаляет сотрудника по ID.
        /// </summary>
        /// <param name="id">ID сотрудника.</param>
        public override void DeleteData(int id)
        {
            string deleteString = string.Format("delete from Personnel where id={0}", id);
            SendQuery(deleteString);
        }
    }
}
