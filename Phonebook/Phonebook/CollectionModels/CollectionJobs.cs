using System.Collections.Generic;
using System.Linq;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    class CollectionJobs
    {
        /// <summary>
        /// Объект для работы с базой. Таблица Jobs.
        /// </summary>
        readonly AbstractBLModels blJob = new BLJob();

        public List<Job> Jobs { get; set; }

        public CollectionJobs(List<Job> jobs)
        {
            Jobs = jobs;
        }

        public CollectionJobs()
        {
            Jobs = blJob.GetListData<List<Job>>();
        }
        /// <summary>
        /// Возвращает список должностей с указанной подстрокой в названии.
        /// </summary>
        /// <param name="mask">Подстрока названия.</param>
        public List<Job> FindJobsForMask(string mask)
        {
            return Jobs.Where(job => job.Name.ToLower().Contains(mask)).ToList();
        }
        /// <summary>
        /// Возвращает Id должности по названию.
        /// </summary>
        /// <param name="name">Название должности.</param>
        public int GetIdByName(string name)
        {
            return Jobs.First(jobs => jobs.Name.Contains(name)).Id;
        }

        public void Update()
        {
            foreach (Job job in Jobs)
            {
                blJob.UpdateData(job);
            }
        }

        public void InsertNew()
        {
            //int newItemId = AccessHelper.InsertNewJob();
            //Jobs.Add(new Job(newItemId, "",9999));
            //return newItemId;
        }

        public void DeleteById(int id)
        {
            blJob.DeleteData(id);
            Jobs.Remove(Jobs.First(jobs => jobs.Id == id));
        }

        public List<Job> SortedList()
        {
            return Jobs.OrderBy(job => job.SortOrder).ToList();
        }
    }
}
