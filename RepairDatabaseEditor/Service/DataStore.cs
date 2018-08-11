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
        /// repair_stepテーブルを修正した
        /// </summary>
        private void CreateRepairStepList()
        {
            for(int i = 0; i <= 10; ++i)
            {
                string x = (i == 10 ? "max" : i.ToString());
                ExecuteNonQuery($"INSERT INTO repair_step(step, name) values({i}, '★{x}')");
            }
        }

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
        /// RepairStepListを更新する
        /// </summary>
        private void RefreshRepairStepList()
        {
            RepairStepList.Clear();
            var list = ExecuteSelectReader("SELECT step, name FROM repair_step ORDER BY step");
            foreach (var pair in list)
            {
                long step = (long)(pair["step"]);
                string name = (string)(pair["name"]);
                var temp = new RepairStep() { Step = (int)step, Name = name };
                RepairStepList.Add(temp);
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
        /// ExtraInfoListを更新する
        /// </summary>
        private void RefreshExtraInfoList()
        {
            ExtraInfoList.Clear();
            var list = ExecuteSelectReader($@"SELECT extra_info.id, extra_info.step, extra_info.next_id, extra_info.gear_prob,
                extra_info.gear_sure, extra_info.screw_prob, extra_info.screw_sure, extra_info.lost_id, extra_info.lost_count,
                weapon1.name AS weapon_name, repair_step.name AS step_name, weapon2.name AS next_weapon_name
                FROM extra_info, weapon AS weapon1, weapon AS weapon2, repair_step WHERE extra_info.id = weapon1.id
                AND extra_info.step = repair_step.step AND extra_info.next_id = weapon2.id ORDER BY extra_info.id");
            foreach (var pair in list)
            {
                long id = (long)(pair["id"]);
                long step = (long)(pair["step"]);
                long nextId = (long)(pair["next_id"]);
                long gearProb = (long)(pair["gear_prob"]);
                long gearSure = (long)(pair["gear_sure"]);
                long screwProb = (long)(pair["screw_prob"]);
                long screwSure = (long)(pair["screw_sure"]);
                long lostId = (long)(pair["lost_id"]);
                long lostCount = (long)(pair["lost_count"]);
                string weaponName = (string)(pair["weapon_name"]);
                string stepName = (string)(pair["step_name"]);
                string nextWeaponName = (string)(pair["next_weapon_name"]);
                var temp = new ExtraInfo()
                {
                    Id = (int)id,
                    Step = (int)step,
                    NextId = (int)nextId,
                    GearProb = (int)gearProb,
                    GearSure = (int)gearSure,
                    ScrewProb = (int)screwProb,
                    ScrewSure = (int)screwSure,
                    LostId = (int)lostId,
                    LostCount = (int)lostCount,
                    WeaponName = weaponName,
                    StepName = stepName,
                    NextWeaponName = nextWeaponName
                };
                ExtraInfoList.Add(temp);
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
        /// 改修段階のデータ
        /// </summary>
        public ObservableCollection<RepairStep> RepairStepList { get; } = new ObservableCollection<RepairStep>();

        /// <summary>
        /// 改修の基本情報のデータ
        /// </summary>
        public ObservableCollection<BasicInfo> BasicInfoList { get; } = new ObservableCollection<BasicInfo>();

        /// <summary>
        /// 改修の拡張情報のデータ
        /// </summary>
        public ObservableCollection<ExtraInfo> ExtraInfoList { get; } = new ObservableCollection<ExtraInfo>();

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
            ExecuteNonQuery($@"CREATE TABLE IF NOT EXISTS repair_step (
                step INTEGER NOT NULL UNIQUE,
                name TEXT NOT NULL UNIQUE,
                PRIMARY KEY(step))");
            ExecuteNonQuery($@"CREATE TABLE IF NOT EXISTS [extra_info] (
                [id] INTEGER NOT NULL REFERENCES [weapon]([id]),
                [step] INTEGER NOT NULL REFERENCES [repair_step]([step]),
                [next_id] INTEGER NOT NULL REFERENCES [weapon]([id]),
                [gear_prob] INTEGER NOT NULL,
                [gear_sure] INTEGER NOT NULL,
                [screw_prob] INTEGER NOT NULL,
                [screw_sure] INTEGER NOT NULL,
                [lost_id] INTEGER NOT NULL REFERENCES [weapon]([id]),
                [lost_count] INTEGER NOT NULL,
                UNIQUE([id],[step],[next_id]),
                PRIMARY KEY([id],[step],[next_id]))");

            // テーブルを初期化する
            if (ExecuteSelectReader("SELECT * FROM repair_step").Count == 0)
            {
                CreateRepairStepList();
            }
            if (ExecuteSelectReader("SELECT * FROM weapon").Count == 0)
            {
                PostWeapon(0, "なし");
            }

            // 表示内容を更新する
            RefreshKammusuList();
            RefreshWeaponList();
            RefreshRepairStepList();
            RefreshBasicInfoList();
            RefreshExtraInfoList();
        }

        /// <summary>
        /// 艦娘のデータを追加する
        /// </summary>
        /// <param name="id">艦船ID</param>
        /// <param name="name">艦名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PostKammusu(int id, string name)
        {
            ExecuteNonQuery($"INSERT INTO kammusu(id, name) values({id}, '{name}')");
            RefreshKammusuList();
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
            ExecuteNonQuery($"DELETE FROM kammusu WHERE id = {oldId}");
            ExecuteNonQuery($"INSERT INTO kammusu(id, name) values({id}, '{name}')");
            RefreshKammusuList();
            return true;
        }

        /// <summary>
        /// 艦娘のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteKammusu(int oldId)
        {

            ExecuteNonQuery($"DELETE FROM kammusu WHERE id = {oldId}");
            RefreshKammusuList();
            return true;
        }

        /// <summary>
        /// 装備のデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Weapon GetWeapon(int id)
        {
            var list = ExecuteSelectReader($"SELECT id, name FROM weapon WHERE id = {id}");
            if (list.Count == 0)
            {
                return null;
            }
            else
            {
                var temp = new Weapon() { Id = (int)((long)list[0]["id"]), Name = (string)list[0]["name"] };
                return temp;
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
            ExecuteNonQuery($"INSERT INTO weapon(id, name) values({id}, '{name}')");
            RefreshWeaponList();
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
            ExecuteNonQuery($"DELETE FROM weapon WHERE id = {oldId}");
            ExecuteNonQuery($"INSERT INTO weapon(id, name) values({id}, '{name}')");
            RefreshWeaponList();
            return true;
        }

        /// <summary>
        /// 装備のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteWeapon(int oldId)
        {
            ExecuteNonQuery($"DELETE FROM weapon WHERE id = {oldId}");
            RefreshWeaponList();
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
            ExecuteNonQuery($"INSERT INTO basic_info(id, fuel, ammo, steel, bauxite) values({id}, '{fuel}', '{ammo}', '{steel}', '{bauxite}')");
            RefreshBasicInfoList();
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
            ExecuteNonQuery($"DELETE FROM basic_info WHERE id = {oldId}");
            ExecuteNonQuery($"INSERT INTO basic_info(id, fuel, ammo, steel, bauxite) values({id}, '{fuel}', '{ammo}', '{steel}', '{bauxite}')");
            RefreshBasicInfoList();
            return true;
        }

        /// <summary>
        /// 改修の基本情報のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteWeaponBasicInfo(int oldId)
        {

            ExecuteNonQuery($"DELETE FROM basic_info WHERE id = {oldId}");
            RefreshBasicInfoList();
            return true;
        }

        /// <summary>
        /// 改修の拡張情報のデータを追加する
        /// </summary>
        /// <param name="id">装備ID</param>
        /// <param name="name">装備名</param>
        /// <returns>追加できたならtrue</returns>
        public bool PostWeaponExtraInfo(int id, int step, int next_id, int gear_prob, int gear_sure,
            int screw_prob, int screw_sure, int lost_id, int lost_count)
        {
            ExecuteNonQuery($"INSERT INTO extra_info(id, step, next_id, gear_prob," +
                $"gear_sure, screw_prob, screw_sure, lost_id, lost_count) " +
                $"values({id}, '{step}', '{next_id}', '{gear_prob}', '{gear_sure}," +
                $" '{screw_prob}', '{screw_sure}', '{lost_id}', '{lost_count}')");
            RefreshExtraInfoList();
            return true;
        }

        /// <summary>
        /// 改修の拡張情報のデータを更新する
        /// </summary>
        /// <param name="id">装備ID</param>
        /// <param name="name">装備名</param>
        /// <returns>更新できたならtrue</returns>
        public bool PutWeaponExtraInfo(int id, int step, int next_id, int gear_prob, int gear_sure,
            int screw_prob, int screw_sure, int lost_id, int lost_count, int oldId)
        {
            ExecuteNonQuery($"DELETE FROM extra_info WHERE id = {oldId}");
            ExecuteNonQuery($"INSERT INTO extra_info(id, step, next_id, gear_prob," +
                $"gear_sure, screw_prob, screw_sure, lost_id, lost_count) " +
                $"values({id}, '{step}', '{next_id}', '{gear_prob}', '{gear_sure}," +
                $" '{screw_prob}', '{screw_sure}', '{lost_id}', '{lost_count}')");
            RefreshBasicInfoList();
            return true;
        }

        /// <summary>
        /// 改修の拡張情報のデータを削除する
        /// </summary>
        /// <param name="oldId"></param>
        public bool DeleteWeaponExtraInfo(int oldId)
        {

            ExecuteNonQuery($"DELETE FROM extra_info WHERE id = {oldId}");
            RefreshExtraInfoList();
            return true;
        }
    }
}
