using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairDatabaseEditor.Model
{
    /// <summary>
    /// 改修段階
    /// </summary>
    class RepairStep
    {
        /// <summary>
        /// 段階
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 段階名
        /// </summary>
        public string Name { get; set; }
    }
}
