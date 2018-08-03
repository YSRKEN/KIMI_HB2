namespace RepairDatabaseEditor.Model
{
    /// <summary>
    /// 艦娘の情報
    /// </summary>
    public class Kammusu
    {
        /// <summary>
        /// 艦船ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 艦名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表示用
        /// </summary>
        public string ListName { get => Id.ToString() + " : " + Name; }
    }
}
