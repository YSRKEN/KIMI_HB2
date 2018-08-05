using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairDatabaseEditor.Model
{
    /// <summary>
    /// 改修の基本情報
    /// </summary>
    class RepairBasicInfo
    {
        /// <summary>
        /// 装備ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 燃料消費
        /// </summary>
        [JsonProperty("fuel")]
        public int Fuel { get; set; }

        /// <summary>
        /// 弾薬消費
        /// </summary>
        [JsonProperty("ammo")]
        public int Ammo { get; set; }

        /// <summary>
        /// 鋼材消費
        /// </summary>
        [JsonProperty("steel")]
        public int Steel { get; set; }

        /// <summary>
        /// ボーキサイト消費
        /// </summary>
        [JsonProperty("bauxite")]
        public int Bauxite { get; set; }
    }

    class RepairBasicInfoForPreview : RepairBasicInfo
    {
        /// <summary>
        /// 装備名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表示用
        /// </summary>
        public string ListName { get => Name + " : " + Fuel + "," + Ammo + "," + Steel + "," + Bauxite; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rbi">RepairBasicInfo型の変数</param>
        /// <param name="name">装備名</param>
        public RepairBasicInfoForPreview(RepairBasicInfo rbi, string name)
        {
            this.Id = rbi.Id;
            this.Fuel = rbi.Fuel;
            this.Ammo = rbi.Ammo;
            this.Steel = rbi.Steel;
            this.Bauxite = rbi.Bauxite;
            this.Name = name;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RepairBasicInfoForPreview()
        {
        }
    }
}
