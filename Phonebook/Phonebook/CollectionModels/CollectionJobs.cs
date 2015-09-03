using System.Collections.Generic;
using System.Linq;
using Phonebook.Helpers;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    class CollectionJobs
    {
        public List<Job> Jobs { get; set; }

        public CollectionJobs(List<Job> jobs)
        {
            Jobs = jobs;
        }

        public CollectionJobs()
        {
            Jobs = AccessHelper.GetJobs();
        }
        /// <summary>
        /// Ищет должность по подстроке названия.
        /// </summary>
        /// <param name="mask">Подстрока названия</param>
        public List<Job> FindJobsForMask(string mask)
        {
            return Jobs.Where(job => job.JobName.ToLower().Contains(mask)).ToList();
        }
        /// <summary>
        /// Возвращает Id должности по названию.
        /// </summary>
        /// <param name="name">Название должности.</param>
        public int GetIdByName(string name)
        {
            return Jobs.First(jobs => jobs.JobName.Contains(name)).Id;
        }

        public void Update()
        {
            foreach (Job job in Jobs)
            {
                AccessHelper.UpdateJob(job);
            }
        }

        public int InsertNew()
        {
            int newItemId = AccessHelper.InsertNewJob();
            Jobs.Add(new Job(newItemId, "",9999));
            return newItemId;
        }

        public void DeleteById(int id)
        {
            AccessHelper.DeleteJob(id);
            Jobs.Remove(Jobs.First(jobs => jobs.Id == id));
        }

        public List<Job> SortedList()
        {
            return Jobs.OrderBy(job => job.SortOrder).ToList();
        }
    }
}
