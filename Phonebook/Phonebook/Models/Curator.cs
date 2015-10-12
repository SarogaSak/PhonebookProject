namespace Phonebook.Models
{
    /// <summary>
    /// Кураторы.
    /// </summary>
    class Curator
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// ФИО куратора
        /// </summary>
        public string FIO { get; set; }
        
        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// конструктор с параметрами.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="fio">ФИО куратора</param>
        /// <param name="sortOrder">Порядок сортировки</param>
        public Curator(int id, string fio, int sortOrder)
        {
            Id = id;
            FIO = fio;
            SortOrder = sortOrder;
        }
    }
}
