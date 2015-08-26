namespace Phonebook.Models
{
    public class Enterprise
    {
        public int Id { get; private set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Enterprise( int id,string address, string name)
        {
            Address = address;
            Name = name;
            Id = id;
        }
    }
}
