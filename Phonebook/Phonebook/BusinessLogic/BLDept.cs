using System.Collections.Generic;
using System.Data.OleDb;
using Phonebook.Models;

namespace Phonebook.BusinessLogic
{
    class BLDept : AbstractBLModels
    {
        public override T GetListData<T>()
        {
            List<Dept> depts = new List<Dept>();

            const string command = "SELECT Depts.*, Enterprises.Name, Curators.FIO " +
                                   "FROM Curators INNER JOIN (Enterprises INNER JOIN Depts ON Enterprises.Id=Depts.IdEnterprise) ON Curators.Id=Depts.IdCurator " +
                                   "ORDER BY Curators.Id, Enterprises.Id, Depts.SortOrder;";

            OleDbConnection connection = new OleDbConnection(ConnectionString);
            OleDbCommand oleDbCommand = new OleDbCommand(command, connection);
            try
            {
                connection.Open();
                OleDbDataReader dataReader = oleDbCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    depts.Add(new Dept(
                        GetField<int>(dataReader,"Id"),
                        GetField<string>(dataReader,"Depts.Name"),
                        GetField<string>(dataReader,"Address"),
                        GetField<int>(dataReader,"SortOrder"),
                        GetField<int>(dataReader, "IdEnterprise"),
                        GetField<string>(dataReader, "Enterprises.Name"),
                        GetField<int>(dataReader,"IdCurator"),
                        GetField<string>(dataReader,"FIO")));
                }
            }
            finally
            {
                oleDbCommand.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return (T) (object) depts;
        }

        public override void UpdateData<T>(T model)
        {
            var dept = (Dept) (object) model;
            string updateString =
                string.Format(
                    "update Depts set Name='{0}', Address='{1}', SortOrder={2}, IdEnterprise={3}, IdCurator={4} where Id={5}",
                    dept.Name, dept.Address, dept.SortOrder, dept.IdEnterprise, dept.IdCurator, dept.Id);
            SendQuery(updateString);
        }

        public override void InsertData<T>(T model)
        {
            var dept = (Dept)(object)model;
            string insertString =
                string.Format(
                    "insert into Depts (Name, Address, SortOrder, IdEnterprise, IdCurator) values('{0}','{1}',{2},{3},{4})",
                    dept.Name, dept.Address, dept.SortOrder, dept.IdEnterprise, dept.IdCurator);
            SendQuery(insertString);
        }

        public override void DeleteData(int id)
        {
            string deleteString = string.Format("delete from Depts where id={0}", id);
            SendQuery(deleteString);
        }
    }
}
