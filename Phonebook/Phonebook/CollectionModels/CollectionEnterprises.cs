using System.Collections.Generic;
using System.Linq;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    class CollectionEnterprises
    {
        /// <summary>
        /// Объект для работы с базой. Таблица Enterprises.
        /// </summary>
        readonly AbstractBLModels blEnterprise = new BLEnterprise();

        public List<Enterprise> Enterprises { get; set; }

        public CollectionEnterprises(List<Enterprise> enterprises)
        {
            Enterprises = enterprises;
        }

        /// <summary>
        /// Конструктор. Инициализирует коллекцию значениями из базы.
        /// </summary>
        public CollectionEnterprises()
        {
            Enterprises = blEnterprise.GetListData<List<Enterprise>>();
        }

        /// <summary>
        /// Возвращает список предприятий с указанной подстрокой в названии.
        /// </summary>
        /// <param name="mask">Подстрока названия.</param>
        public List<Enterprise> FindEnterprisesForMask(string mask)
        {
            return Enterprises.Where(enterprise => enterprise.Name.ToLower().Contains(mask)).ToList();
        }
        /// <summary>
        /// Возвращает адрес предприятия по названию.
        /// </summary>
        /// <param name="name">Название предприятия.</param>
        public string GetAddressByName(string name)
        {
            return Enterprises.First(enterprise => enterprise.Name.Contains(name)).Address;
        }
        /// <summary>
        /// Возвращает Id предприятия по названию.
        /// </summary>
        /// <param name="name">Название предприятия.</param>
        public int GetIdByName(string name)
        {
            return Enterprises.First(enterprise => enterprise.Name.ToLower().Contains(name.ToLower())).Id;
        }

        /// <summary>
        /// Обновляет све записи в в таблице Enterprises значениями из коллекции.
        /// </summary>
        public void Update()
        {
            foreach (var enterprise in Enterprises)
            {
                blEnterprise.UpdateData(enterprise);
            }
        }
        /// <summary>
        /// Вставляет новую запись в таблицу Enterprises.
        /// </summary>
        public void InsertNew()
        {
            //int newItemId = AccessHelper.InsertNewEnterprise();
            //Enterprises.Add(new Enterprise(newItemId, "", "",9999));
            //return newItemId;
        }

        /// <summary>
        /// Удаляет запись из таблицы Enterprises с указанным Id.
        /// </summary>
        /// <param name="id">Id записи для удаления.</param>
        public void DeleteById(int id)
        {
            blEnterprise.DeleteData(id);
            Enterprises.Remove(Enterprises.First(enterprise => enterprise.Id==id));
        }

        /// <summary>
        /// Сортирует предприятия по коду сортировки.
        /// </summary>
        /// <returns></returns>
        public List<Enterprise> SortedList()
        {
            return Enterprises.OrderBy(enterprise => enterprise.SortOrder).ToList();
        }
    }
}
