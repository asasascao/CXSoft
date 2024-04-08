using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 流程工厂
    /// </summary>
    public class FlowshFactory
    {
        /// <summary>
        /// 流程字典
        /// </summary>
        static List<IFlowsh> flowshDic = new List<IFlowsh>();

        /// <summary>
        /// 创建流程
        /// </summary>
        /// <returns>流程</returns>
        public static Flowsh CreateFlowsh()
        {
            Flowsh flowsh = new Flowsh();
            flowshDic.Add(flowsh);
            return flowsh;
        }

        /// <summary>
        /// 创建流程
        /// </summary>
        /// <returns>流程</returns>
        public static Flowsh CreateFlowsh(int type = (int)FlowshType.Normal, string discription = "")
        {
            Flowsh flowsh = new Flowsh(type, discription);
            flowshDic.Add(flowsh);
            return flowsh;
        }

        /// <summary>
        /// 获取通过流程id
        /// </summary>
        /// <param name="id">流程id</param>
        /// <returns>流程</returns>
        public static IFlowsh GetFlowshByType(string id)
        => flowshDic.FirstOrDefault(o => o.Key == id);

        /// <summary>
        /// 获取通过流程类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>流程</returns>
        public static IEnumerable<IFlowsh> GetFlowshByType(int type = (int)FlowshType.Normal)
        => flowshDic.Where(o => o.Type == type);

        /// <summary>
        /// 获取通过流程描述
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>流程</returns>
        public static IEnumerable<IFlowsh> GetFlowshByDiscriptionContain(string keyword = "")
        => flowshDic.Where(o => o.Discription.Contains(keyword));
    }
}
