using Newtonsoft.Json;
using RepairDatabaseEditor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairDatabaseEditor.Service
{
    /// <summary>
    /// データベース
    /// </summary>
    class DataStore
    {
        public List<Kammusu> Kammusus { get; } = new List<Kammusu>();
        public List<Weapon> Weapons { get; } = new List<Weapon>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataStore()
        {
            using(var sr = new StreamReader(@"DB/kammusu_list.json", Encoding.UTF8))
            {
                string jsonText = sr.ReadToEnd();
                Kammusus = JsonConvert.DeserializeObject<List<Kammusu>>(jsonText);
                Kammusus.Sort((a, b) => a.Id > b.Id ? 1 : a.Id < b.Id ? -1 : 0);
            }
            using (var sr = new StreamReader(@"DB/weapon_list.json", Encoding.UTF8))
            {
                string jsonText = sr.ReadToEnd();
                Weapons = JsonConvert.DeserializeObject<List<Weapon>>(jsonText);
                Weapons.Sort((a, b) => a.Id > b.Id ? 1 : a.Id < b.Id ? -1 : 0);
            }
        }
    }
}
