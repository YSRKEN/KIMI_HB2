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
        /// <summary>
        /// 艦娘のデータ
        /// </summary>
        public List<Kammusu> Kammusus { get; set; } = new List<Kammusu>();

        /// <summary>
        /// 装備のデータ
        /// </summary>
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

        /// <summary>
        /// 艦娘のデータを追加する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void PostKammusu(int id, string name)
        {
            Kammusus.Add(new Kammusu() { Id = id, Name = name });
            Kammusus.Sort((a, b) => a.Id > b.Id ? 1 : a.Id < b.Id ? -1 : 0);
        }

        /// <summary>
        /// 艦娘のデータを更新する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void PutKammusu(int id, string name, int oldId)
        {
            var temp = new Kammusu() { Id = id, Name = name };
            int index = Kammusus.FindIndex(k => k.Id == oldId);
            Kammusus[index] = temp;
            Kammusus.Sort((a, b) => a.Id > b.Id ? 1 : a.Id < b.Id ? -1 : 0);
        }

        /// <summary>
        /// 艦娘のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public void DeleteKammusu(int oldId)
        {
            int index = Kammusus.FindIndex(k => k.Id == oldId);
            Kammusus.RemoveAt(index);
        }
    }
}
