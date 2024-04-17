using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 流程接口
    /// </summary>
    public interface IFlowsh
    {
        /// <summary>
        /// 索引
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 索引
        /// </summary>
        int Type { get; }

        /// <summary>
        /// 描述
        /// </summary>
        string Discription { get; set; }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="level">层级 默认传-1为顺序模式</param>
        /// <param name="steps">步骤 --委托 --层级 默认传-1为顺序模式</param>
        /// <returns>流程</returns>
        Flowsh AddHandlers(params Tuple<DelegateInfo, int>[] steps);

        /// <summary>
        /// 添加层级步骤
        /// </summary>
        /// <param name="level">层级 默认传-1为顺序模式</param>
        /// <param name="steps">步骤</param>
        /// <returns>流程</returns>
        Flowsh AddLevelHandlers(int level = -1, params DelegateInfo[] steps);

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="delegate">委托</param>
        /// <param name="level">层级 默认传-1为顺序模式</param>
        Flowsh AddHandler(DelegateInfo @delegate, int level = -1);

        /// <summary>
        /// 获取委托信息列表
        /// </summary>
        /// <param name="level">步骤次序</param>
        /// <param name="methodname">方法名</param>
        /// <returns>委托信息集</returns>
        IEnumerable<DelegateInfo> GetDelegateByLevelArray(int level, string methodname);

        /// <summary>
        /// 获取委托信息
        /// </summary>
        /// <param name="level">步骤次序</param>
        /// <param name="methodname">方法名</param>
        /// <param name="methodindex">方法顺序</param>
        /// <returns>委托信息</returns>
        DelegateInfo GetDelegateByLevel(int level, string methodname, int methodindex = 0);

        /// <summary>
        /// 获取步骤图谱
        /// </summary>
        /// <returns>图谱</returns>
        string[,] GetHandlers();

        /// <summary>
        /// 执行
        /// </summary>
        ResStruct[] Execute();
    }
}
