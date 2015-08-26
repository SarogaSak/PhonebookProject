namespace Phonebook.Models
{
    public class Job
    {
        public int Id { get; private set; }
        public string JobName { get; set; }

        public Job(int id, string jobname)
        {
            Id = id;
            JobName = jobname;
        }

    }
}
