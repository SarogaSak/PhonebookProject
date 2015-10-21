using System.Collections.Generic;
using System.Linq;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    public class CollectionDepts
    {
        /// <summary>
        /// Объект для работы с базой. Таблица Depts.
        /// </summary>
        readonly AbstractBLModels blDept = new BLDept();

        public List<Dept> Depts { get; set; }

        public CollectionDepts(List<Dept> depts)
        {
            Depts = depts;
        }

        public CollectionDepts()
        {
            Depts = blDept.GetListData<List<Dept>>();
        }

        /// <summary>
        /// Возвращает список отделов с указанной подстрокой в названии.
        /// </summary>
        /// <param name="mask">Подстрока названия.</param>
        public List<Dept> FindDeptsForMask(string mask)
        {
            return Depts.Where(dept => dept.Name.ToLower().Contains(mask)).ToList();
        }

        public List<Dept> GetDeptsByEnterprise(int enterpriseId)
        {
            return enterpriseId == 0 ? Depts : Depts.Where(dept => dept.IdEnterprise==enterpriseId).ToList();
        }

        /// <summary>
        /// Возвращает Id отдела по названию.
        /// </summary>
        /// <param name="name">Название отдела.</param>
        public int GetIdByName(string name)
        {
            return Depts.First(dept => dept.Name.Contains(name)).Id;
        }

        /// <summary>
        /// Возвращает адрес отдела по названию.
        /// </summary>
        /// <param name="name">Название отдела.</param>
        public string GetAddressByName(string name)
        {
            return Depts.First(dept => dept.Name.Contains(name)).Address;
        }

        /// <summary>
        /// Обновляет значения в базе значениями из коллекции.
        /// </summary>
        public void Update()
        {
            foreach (Dept dept in Depts)
            {
                blDept.UpdateData(dept);
            }
        }

        public void InsertNew()
        {
            
        }

        public void DeleteById(int id)
        {
            blDept.DeleteData(id);
            Depts.Remove(Depts.First(dept => dept.Id == id));
        }

        public List<Dept> SortedList()
        {
            return Depts.OrderBy(dept => dept.SortOrder).ToList();
        }
    }
}
