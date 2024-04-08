using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 返回结构
    /// </summary>
    public struct ResStruct
    {
        /// <summary>
        /// 类型
        /// </summary>
        internal ResType Type { get; set; }

        /// <summary>
        /// 跳转目标
        /// </summary>
        internal JumpTargetStruct JumpTarget { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public object Res { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Name { get; set; }
    }
}
