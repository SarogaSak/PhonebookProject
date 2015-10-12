namespace Phonebook.Models
{
    /// <summary>
    /// Управление/отдел/сектор.
    /// </summary>
    class Dept
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// Название управления/отдела.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Адрес управления/отдела.
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Порядок сортировки.
        /// </summary>
        public int SortOrder { get; set; }
        
        /// <summary>
        /// ID родительского предприятия.
        /// </summary>
        public int IdEnterprise { get; set; }
        
        /// <summary>
        /// Название родительского предприятия.
        /// </summary>
        public string EnterpriseName { get; set; }
        
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
        /// <param name="name">Название управления/отдела.</param>
        /// <param name="address">Адрес управления/отдела.</param>
        /// <param name="sortOrder">Порядок сортировки.</param>
        /// <param name="idEnterprise">ID родительского предприятия.</param>
        /// <param name="enterpriseName">Название родительского предприятия.</param>
        /// <param name="curatorFio">ID куратора.</param>
        /// <param name="idCurator">ФИО куратора.</param>
        public Dept(int id, string name, string address, int sortOrder, int idEnterprise, string enterpriseName, int idCurator, string curatorFio)
        {
            Id = id;
            Name = name;
            Address = address;
            SortOrder = sortOrder;
            IdEnterprise = idEnterprise;
            EnterpriseName = enterpriseName;
            CuratorFio = curatorFio;
            IdCurator = idCurator;
        }
    }
}
