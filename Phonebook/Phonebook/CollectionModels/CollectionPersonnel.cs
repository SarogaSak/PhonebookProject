using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Phonebook.Helpers;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    class CollectionPersonnel
    {
        public List<Person> Personnel { get; set; }

        public CollectionPersonnel(List<Person> personnel)
        {
            Personnel = personnel;
        }

        public CollectionPersonnel()
        {
            Personnel = AccessHelper.GetPersonnel();
        }

        public List<Person> FindPersonnel(string fio, string job, string enterprise, string phone)
        {
            return
                Personnel.Where(
                    person =>
                        person.Surname.ToLower().Contains(fio) && person.Job.ToLower().Contains(job) &&
                        person.Entretprise.ToLower().Contains(enterprise) && person.LandlineNumber.Contains(phone)).ToList();
        }

        public Person FindPersonForId(int id)
        {
            return Personnel.First(person => person.Id == id);
        }
    }
}
