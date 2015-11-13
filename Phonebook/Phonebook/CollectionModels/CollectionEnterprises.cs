using System.Collections.Generic;
using System.Linq;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    /// <summary>
    /// Коллекция предприятий.
    /// </summary>
    public class CollectionEnterprises
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
            //Enterprises = SortedList();
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
            var firstOrDefault = Enterprises.FirstOrDefault(enterprise => enterprise.Name.ToLower().Equals(name.ToLower()));
            if (firstOrDefault != null)
                return firstOrDefault.Id;
            return 0;
        }

        /// <summary>
        /// Обновляет све записи в в таблице Enterprises значениями из коллекции.
        /// </summary>
        public void Update(CollectionEnterprises oldCollection)
        {
            foreach (var enterprise in Enterprises)
            {
                if (enterprise.Id == 0)
                {
                    blEnterprise.InsertData(enterprise);
                }
                else
                {
                    if (!oldCollection.GetEnterprisesById(enterprise.Id).Equals(enterprise))
                    {
                        blEnterprise.UpdateData(enterprise);
                    }
                }
            }
        }

        private Enterprise GetEnterprisesById(int id)
        {
            return Enterprises.First(enterprise => enterprise.Id == id);
        }

        /// <summary>
        /// Вставляет новую запись в таблицу Enterprises.
        /// </summary>
        public void InsertNew(Enterprise newEnterprise)
        {
            blEnterprise.InsertData(newEnterprise);
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
        public List<Enterprise> SortedList()
        {
            return Enterprises.OrderBy(enterprise => enterprise.SortOrder).ToList();
        }

        /// <summary>
        /// Получает список предприятий по ID куратора.
        /// </summary>
        /// <param name="id">ID куратора.</param>
        public List<Enterprise> GetEnterprisesByCurator(int id)
        {
            return Enterprises.Where(enterprise => enterprise.IdCurator == id).ToList();
        }

        /// <summary>
        /// Получает список предприятий по ФИО куратора.
        /// </summary>
        /// <param name="fio">ФИО куратора.</param>
        public List<Enterprise> GetEnterprisesByCurator(string fio)
        {
            return Enterprises.Where(enterprise => enterprise.CuratorFio.Equals(fio)).ToList();
        }
    }
}
