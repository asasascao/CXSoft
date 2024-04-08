using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 跳转目标结构
    /// </summary>
    internal class JumpTargetStruct
    {
        /// <summary>
        /// 步骤号
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 执行组别
        /// </summary>
        public int ExecuteGroupType { get; set; }
    }
}
