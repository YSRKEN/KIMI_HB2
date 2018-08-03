namespace RepairDatabaseEditor.Model
{
    /// <summary>
    /// 装備の情報
    /// </summary>
    public class Weapon
    {
        /// <summary>
        /// 装備ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 装備名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表示用
        /// </summary>
        public string ListName { get => Id.ToString() + " : " + Name; }
    }
}
