using System.Collections.Generic;
using System.Data.OleDb;
using Phonebook.Models;

namespace Phonebook.BusinessLogic
{
    class BLCurator : AbstractBLModels
    {
        public override T GetListData<T>()
        {
            List<Curator> curators = new List<Curator>();

            const string command = "SELECT Curators.* " +
                                   "FROM Curators;";

            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand oleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = oleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    curators.Add(new Curator(
                        GetField<int>(dataReader, "Id"),
                        GetField<string>(dataReader, "FIO"),
                        GetField<int>(dataReader, "SortOrder")));
                }
            }
            finally
            {
                oleDbCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return (T) (object) curators;
        }

        public override void UpdateData<T>(T model)
        {
            var curator = (Curator) (object) model;
            string updateString =
                string.Format("update Curators set FIO='{0}', SortOrder={1} where Id={2}", 
                curator.FIO, curator.SortOrder, curator.Id);
            SendQuery(updateString);
        }

        public override void InsertData<T>(T model)
        {
            var curator = (Curator) (object) model;
            string insertString = string.Format("insert into Curators (FIO, SortOrder) values('{0}',{1})",
                curator.FIO, curator.SortOrder);
            SendQuery(insertString);
        }

        public override void DeleteData(int id)
        {
            string deleteString = string.Format("delete from Curators where id={0}", id);
            SendQuery(deleteString);
        }
    }
}
