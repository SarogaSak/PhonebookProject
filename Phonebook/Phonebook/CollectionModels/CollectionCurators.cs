using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    class CollectionCurators
    {
        /// <summary>
        /// Объект для работы с базой. Таблица Curators.
        /// </summary>
        readonly AbstractBLModels blCurator = new BLCurator();

        public List<Curator> Curators { get; set; }

        public CollectionCurators(List<Curator> curators)
        {
            Curators = curators;
        }

        public CollectionCurators()
        {
            Curators = blCurator.GetListData<List<Curator>>();
        }

        public void Update(CollectionCurators oldCollection)
        {
            foreach (var curator in Curators)
            {
                if (curator.Id == 0)
                {
                    blCurator.InsertData(curator);
                }
                else
                {
                    if (!oldCollection.GetCuratorById(curator.Id).Equals(curator))
                    {
                        blCurator.UpdateData(curator);
                    }
                }
            }
        }

        private Curator GetCuratorById(int id)
        {
            return Curators.First(curator => curator.Id == id);
        }

        public List<Curator> SortedList()
        {
            return Curators.OrderBy(curator => curator.SortOrder).ToList();
        }

        public void DeleteById(int id)
        {
            blCurator.DeleteData(id);
            Curators.Remove(Curators.First(curator => curator.Id == id));
        }
    }
}
