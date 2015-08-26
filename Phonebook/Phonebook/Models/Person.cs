namespace Phonebook.Models
{
    public class Person
    {
        public int Id { get; private set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Job { get; set; }
        public string Entretprise { get; set; }
        public string CellNumber { get; set; }
        public string LandlineNumber { get; set; }
        public string InternalNumber { get; set; }
        public string Photo { get; set; }

        public Person(int id, string surname, string name, string secondName, string entretprise, string job, string landlineNumber, string cellNumber, string internalNumber, string photo)
        {
            Id = id;
            Surname = surname;
            Name = name;
            SecondName = secondName;
            Entretprise = entretprise;
            Job = job;
            LandlineNumber = landlineNumber;
            CellNumber = cellNumber;
            InternalNumber = internalNumber;
            Photo = photo;
        }

        public Person()
        {
            Id = 0;
            Surname = "";
            Name = "";
            SecondName = "";
            Entretprise = "";
            Job = "";
            LandlineNumber = "";
            CellNumber = "";
            InternalNumber = "";
            Photo = "";
        }
    }
}
