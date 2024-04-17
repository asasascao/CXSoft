using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 结果类别
    /// </summary>
    public enum ResType
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 跳转
        /// </summary>
        Jump = 1,
        /// <summary>
        /// 报错
        /// </summary>
        Exception = 2
    }
}
