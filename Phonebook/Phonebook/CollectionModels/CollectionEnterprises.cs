using System.Collections.Generic;
using System.Linq;
using Phonebook.Helpers;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    class CollectionEnterprises
    {
        public List<Enterprise> Enterprises { get; set; }

        public CollectionEnterprises(List<Enterprise> enterprises)
        {
            Enterprises = enterprises;
        }

        public CollectionEnterprises()
        {
            Enterprises = AccessHelper.GetEnterprises();
        }

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

        public void Update()
        {
            foreach (var enterprise in Enterprises)
            {
                AccessHelper.UpdateEnterpise(enterprise);
            }
        }

        public int InsertNew()
        {
            int newItemId = AccessHelper.InsertNewEnterprise();
            Enterprises.Add(new Enterprise(newItemId, "", "",9999));
            return newItemId;
        }

        public void DeleteById(int id)
        {
            AccessHelper.DeleteEnterprise(id);
            Enterprises.Remove(Enterprises.First(enterprise => enterprise.Id==id));
        }

        public List<Enterprise> SortedList()
        {
            return Enterprises.OrderBy(enterprise => enterprise.SortOrder).ToList();
        }
    }
}
