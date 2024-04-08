using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    internal class LevelList<T>: List<T>
    {
        /// <summary>
        /// 层级
        /// </summary>
        internal int Level { get; set; } = -1;
    }
}
