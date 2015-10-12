namespace Phonebook.Models
{
    /// <summary>
    /// Сотрудники.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// ID должности.
        /// </summary>
        public int IdJob { get; set; }

        /// <summary>
        /// Название должности.
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// ID  отдела.
        /// </summary>
        public int IdDept { get; set; }

        /// <summary>
        /// Название отдела/управления.
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// Мобильные номера, хранятся через разделитель *.
        /// </summary>
        public string CellNumbers { get; set; }

        /// <summary>
        /// Городские номера, хранятся через разделитель *.
        /// </summary>
        public string LandlineNumbers { get; set; }

        /// <summary>
        /// Вертушка, хранятся через разделитель *.
        /// </summary>
        public string InternalNumbers { get; set; }

        /// <summary>
        /// Имя файла с  фото.
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Почта, хранятся через разделитель *
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="surname">Фамилия.</param>
        /// <param name="name">Имя.</param>
        /// <param name="secondName">Отчество.</param>
        /// <param name="idJob">ID должности.</param>
        /// <param name="jobName">Название должности.</param>
        /// <param name="idDept">ID  отдела.</param>
        /// <param name="deptName">Название отдела/управления.</param>
        /// <param name="cellNumbers">Мобильные номера, хранятся через разделитель *.</param>
        /// <param name="landlineNumbers">Городские номера, хранятся через разделитель *.</param>
        /// <param name="internalNumbers">Вертушка, хранятся через разделитель *.</param>
        /// <param name="photo">Имя файла с  фото.</param>
        /// <param name="email">Почта, хранятся через разделитель *</param>
        public Person(int id, string surname, string name, string secondName, int idJob, string jobName, int idDept,
            string deptName, string cellNumbers, string landlineNumbers, string internalNumbers, string photo,
            string email)
        {
            Id = id;
            Surname = surname;
            Name = name;
            SecondName = secondName;
            IdJob = idJob;
            JobName = jobName;
            IdDept = idDept;
            DeptName = deptName;
            CellNumbers = cellNumbers;
            LandlineNumbers = landlineNumbers;
            InternalNumbers = internalNumbers;
            Photo = photo;
            Email = email;
        }

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public Person()
        {
            Id = 0;
            Surname = "";
            Name = "";
            SecondName = "";
            IdJob = 0;
            JobName = "";
            IdDept = 0;
            DeptName = "";
            CellNumbers = "";
            LandlineNumbers = "";
            InternalNumbers = "";
            Photo = "";
            Email = "";
        }
    }
}
