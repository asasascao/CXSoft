using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 参数转换
    /// </summary>
    public class ParamTransfer: ITransfer
    {
        #region
        /// <summary>
        /// 参数名
        /// </summary>
        internal string ParamName { get; set; } = "";

        /// <summary>
        /// 源等级
        /// </summary>
        internal int SourceLevel { get; set; } = 0;

        /// <summary>
        /// 源方法名
        /// </summary>
        internal string MethodName { get; set; } = "";

        /// <summary>
        /// 源方法序号
        /// </summary>
        internal int MethodIndex { get; set; } = 0;

        /// <summary>
        /// 源参数名
        /// </summary>
        internal string SourceParamName { get; set; } = "";

        /// <summary>
        /// 参数in/out
        /// </summary>
        internal string ParamInOut { get; set; } = "";
        #endregion

        /// <summary>
        /// 设置转换器
        /// </summary>
        /// <param name="parts">配置字段</param>
        public void SetTransfer(string[] parts)
        {
            #region
            if (parts == null) return;

            ParamName = parts.Length > 0 ? parts[0]:"";

            int sourcelevel = 0;
            if (parts.Length > 1)
            {
                int.TryParse(parts[1], out sourcelevel);
            }
            SourceLevel = sourcelevel;

            if (parts.Length > 2)
            {
                if(parts[2].Contains('_'))
                {
                    string methodname = parts[2];
                    string parmi=parts[2];
                    var ml= parmi.Split('_');
                    int methodindex = -1;
                    int.TryParse(ml.LastOrDefault(), out methodindex);
                    if(methodindex != -1)
                    {
                        methodname=parmi.Replace("_" + methodindex.ToString(), "");
                    }
                    MethodName = methodname;
                    MethodIndex = methodindex;
                }
                else
                {
                    MethodName = parts[2];
                    MethodIndex = 0;
                }
            }

            SourceParamName = parts.Length > 3 ? parts[3]:"";

            ParamInOut = parts.Length > 4 ? parts[4] : "";
            #endregion
        }
    }
}
