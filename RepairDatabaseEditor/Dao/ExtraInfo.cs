namespace RepairDatabaseEditor.Model
{
    /// <summary>
    /// 改修の拡張情報
    /// </summary>
    class ExtraInfo
    {
        /// <summary>
        /// 装備ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 改修段階
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 改修後の装備ID
        /// </summary>
        public int NextId { get; set; }

        /// <summary>
        /// 開発資材(通常)
        /// </summary>
        public int GearProb { get; set; }

        /// <summary>
        /// 開発資材(確定)
        /// </summary>
        public int GearSure { get; set; }

        /// <summary>
        /// 改修資材(通常)
        /// </summary>
        public int ScrewProb { get; set; }

        /// <summary>
        /// 改修資材(確定)
        /// </summary>
        public int ScrewSure { get; set; }

        /// <summary>
        /// 消費装備ID
        /// </summary>
        public int LostId { get; set; }

        /// <summary>
        /// 消費装備数
        /// </summary>
        public int LostCount { get; set; }

        /// <summary>
        /// 装備名
        /// </summary>
        public string WeaponName { get; set; }

        /// <summary>
        /// 改修段階名
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 装備名
        /// </summary>
        public string NextWeaponName { get; set; }

        /// <summary>
        /// 改修段階名(表示用)
        /// </summary>
        public string StepName2 {
            get {
                return (Step == 0 ? "★0～★5" : Step == 6 ? "★6～★9" : "★max");
            }
        }

        /// <summary>
        /// 表示用
        /// </summary>
        public string ListName {
            get {
                string nextWeaponName = (NextId == 0 ? "" : $"⇒{NextWeaponName}");
                return $"{WeaponName}({StepName2}){nextWeaponName}";
            }
        }
    }
}
