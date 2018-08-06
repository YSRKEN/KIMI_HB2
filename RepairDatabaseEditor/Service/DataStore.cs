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
        /// 艦娘のID一覧(重複検索用)
        /// </summary>
        private IDictionary<int, int> kammusuIdDic = new Dictionary<int, int>();

        /// <summary>
        /// 装備のID一覧(重複検索用)
        /// </summary>
        private IDictionary<int, int> weaponIdDic = new Dictionary<int, int>();

        /// <summary>
        /// 装備のID一覧(基本情報の重複検索用)
        /// </summary>
        private IDictionary<int, int> weaponIdDic2 = new Dictionary<int, int>();

        /// <summary>
        /// 艦娘のデータを保存
        /// </summary>
        private void SaveKammusuList()
        {
            string jsonText = JsonConvert.SerializeObject(KammusuList, Formatting.Indented);
            using(var sw = new StreamWriter(@"DB/kammusu_list.json", false, Encoding.UTF8))
            {
                sw.Write(jsonText);
            }
        }

        /// <summary>
        /// 装備のデータを保存
        /// </summary>
        private void SaveWeaponList()
        {
            string jsonText = JsonConvert.SerializeObject(WeaponList, Formatting.Indented);
            using (var sw = new StreamWriter(@"DB/weapon_list.json", false, Encoding.UTF8))
            {
                sw.Write(jsonText);
            }
        }

        /// <summary>
        /// 改修の基本情報のデータを保存
        /// </summary>
        private void SaveBasicInfoList()
        {
            string jsonText = JsonConvert.SerializeObject(BasicInfoList, Formatting.Indented);
            using (var sw = new StreamWriter(@"DB/basicinfo_list.json", false, Encoding.UTF8))
            {
                sw.Write(jsonText);
            }
        }

        /// <summary>
        /// 艦娘のデータ
        /// </summary>
        public ObservableCollection<Kammusu> KammusuList { get; } = new ObservableCollection<Kammusu>();

        /// <summary>
        /// 装備のデータ
        /// </summary>
        public ObservableCollection<Weapon> WeaponList { get; } = new ObservableCollection<Weapon>();

        /// <summary>
        /// 改修の基本情報のデータ
        /// </summary>
        public ObservableCollection<RepairBasicInfoForPreview> BasicInfoList { get; } = new ObservableCollection<RepairBasicInfoForPreview>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataStore()
        {
            // 艦娘データを読み込み
            try
            {
                using (var sr = new StreamReader(@"DB/kammusu_list.json", Encoding.UTF8))
                {
                    string jsonText = sr.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<IList<Kammusu>>(jsonText);
                    int i = 0;
                    foreach (Kammusu temp in list)
                    {
                        KammusuList.Add(temp);
                        kammusuIdDic.Add(temp.Id, i);
                        ++i;
                    }
                }
            }
            catch(IOException e)
            {
                KammusuList.Clear();
                kammusuIdDic.Clear();
                Console.WriteLine(e);
            }

            // 装備データを読み込み
            try
            {
                using (var sr = new StreamReader(@"DB/weapon_list.json", Encoding.UTF8))
                {
                    string jsonText = sr.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<IList<Weapon>>(jsonText);
                    int i = 0;
                    foreach (Weapon temp in list)
                    {
                        WeaponList.Add(temp);
                        weaponIdDic.Add(temp.Id, i);
                        ++i;
                    }
                }
            }
            catch(IOException e)
            {
                WeaponList.Clear();
                weaponIdDic.Clear();
                Console.WriteLine(e);
            }

            // 改修の基本情報データを読み込み
            try
            {
                using (var sr = new StreamReader(@"DB/basicinfo_list.json", Encoding.UTF8))
                {
                    string jsonText = sr.ReadToEnd();
                    var list = JsonConvert.DeserializeObject<IList<RepairBasicInfo>>(jsonText);
                    int i = 0;
                    foreach (RepairBasicInfo temp in list)
                    {
                        if (!weaponIdDic.ContainsKey(temp.Id))
                        {
                            continue;
                        }
                        string name = WeaponList[weaponIdDic[temp.Id]].Name;
                        var temp2 = new RepairBasicInfoForPreview(temp, name);
                        BasicInfoList.Add(temp2);
                        weaponIdDic2.Add(temp2.Id, i);
                        ++i;
                    }
                }
            }
            catch(Exception e)
            {
                BasicInfoList.Clear();
                weaponIdDic2.Clear();
                Console.WriteLine(e);
            }
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
            KammusuList.Add(new Kammusu() { Id = id, Name = name });
            kammusuIdDic.Add(id, KammusuList.Count - 1);
            SaveKammusuList();
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
            KammusuList[index] = temp;
            kammusuIdDic.Remove(oldId);
            kammusuIdDic.Add(id, index);
            SaveKammusuList();
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
            KammusuList.RemoveAt(index);
            kammusuIdDic.Remove(oldId);
            for (int i = index; i < KammusuList.Count; ++i)
            {
                int id = KammusuList[i].Id;
                --kammusuIdDic[id];
            }
            SaveKammusuList();
            return true;
        }

        /// <summary>
        /// 装備のデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Weapon GetWeapon(int id)
        {
            if (weaponIdDic.ContainsKey(id))
            {
                return WeaponList[weaponIdDic[id]];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 装備のデータを追加する
        /// </summary>
        /// <param name="id">装備ID</param>
        /// <param name="name">装備名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PostWeapon(int id, string name)
        {
            if (weaponIdDic.ContainsKey(id))
            {
                return false;
            }
            WeaponList.Add(new Weapon() { Id = id, Name = name });
            weaponIdDic.Add(id, WeaponList.Count - 1);
            SaveWeaponList();
            return true;
        }

        /// <summary>
        /// 装備のデータを更新する
        /// </summary>
        /// <param name="id">装備ID</param>
        /// <param name="name">装備名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PutWeapon(int id, string name, int oldId)
        {
            if (id != oldId && weaponIdDic.ContainsKey(id))
            {
                return false;
            }
            var temp = new Weapon() { Id = id, Name = name };
            int index = weaponIdDic[oldId];
            WeaponList[index] = temp;
            weaponIdDic.Remove(oldId);
            weaponIdDic.Add(id, index);
            SaveWeaponList();
            return true;
        }

        /// <summary>
        /// 装備のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteWeapon(int oldId)
        {

            if (!weaponIdDic.ContainsKey(oldId))
            {
                return false;
            }
            int index = weaponIdDic[oldId];
            WeaponList.RemoveAt(index);
            weaponIdDic.Remove(oldId);
            for (int i = index; i < WeaponList.Count; ++i)
            {
                int id = WeaponList[i].Id;
                --weaponIdDic[id];
            }
            SaveWeaponList();
            return true;
        }

        /// <summary>
        /// 改修の基本情報のデータを追加する
        /// </summary>
        /// <param name="id">装備ID</param>
        /// <param name="name">装備名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PostWeaponBasicInfo(int id, int fuel, int ammo, int steel, int bauxite)
        {
            if (weaponIdDic2.ContainsKey(id))
            {
                return false;
            }
            BasicInfoList.Add(new RepairBasicInfoForPreview() {
                Id = id,
                Name = WeaponList[weaponIdDic[id]].Name,
                Fuel = fuel,
                Ammo = ammo,
                Steel = steel,
                Bauxite = bauxite
            });
            weaponIdDic2.Add(id, BasicInfoList.Count - 1);
            SaveBasicInfoList();
            return true;
        }

        /// <summary>
        /// 改修の基本情報のデータを更新する
        /// </summary>
        /// <param name="id">装備ID</param>
        /// <param name="name">装備名</param>
        /// <returns>更新できたならtrue</returns>
        public bool PutWeaponBasicInfo(int id, int fuel, int ammo, int steel, int bauxite, int oldId)
        {
            if (id != oldId && weaponIdDic2.ContainsKey(id))
            {
                return false;
            }
            var temp = new RepairBasicInfoForPreview()
            {
                Id = id,
                Name = WeaponList[weaponIdDic[id]].Name,
                Fuel = fuel,
                Ammo = ammo,
                Steel = steel,
                Bauxite = bauxite
            };
            int index = weaponIdDic2[oldId];
            BasicInfoList[index] = temp;
            weaponIdDic2.Remove(oldId);
            weaponIdDic2.Add(id, index);
            SaveBasicInfoList();
            return true;
        }

        /// <summary>
        /// 改修の基本情報のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteWeaponBasicInfo(int oldId)
        {

            if (!weaponIdDic2.ContainsKey(oldId))
            {
                return false;
            }
            int index = weaponIdDic2[oldId];
            BasicInfoList.RemoveAt(index);
            weaponIdDic2.Remove(oldId);
            for (int i = index; i < BasicInfoList.Count; ++i)
            {
                int id = BasicInfoList[i].Id;
                --weaponIdDic2[id];
            }
            SaveBasicInfoList();
            return true;
        }
    }
}
