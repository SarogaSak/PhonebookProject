using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using Phonebook.Models;

namespace Phonebook.Helpers
{
    class AccessHelper
    {
        private const string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Phonebook.mdb; Jet OLEDB:Database Password=2159820";

        /// <summary>
        /// Получает список всех должностей.
        /// </summary>
        public static List<Job> GetJobs()
        {
            List<Job> jobs = new List<Job>();

            const string command = "select * from Jobs order by Job";
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand OleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = OleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    jobs.Add(new Job(GetField<int>(dataReader, "Id"), GetField<string>(dataReader, "Job")));
                }
            }
            finally
            {
                connection.Close();
            }

            return jobs;
        }
        /// <summary>
        /// Получает список всех предприятий.
        /// </summary>
        public static List<Enterprise> GetEnterprises()
        {
            List<Enterprise> enterprises = new List<Enterprise>();

            const string command = "select * from Enterprises order by Name";
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand OleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = OleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    enterprises.Add(new Enterprise(GetField<int>(dataReader, "Id"), GetField<string>(dataReader, "Address"), GetField<string>(dataReader, "Name")));
                }
            }
            finally
            {
                connection.Close();
            }
            return enterprises;
        }
        /// <summary>
        /// Получает список всех сотрудников.
        /// </summary>
        public static List<Person> GetPersonnel()
        {
            List<Person> persons = new List<Person>();

            const string command =
                "SELECT Personnel.Id, Personnel.Surname, Personnel.Name, Personnel.Secondname, Enterprises.Name, Jobs.Job, Personnel.LandlineNumbers, Personnel.CellNumbers, Personnel.InternalNumbers, Personnel.Photo " +
                "FROM Jobs INNER JOIN (Enterprises INNER JOIN Personnel ON Enterprises.Id = Personnel.Id_enterprise) ON Jobs.Id = Personnel.Id_job " +
                "ORDER BY Personnel.Surname;";
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand OleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = OleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    persons.Add(new Person(
                        GetField<int>(dataReader,"Id"),
                        GetField<string>(dataReader, "Surname"),
                        GetField<string>(dataReader, "Personnel.Name"),
                        GetField<string>(dataReader, "Secondname"),
                        GetField<string>(dataReader, "Enterprises.Name"),
                        GetField<string>(dataReader, "Job"),
                        GetField<string>(dataReader, "LandlineNumbers"),
                        GetField<string>(dataReader, "CellNumbers"),
                        GetField<string>(dataReader,"InternalNumbers"),
                        GetField<string>(dataReader, "Photo")));
                }
            }
            finally
            {
                connection.Close();
            }
            return persons;
        }
        /// <summary>
        /// Обновляет данные сотрудника.
        /// </summary>
        /// <param name="person">Сотрудник</param>
        /// <param name="idJob">Id должности</param>
        /// <param name="idEnterprise">Id предприятия</param>
        public static void UpdatePerson(Person person, int idJob, int idEnterprise)
        {
            string updateString =
                string.Format(
                    "update Personnel set Surname='{1}', Name='{2}', Secondname='{3}', Id_job={4}, Id_enterprise={5}, CellNumbers='{6}', LandlineNumbers='{7}', InternalNumbers='{8}', Photo='{9}' where Id={0}",
                    person.Id,person.Surname,person.Name,person.SecondName,idJob,idEnterprise,person.CellNumber,person.LandlineNumber,person.InternalNumber,person.Photo);
            SendQuery(updateString);
        }
        /// <summary>
        /// Добавляет нового сотрудника.
        /// </summary>
        /// <param name="person">Сотрудник.</param>
        /// <param name="idJob">Id должности</param>
        /// <param name="idEnterprise">Id предприятия</param>
        public static void InsertPerson(Person person, int idJob, int idEnterprise)
        {
            string insertString =
                string.Format(
                    "insert into Personnel (Surname, Name, Secondname, Id_job, Id_enterprise, CellNumbers, LandlineNumbers, InternalNumbers, Photo)" +
                    "values('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}','{8}')",
                    person.Surname,
                    person.Name,
                    person.SecondName,
                    idJob,
                    idEnterprise,
                    person.CellNumber,
                    person.LandlineNumber,
                    person.InternalNumber,
                    person.Photo);
            SendQuery(insertString);
        }
        /// <summary>
        /// Удаляет сотрудника по ID.
        /// </summary>
        /// <param name="id">ID сотрудника.</param>
        public static void DeletePerson(int id)
        {
            string deleteString = string.Format("delete from Personnel where id={0}", id);
            SendQuery(deleteString);
        }
        /// <summary>
        /// Обновляет данные должности.
        /// </summary>
        /// <param name="job">Должность</param>
        public static void UpdateJob(Job job)
        {
            string updateString = string.Format("update Jobs set Job='{0}' where Id={1}", job.JobName, job.Id);
            SendQuery(updateString);
        }
        /// <summary>
        /// Обновляет данные предприятия.
        /// </summary>
        /// <param name="enterprise">Предприятие.</param>
        public static void UpdateEnterpise(Enterprise enterprise)
        {
            string updateString = string.Format("update Enterprises set Name='{0}', Address='{1}' where Id={2}", enterprise.Name, enterprise.Address, enterprise.Id);
            SendQuery(updateString);
        }
        /// <summary>
        /// Добавляет пустую запись в таблицу Jobs и возвращает Id этой записи.
        /// </summary>
        public static int InsertNewJob()
        {
            string insertString = "insert into Jobs (Job) values('')";
            string getNewId = "SELECT max(Id) from Jobs";
            return SendQuery(insertString, getNewId);

        }
        /// <summary>
        /// Добавляет пустую запись в таблицу Enterprises и возвращает Id этой записи.
        /// </summary>
        /// <returns></returns>
        public static int InsertNewEnterprise()
        {
            string insertString = "insert into Enterprises (Name, Address) values('','')";
            string getNewId = "SELECT max(Id) from Enterprises";
            return SendQuery(insertString, getNewId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteJob(int id)
        {
            string deleteString = string.Format("delete from Jobs where id={0}", id);
            SendQuery(deleteString);
        }

        public static void DeleteEnterprise(int id)
        {
            string deleteString = string.Format("delete from Enterprises where id={0}", id);
            SendQuery(deleteString);
        }
        private static void SendQuery(string queryString)
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
                finally
                {
                    oleDbCommand.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            };
            worker.RunWorkerAsync();
        }

        private static int SendQuery(string queryString, string getNewId)
        {
            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand oleDbCommand = new OleDbCommand(queryString, connection);
            connection.Open();
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand = new OleDbCommand(getNewId, connection);
            var newId = (int)oleDbCommand.ExecuteScalar();
            oleDbCommand.Dispose();
            connection.Close();
            connection.Dispose();
            return newId;
        }
        /// <summary>
        /// Возвращает значение поля в зависимости от типа данных.
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="reader">Датаридер</param>
        /// <param name="fieldName">Имя поля</param>
        private static T GetField<T>(OleDbDataReader reader, string fieldName)
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
