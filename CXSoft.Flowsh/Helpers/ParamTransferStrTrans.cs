using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 参数串转参数转换
    /// </summary>
    internal class ParamStrToParamTransfer
    {
        /// <summary>
        /// 设置参数转换
        /// 格式：
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// </summary>
        /// <param name="paramStr">参数转换串</param>
        /// <returns>参数转换</returns>
        internal static ParamTransfer Transfer(string paramStr)
        {
            if (paramStr == null || string.IsNullOrWhiteSpace(paramStr)) return null;
            string pattern = @"--";
            string[] parts = Regex.Split(paramStr, pattern)?.Where(o => !string.IsNullOrWhiteSpace(o))?.ToArray();
            if (parts == null || parts.Length<=0) return null;
            ParamTransfer transfer = new ParamTransfer();
            transfer.ParamName = parts.Length > 0 ? parts[0] : "";

            int sourcelevel = 0;
            if (parts.Length > 1)
            {
                int.TryParse(parts[1], out sourcelevel);
            }
            transfer.SourceLevel = sourcelevel;

            if (parts.Length > 2)
            {
                if (parts[2].Contains('_'))
                {
                    string methodname = parts[2];
                    string parmi = parts[2];
                    var ml = parmi.Split('_');
                    int methodindex = -1;
                    int.TryParse(ml.LastOrDefault(), out methodindex);
                    if (methodindex != -1)
                    {
                        methodname = parmi.Replace("_" + methodindex.ToString(), "");
                    }
                    transfer.MethodName = methodname;
                    transfer.MethodIndex = methodindex;
                }
                else
                {
                    transfer.MethodName = parts[2];
                    transfer.MethodIndex = 0;
                }
            }

            transfer.SourceParamName = parts.Length > 3 ? parts[3] : "";

            transfer.ParamInOut = parts.Length > 4 ? parts[4] : "";

            return transfer;
        }
    }

    /// <summary>
    /// 参数转换转参数串
    /// </summary>
    internal class ParamTransferToStr
    {
        /// <summary>
        /// 设置代理参数
        /// 格式：
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// </summary>
        /// <param name="param">参数转换</param>
        /// <returns>参数转换串</returns>
        internal static string Transfer(IEnumerable<ParamTransfer> param)
        {
            if (param == null || param.Count() <= 0) return "";
            if(param.Count() == 1)
            {
                ParamTransfer paramitem = param.First();
                string str="--" + paramitem.ParamName + "--" + paramitem.SourceLevel + "--";
                if(paramitem.MethodIndex<=0)
                {
                    str+= paramitem.MethodName + "--";
                }
                else
                {
                    str += paramitem.MethodName+"_"+ paramitem.MethodIndex + "--";
                }
                if(!string.IsNullOrWhiteSpace(paramitem.SourceParamName))
                {
                    str += paramitem.SourceParamName + "--";
                }
                if (!string.IsNullOrWhiteSpace(paramitem.ParamInOut))
                {
                    str += paramitem.ParamInOut + "--";
                }
                return str;
            }
            else
            {
                List<string> strlist = new List<string>();
                foreach (var paramitem in param)
                {
                    string str = "--" + paramitem.ParamName + "--" + paramitem.SourceLevel + "--";
                    if (paramitem.MethodIndex <= 0)
                    {
                        str += paramitem.MethodName + "--";
                    }
                    else
                    {
                        str += paramitem.MethodName + "_" + paramitem.MethodIndex + "--";
                    }
                    if (!string.IsNullOrWhiteSpace(paramitem.SourceParamName))
                    {
                        str += paramitem.SourceParamName + "--";
                    }
                    if (!string.IsNullOrWhiteSpace(paramitem.ParamInOut))
                    {
                        str += paramitem.ParamInOut + "--";
                    }
                    strlist.Add(str);
                }
                return string.Join(Environment.NewLine, strlist);
            }
        }
    }
}
