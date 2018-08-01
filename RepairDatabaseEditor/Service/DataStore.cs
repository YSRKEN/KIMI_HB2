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

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataStore()
        {
            using(var sr = new StreamReader(@"DB/kammusu_list.json", Encoding.UTF8))
            {
                string jsonText = sr.ReadToEnd();
                Kammusus = JsonConvert.DeserializeObject<List<Kammusu>>(jsonText);
            }
        }
    }
}
