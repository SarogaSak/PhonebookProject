using System;
using System.Data.OleDb;

namespace Phonebook.BusinessLogic
{
    class AccessHelper
    {
        /// <summary>
        /// Получает уровень доступа.
        /// </summary>
        /// <returns>AccessLevel = 0 : Администратор; по умочанию возвращает AccessLevel = 3</returns>
        public static int GetAccessLevel()
        {
            string command = string.Format("SELECT AccessLevel " +
                                                 "FROM AuthenticationInformation where UserName ='{0}';", Environment.UserDomainName + Environment.UserName);
            OleDbConnection connection = new OleDbConnection("");
            OleDbCommand oleDbCommand = new OleDbCommand(command, connection);

            try
            {
                connection.Open();
                OleDbDataReader dataReader = oleDbCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    return dataReader.GetInt32(0);
                }
                else
                {
                    return 3;
                }
            }
            finally
            {
                oleDbCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
