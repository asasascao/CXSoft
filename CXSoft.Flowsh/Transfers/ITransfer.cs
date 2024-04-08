using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 转换器
    /// </summary>
    public interface ITransfer
    {
        /// <summary>
        /// 设置转换
        /// </summary>
        /// <param name="parts"></param>
        void SetTransfer(string[] parts);
    }

    internal interface IConfig
    {
        void SetConfig(string[] parts);
    }
}
