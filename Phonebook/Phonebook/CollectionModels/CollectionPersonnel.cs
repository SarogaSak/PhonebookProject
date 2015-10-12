using System.Collections.Generic;
using System.Linq;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    class CollectionPersonnel
    {
        /// <summary>
        /// Объект для работы с базой. Таблица Personnel.
        /// </summary>
        readonly AbstractBLModels blPersonnel = new BLPerson();

        public List<Person> Personnel { get; set; }

        public CollectionPersonnel(List<Person> personnel)
        {
            Personnel = personnel;
        }

        public CollectionPersonnel()
        {
            Personnel = blPersonnel.GetListData<List<Person>>();
        }

        public List<Person> FindPersonnel(string fio, string jobName, string deptName, string phone)
        {
            return
                Personnel.Where(
                    person =>
                        person.Surname.ToLower().Contains(fio) && 
                        person.JobName.ToLower().Contains(jobName) &&
                        person.DeptName.ToLower().Contains(deptName) && 
                        person.LandlineNumbers.Contains(phone)).ToList();
        }

        public Person FindPersonForId(int id)
        {
            return Personnel.First(person => person.Id == id);
        }
    }
}
