using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 判断结果串转字典
    /// </summary>
    internal class JudgeStrToDic
    {
        /// <summary>
        /// 设置判断参数
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <param name="paramStr">返回判断参数串</param>
        /// <returns>参数集</returns>
        internal static Dictionary<object, int> Transfer(string paramStr)
        {
            Dictionary<object, int> param = new Dictionary<object, int>();
            if (paramStr == null || string.IsNullOrWhiteSpace(paramStr)) return param;
            var paramsinfos = paramStr.Split(';');
            if (paramsinfos == null || paramsinfos.Count() <= 0) return param;
            foreach (var paramsinfo in paramsinfos)
            {
                #region
                try
                {
                    var value = paramsinfo;
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    value = value.Trim();
                    var spl = value.Split('-');
                    string v = (spl != null && spl.Count() > 1) ? spl.Last() : "";
                    if (int.TryParse(v, out int level))
                    {
                        var key = value.Replace("-" + level, "");
                        if (param.ContainsKey(key))
                        {
                            param[key] = level;
                        }
                        else
                        {
                            param.Add(key, level);
                        }
                    }
                }
                catch { }
                #endregion
            }

            return param;
        }
    }

    /// <summary>
    /// 字典转参数串
    /// </summary>
    internal class JudgeDicToStr
    {
        /// <summary>
        /// 设置代理参数
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>返回判断参数串</returns>
        internal static string Transfer(Dictionary<string, int> param)
        {
            if (param == null || param.Count <= 0) return "";
            return string.Join(";", param.Select(i => i.Key + "-" + i.Value.ToString()));
        }
    }
}
