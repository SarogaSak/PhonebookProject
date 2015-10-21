using System.Collections.Generic;
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
    }
}
