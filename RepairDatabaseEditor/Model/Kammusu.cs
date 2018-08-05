using Newtonsoft.Json;

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
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 艦名
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
