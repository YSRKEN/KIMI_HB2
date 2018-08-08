using Newtonsoft.Json;
using Reactive.Bindings;
using RepairDatabaseEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
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
        /// 改修情報を取り扱うためのデータベース
        /// </summary>
        private SQLiteConnectionStringBuilder sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = @"DB\repair_db.db" };

        /// <summary>
        /// KammusuListを更新する
        /// </summary>
        private void RefreshKammusuList()
        {
            KammusuList.Clear();
            var list = ExecuteSelectReader("SELECT id, name FROM kammusu ORDER BY id");
            foreach(var pair in list)
            {
                long id = (long)(pair["id"]);
                string name = (string)(pair["name"]);
                var temp = new Kammusu() { Id = (int)id, Name = name };
                KammusuList.Add(temp);
            }
            return;
        }

        /// <summary>
        /// WeaponListを更新する
        /// </summary>
        private void RefreshWeaponList()
        {
            WeaponList.Clear();
            var list = ExecuteSelectReader("SELECT id, name FROM weapon ORDER BY id");
            foreach (var pair in list)
            {
                long id = (long)(pair["id"]);
                string name = (string)(pair["name"]);
                var temp = new Weapon() { Id = (int)id, Name = name };
                WeaponList.Add(temp);
            }
            return;
        }

        /// <summary>
        /// BasicInfoListを更新する
        /// </summary>
        private void RefreshBasicInfoList()
        {
            BasicInfoList.Clear();
            var list = ExecuteSelectReader($@"SELECT basic_info.id, basic_info.fuel, basic_info.ammo, basic_info.steel,
                basic_info.bauxite, weapon.name FROM basic_info, weapon WHERE basic_info.id = weapon.id ORDER BY basic_info.id");
            foreach (var pair in list)
            {
                long id = (long)(pair["id"]);
                long fuel = (long)(pair["fuel"]);
                long ammo = (long)(pair["ammo"]);
                long steel = (long)(pair["steel"]);
                long bauxite = (long)(pair["bauxite"]);
                string name = (string)(pair["name"]);
                var temp = new BasicInfo() { Id = (int)id, Fuel = (int)fuel, Ammo = (int)ammo,
                    Steel = (int)steel, Bauxite = (int)bauxite, Name = name };
                BasicInfoList.Add(temp);
            }
            return;
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
        public ObservableCollection<BasicInfo> BasicInfoList { get; } = new ObservableCollection<BasicInfo>();

        /// <summary>
        /// 結果を返さなくてもいいSQLを処理する
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteNonQuery(string query)
        {
            using (var cn = new SQLiteConnection(sqlConnectionSb.ToString()))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// SELECT SQLを処理する
        /// </summary>
        /// <param name="query"></param>
        public List<Dictionary<String, object>> ExecuteSelectReader(string query)
        {
            var result = new List<Dictionary<String, object>>();
            using (var cn = new SQLiteConnection(sqlConnectionSb.ToString()))
            {
                cn.Open();
                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = query;
                    using (var reader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            var temp = new Dictionary<String, object>();
                            for (int j = 0; j < reader.FieldCount; ++j)
                            {
                                temp[reader.GetValues().AllKeys[j]] = reader.GetValue(j);
                            }
                            result.Add(temp);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataStore()
        {
            // テーブルが存在しない場合は作成し直す
            ExecuteNonQuery($@"CREATE TABLE IF NOT EXISTS kammusu (
                id INTEGER NOT NULL UNIQUE,
                name TEXT NOT NULL,
                PRIMARY KEY(id))");
            ExecuteNonQuery($@"CREATE TABLE IF NOT EXISTS weapon (
                id INTEGER NOT NULL UNIQUE,
                name TEXT NOT NULL,
                PRIMARY KEY(id))");
            ExecuteNonQuery($@"CREATE TABLE IF NOT EXISTS basic_info (
                id INTEGER NOT NULL UNIQUE REFERENCES weapon(id),
                fuel INTEGER NOT NULL,
                ammo INTEGER NOT NULL,
                steel INTEGER NOT NULL,
                bauxite INTEGER NOT NULL,
                PRIMARY KEY(id))");

            // 表示内容を更新する
            RefreshKammusuList();
            RefreshWeaponList();
            RefreshBasicInfoList();
        }

        /// <summary>
        /// 艦娘のデータを追加する
        /// </summary>
        /// <param name="id">艦船ID</param>
        /// <param name="name">艦名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PostKammusu(int id, string name)
        {
            /*if (kammusuIdDic.ContainsKey(id))
            {
                return false;
            }
            KammusuList.Add(new Kammusu() { Id = id, Name = name });
            kammusuIdDic.Add(id, KammusuList.Count - 1);
            RefreshKammusuList();*/
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
            /*if(id != oldId && kammusuIdDic.ContainsKey(id))
            {
                return false;
            }
            var temp = new Kammusu() { Id = id, Name = name };
            int index = kammusuIdDic[oldId];
            KammusuList[index] = temp;
            kammusuIdDic.Remove(oldId);
            kammusuIdDic.Add(id, index);
            SaveKammusuList();*/
            return true;
        }

        /// <summary>
        /// 艦娘のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteKammusu(int oldId)
        {

            /*if (!kammusuIdDic.ContainsKey(oldId))
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
            SaveKammusuList();*/
            return true;
        }

        /// <summary>
        /// 装備のデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Weapon GetWeapon(int id)
        {
            /*if (weaponIdDic.ContainsKey(id))
            {
                return WeaponList[weaponIdDic[id]];
            }
            else
            {
                return null;
            }*/
            return null;
        }

        /// <summary>
        /// 装備のデータを追加する
        /// </summary>
        /// <param name="id">装備ID</param>
        /// <param name="name">装備名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PostWeapon(int id, string name)
        {
            /*if (weaponIdDic.ContainsKey(id))
            {
                return false;
            }
            WeaponList.Add(new Weapon() { Id = id, Name = name });
            weaponIdDic.Add(id, WeaponList.Count - 1);
            SaveWeaponList();*/
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
            /*if (id != oldId && weaponIdDic.ContainsKey(id))
            {
                return false;
            }
            var temp = new Weapon() { Id = id, Name = name };
            int index = weaponIdDic[oldId];
            WeaponList[index] = temp;
            weaponIdDic.Remove(oldId);
            weaponIdDic.Add(id, index);
            SaveWeaponList();*/
            return true;
        }

        /// <summary>
        /// 装備のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteWeapon(int oldId)
        {

            /*if (!weaponIdDic.ContainsKey(oldId))
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
            SaveWeaponList();*/
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
            /*if (weaponIdDic2.ContainsKey(id))
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
            SaveBasicInfoList();*/
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
            /*if (id != oldId && weaponIdDic2.ContainsKey(id))
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
            SaveBasicInfoList();*/
            return true;
        }

        /// <summary>
        /// 改修の基本情報のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteWeaponBasicInfo(int oldId)
        {

            /*if (!weaponIdDic2.ContainsKey(oldId))
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
            SaveBasicInfoList();*/
            return true;
        }
    }
}
