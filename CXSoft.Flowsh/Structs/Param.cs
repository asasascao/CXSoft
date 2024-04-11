using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 参数
    /// </summary>
    public class Param
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否有默认值
        /// </summary>
        public bool HasDefaultValue { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object ValueInfo { get; set; }

        public override string ToString()
        {
            return Name+":"+ ValueInfo.ToString();
        }
    }
}
