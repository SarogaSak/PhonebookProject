namespace Phonebook.Models
{
    /// <summary>
    /// Предприятие.
    /// </summary>
    public class Enterprise
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// Название предприятия.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Адрес предриятия.
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Порядок сортировки.
        /// </summary>
        public int SortOrder { get; set; }
        
        /// <summary>
        /// ID куратора.
        /// </summary>
        public int IdCurator { get; set; }
        
        /// <summary>
        /// ФИО куратора.
        /// </summary>
        public string CuratorFio { get; set; }
        
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="name">Название предприятия.</param>
        /// <param name="address">Адрес предриятия.</param>
        /// <param name="sortOrder">Порядок сортировки.</param>
        /// <param name="idCurator">ID куратора.</param>
        /// <param name="curatorFio">ФИО куратора.</param>
        public Enterprise(int id, string name, string address, int sortOrder, int idCurator, string curatorFio)
        {
            Id = id;
            Name = name;
            Address = address;
            SortOrder = sortOrder;
            IdCurator = idCurator;
            CuratorFio = curatorFio;
        }

        public Enterprise()
        {
            Id = 0;
            Name = "";
            Address = "";
            SortOrder = 99999;
            IdCurator = 99999;
        }

        /// <summary>
        /// Сравнивает объект с текущим.
        /// </summary>
        /// <param name="enterprise">Объект для сравнения.</param>
        public bool Equals(Enterprise enterprise)
        {
            return (Name.Equals(enterprise.Name) &&
                    Address.Equals(enterprise.Address) &&
                    SortOrder == enterprise.SortOrder &&
                    IdCurator == enterprise.IdCurator);
        }
    }
}
