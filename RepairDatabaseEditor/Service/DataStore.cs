using Newtonsoft.Json;
using Reactive.Bindings;
using RepairDatabaseEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// 艦娘のデータ(RAW)
        /// </summary>
        private ObservableCollection<Kammusu> kammusuList { get; } = new ObservableCollection<Kammusu>();

        /// <summary>
        /// 艦娘のID一覧(重複検索用)
        /// </summary>
        private IDictionary<int, int> kammusuIdDic = new Dictionary<int, int>();

        /// <summary>
        /// 艦娘のデータ
        /// </summary>
        public ReadOnlyReactiveCollection<Kammusu> SortedKammusuList { get; }

        /// <summary>
        /// 装備のデータ(RAW)
        /// </summary>
        private ObservableCollection<Weapon> weaponList { get; } = new ObservableCollection<Weapon>();

        /// <summary>
        /// 装備のID一覧(重複検索用)
        /// </summary>
        private IDictionary<int, int> weaponIdDic = new Dictionary<int, int>();

        /// <summary>
        /// 装備のデータ
        /// </summary>
        public ReadOnlyReactiveCollection<Weapon> SortedWeaponList { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataStore()
        {
            // 艦娘データを読み込み
            using(var sr = new StreamReader(@"DB/kammusu_list.json", Encoding.UTF8))
            {
                string jsonText = sr.ReadToEnd();
                var list = JsonConvert.DeserializeObject<IList<Kammusu>>(jsonText);
                int i = 0;
                foreach(Kammusu temp in list)
                {
                    kammusuList.Add(temp);
                    kammusuIdDic.Add(temp.Id, i);
                    ++i;
                }
            }

            // 装備データを読み込み
            using (var sr = new StreamReader(@"DB/weapon_list.json", Encoding.UTF8))
            {
                string jsonText = sr.ReadToEnd();
                var list = JsonConvert.DeserializeObject<IList<Weapon>>(jsonText);
                int i = 0;
                foreach (Weapon temp in list)
                {
                    weaponList.Add(temp);
                    weaponIdDic.Add(temp.Id, i);
                    ++i;
                }
            }

            // その他変数を設定
            SortedKammusuList = (new ObservableCollection<Kammusu>(kammusuList.OrderBy(k => k.Id))).ToReadOnlyReactiveCollection();
            SortedWeaponList = (new ObservableCollection<Weapon>(weaponList.OrderBy(k => k.Id))).ToReadOnlyReactiveCollection();
        }

        /// <summary>
        /// 艦娘のデータを追加する
        /// </summary>
        /// <param name="id">艦船ID</param>
        /// <param name="name">艦名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PostKammusu(int id, string name)
        {
            if (kammusuIdDic.ContainsKey(id))
            {
                return false;
            }
            kammusuList.Add(new Kammusu() { Id = id, Name = name });
            kammusuIdDic.Add(id, kammusuList.Count - 1);
            return true;
        }

        /// <summary>
        /// 艦娘のデータを更新する
        /// </summary>
        /// <param name="id">艦船ID</param>
        /// <param name="name">艦名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PutKammusu(int id, string name, int oldId)
        {
            if(id != oldId && kammusuIdDic.ContainsKey(id))
            {
                return false;
            }
            var temp = new Kammusu() { Id = id, Name = name };
            int index = kammusuIdDic[oldId];
            kammusuList[index] = temp;
            kammusuIdDic.Remove(oldId);
            kammusuIdDic.Add(id, index);
            return true;
        }

        /// <summary>
        /// 艦娘のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteKammusu(int oldId)
        {

            if (!kammusuIdDic.ContainsKey(oldId))
            {
                return false;
            }
            int index = kammusuIdDic[oldId];
            kammusuList.RemoveAt(index);
            kammusuIdDic.Remove(oldId);
            for (int i = index; i < kammusuList.Count; ++i)
            {
                int id = kammusuList[i].Id;
                --kammusuIdDic[id];
            }
            return true;
        }
    }
}
