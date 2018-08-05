using Newtonsoft.Json;

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
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 装備名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 表示用
        /// </summary>
        [JsonIgnore]
        public string ListName { get => Id.ToString() + " : " + Name; }
    }
}
