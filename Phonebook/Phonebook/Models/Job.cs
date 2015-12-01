namespace Phonebook.Models
{
    /// <summary>
    /// Должности.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// Название должности.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Порядок сортировки.
        /// </summary>
        public int SortOrder { get; set; }
        
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Название должности</param>
        /// <param name="sortOrder">Порядок сортировки</param>
        public Job(int id, string name, int sortOrder)
        {
            Id = id;
            Name = name;
            SortOrder = sortOrder;
        }

        public Job()
        {
            Id = 0;
            Name = "";
            SortOrder = 99999;
        }

        /// <summary>
        /// Сравнивает переданный объект с текущим.
        /// </summary>
        public bool Equals(Job job)
        {
            return (Name.Equals(job.Name) && SortOrder == job.SortOrder);
        }
    }
}
