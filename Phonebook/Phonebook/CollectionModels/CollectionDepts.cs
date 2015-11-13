using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    /// <summary>
    /// Коллекция отделов.
    /// </summary>
    public class CollectionDepts
    {
        /// <summary>
        /// Объект для работы с базой. Таблица Depts.
        /// </summary>
        readonly AbstractBLModels blDept = new BLDept();

        public List<Dept> Depts { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="depts">Список отделов.</param>
        public CollectionDepts(List<Dept> depts)
        {
            Depts = depts;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CollectionDepts()
        {
            Depts = blDept.GetListData<List<Dept>>();
        }

        /// <summary>
        /// Возвращает список всех отделов.
        /// </summary>
        public List<Dept> GetDepts()
        {
            return Depts.Where(dept => !dept.Name.Equals("Отдел")).ToList();
        }

        /// <summary>
        /// Возвращает список отделов с указанной подстрокой в названии.
        /// </summary>
        /// <param name="mask">Подстрока названия.</param>
        public List<Dept> FindDeptsForMask(string mask)
        {
            return Depts.Where(dept => dept.Name.ToLower().Contains(mask)).ToList();
        }

        /// <summary>
        /// Получает список отделов предприятия по его ID.
        /// </summary>
        /// <param name="enterpriseId">ID предприятия.</param>
        public List<Dept> GetDeptsByEnterprise(int enterpriseId)
        {
            return enterpriseId == 0 ? Depts : Depts.Where(dept => dept.IdEnterprise==enterpriseId).ToList();
        }

        /// <summary>
        /// Получает список отделов предприятия по его названию.
        /// </summary>
        /// <param name="enterpriseName">Название предприятия.</param>
        public List<Dept> GetDeptsByEnterprise(string enterpriseName)
        {
            return enterpriseName.Equals("") ? Depts : Depts.Where(dept => dept.EnterpriseName == enterpriseName).ToList();
        }

        /// <summary>
        /// Возвращает Id отдела по названию.
        /// </summary>
        /// <param name="nameDept">Название отдела.</param>
        public int GetIdByName(string nameDept, string nameInterprise)
        {
            List<Dept> tempDepts = GetDeptsByEnterprise(nameInterprise);
            return tempDepts.FirstOrDefault(dept => dept.Name.Equals(nameDept)).Id;
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
        public void Update(CollectionDepts oldCollection)
        {
            foreach (var dept in Depts)
            {
                if (dept.Id == 0)
                {
                    blDept.InsertData(dept);
                }
                else
                {
                    if (!oldCollection.GetDeptById(dept.Id).Equals(dept))
                    {
                        blDept.UpdateData(dept);
                    }
                }
            }
        }

        private Dept GetDeptById(int id)
        {
            return Depts.First(dept => dept.Id == id);
        }

        public void InsertNew(Dept newDept)
        {
            blDept.InsertData(newDept);
        }

        /// <summary>
        /// Удаляет отдел с указанным ID из базы.
        /// </summary>
        /// <param name="id">ID отдела.</param>
        public void DeleteById(int id)
        {
            blDept.DeleteData(id);
            Depts.Remove(Depts.First(dept => dept.Id == id));
        }

        public List<Dept> SortedList()
        {
            return Depts.OrderBy(dept => dept.SortOrder).ToList();
        }

        /// <summary>
        /// Получает список отделов по ФИО куратора.
        /// </summary>
        /// <param name="curatorFio">ФИО куратора.</param>
        public List<Dept> GetDeptByCurator(string curatorFio)
        {
            return Depts.Where(dept => dept.CuratorFio.Equals(curatorFio)).ToList();
        }

        /// <summary>
        /// Получает список отделов по ID куратора.
        /// </summary>
        /// <param name="curatorId">ID куратора.</param>
        public List<Dept> GetDeptByCurator(int curatorId)
        {
            return Depts.Where(dept => dept.IdCurator==curatorId).ToList();
        }
    }
}
