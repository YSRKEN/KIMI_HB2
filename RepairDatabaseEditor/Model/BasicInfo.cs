namespace RepairDatabaseEditor.Model
{
    /// <summary>
    /// 改修の基本情報
    /// </summary>
    class BasicInfo
    {
        /// <summary>
        /// 装備ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 燃料消費
        /// </summary>
        public int Fuel { get; set; }

        /// <summary>
        /// 弾薬消費
        /// </summary>
        public int Ammo { get; set; }

        /// <summary>
        /// 鋼材消費
        /// </summary>
        public int Steel { get; set; }

        /// <summary>
        /// ボーキサイト消費
        /// </summary>
        public int Bauxite { get; set; }

        /// <summary>
        /// 装備名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表示用
        /// </summary>
        public string ListName { get => Name + " : " + Fuel + "," + Ammo + "," + Steel + "," + Bauxite; }
    }
}
