using System.Data.OleDb;

namespace Phonebook.BusinessLogic
{
    public class Authentication
    {
        private const int DefaultAccessLevel = 3;

        public static int GetAccessLevel(string user)
        {
            string command = string.Format("SELECT AccessLevel " +
                                     "FROM AuthenticationInformation where UserName ='{0}';", user);
            OleDbConnection connection = new OleDbConnection(AbstractBLModels.ConnectionString);
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
                    Insert(user);
                    return DefaultAccessLevel;
                }
            }
            finally
            {
                oleDbCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public static void Insert(string user)
        {
            string command = string.Format(
                "insert into AuthenticationInformation (UserName, AccessLevel) values('{0}',{1})", user, DefaultAccessLevel);
            
            AbstractBLModels.SendQuery(command);
        }

        public static void Update(string user, int accessLevel)
        {
            string command = string.Format(
                "update AuthenticationInformation set AccessLevel={1} where  UserName='{0}')", user, accessLevel);

            AbstractBLModels.SendQuery(command);
        }
    }
}
