using RepairDatabaseEditor.Model;
using System;
using System.Collections.Generic;
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
            // スタブ
            Kammusus.Add(new Kammusu() { Id = 0, Name = "○" });
            Kammusus.Add(new Kammusu() { Id = 1, Name = "吹雪" });

        }
    }
}
