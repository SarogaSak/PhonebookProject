namespace Phonebook.Models
{
    public class Job
    {
        public int Id { get; private set; }
        public string JobName { get; set; }
        public int SortOrder { get; set; }

        public Job(int id, string jobname, int sortorder)
        {
            Id = id;
            JobName = jobname;
            SortOrder = sortorder;
        }

    }
}
