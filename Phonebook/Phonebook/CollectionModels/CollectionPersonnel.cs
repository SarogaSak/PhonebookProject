using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public List<Person> FindPersonnel(string fio, string jobName, string enterpriseName, string deptName, string phone)
        {
            return
                Personnel.Where(
                    person =>
                        person.Surname.ToLower().Contains(fio) && 
                        person.JobName.ToLower().Contains(jobName) &&
                        person.EnterpriseName.ToLower().Contains(enterpriseName) &&
                        person.DeptName.ToLower().Contains(deptName) && 
                        person.LandlineNumbers.Contains(phone)).ToList();
        }

        public List<Person> FindPersonnel(List<Dept> depts)
        {
            List<Person> persons = new List<Person>();
            foreach (var dept in depts)
            {
                List<Person> temp = Personnel.Where(person => person.IdDept==dept.Id).ToList();
                persons.AddRange(temp);
            }
            return persons;
        }

        public Person FindPersonForId(int id)
        {
            return Personnel.First(person => person.Id == id);
        }

        public IEnumerable<ListItem> ConvertToListItems()
        {
            return Personnel.Select(person => new ListItem(person));
        }
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string JobName { get; set; }
        public string DeptName { get; set; }
        public string EnterpriseName { get; set; }
        public string Numbers { get; set; }

        public ListItem(Person person)
        {
            Id = person.Id;
            FIO = person.Surname + ", " + person.Name + " " + person.SecondName;
            JobName = SplitString(person.JobName, 50);
            DeptName = person.DeptName;//SplitString(person.DeptName, 50);
            EnterpriseName = SplitString(person.EnterpriseName, 100);
            Numbers = person.LandlineNumbers;
        }

        private static string SplitString(string str, int strLength)
        {
            var temp = str.Split(' ');
            StringBuilder result = new StringBuilder("");
            int lineCount = 1;
            foreach (var s in temp)
            {
                if (result.Length + s.Length > lineCount*strLength)
                {
                    result.Append(Environment.NewLine);
                    lineCount++;
                }
                result.Append(s);
                result.Append(" ");
            }
            return result.ToString();
        }
    }
}
