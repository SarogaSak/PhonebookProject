using System.Collections.Generic;
using System.Linq;
using Phonebook.BusinessLogic;
using Phonebook.Models;

namespace Phonebook.CollectionModels
{
    /// <summary>
    /// Коллекция должностей.
    /// </summary>
    public class CollectionJobs
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
            return Jobs.First(jobs => jobs.Name.Equals(name)).Id;
        }

        public Job GetJobById(int id)
        {
            return Jobs.First(jobs => jobs.Id == id);
        }

        /// <summary>
        /// Обновляет значения в базе значениями из коллекции.
        /// </summary>
        public void Update(CollectionJobs oldCollection)
        {
            foreach (var job in Jobs)
            {
                if (job.Id == 0)
                {
                    blJob.InsertData(job);
                }
                else
                {
                    if (!oldCollection.GetJobById(job.Id).Equals(job))
                    {
                        blJob.UpdateData(job);
                    }
                }
            }
        }

        /// <summary>
        /// Вставляет новую запись в таблицу Jobs.
        /// </summary>
        public void InsertNew(Job newJob)
        {
            blJob.InsertData(newJob);
        }

        /// <summary>
        /// Удаляет запись из таблицы Jobs с указанным Id.
        /// </summary>
        /// <param name="id">Id записи для удаления.</param>
        public void DeleteById(int id)
        {
            blJob.DeleteData(id);
            Jobs.Remove(Jobs.First(jobs => jobs.Id == id));
        }

        /// <summary>
        /// Сортирует должности по коду сортировки.
        /// </summary>
        public List<Job> SortedList()
        {
            return Jobs.OrderBy(job => job.SortOrder).ToList();
        }

    }
}
