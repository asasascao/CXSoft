using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 参数串转字典
    /// </summary>
    internal class ParamStrToDic
    {
        /// <summary>
        /// 设置代理参数
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <param name="paramStr">参数串</param>
        /// <returns>参数集</returns>
        internal static Dictionary<string, object> Transfer(string paramStr)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            if (paramStr == null || string.IsNullOrWhiteSpace(paramStr)) return param;
            var paramsinfos = paramStr.Split(';');
            if (paramsinfos == null || paramsinfos.Count() <= 0) return param;
            foreach (var paramsinfo in paramsinfos)
            {
                #region
                try
                {
                    var paraminfo = paramsinfo.Split(':');
                    var key = paraminfo.Length > 0 ? paraminfo[0].ToString() : "";
                    var value = paraminfo.Length > 1 ? paraminfo[1].ToString() : "";
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        AddParam(param, key, value);
                    }
                    else
                    {
                        value = value.Trim();
                        if (int.TryParse(value, out int v))
                        {
                            AddParam(param, key, v);
                        }
                        else if (bool.TryParse(value, out bool vb))
                        {
                            AddParam(param, key, vb);
                        }
                        else if (long.TryParse(value, out long vl))
                        {
                            AddParam(param, key, vl);
                        }
                        else if (double.TryParse(value, out double vd))
                        {
                            AddParam(param, key, vd);
                        }
                        else if (DateTime.TryParse(value, out DateTime vt))
                        {
                            AddParam(param, key, vt);
                        }
                        else
                        {
                            AddParam(param, key, value);
                        }
                    }
                }
                catch
                { }
                #endregion
            }

            return param;
        }

        /// <summary>
        /// 添加参数集
        /// </summary>
        /// <param name="param">参数集</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        private static void AddParam(Dictionary<string, object> param, string key, object value)
        {
            if (param.ContainsKey(key))
            {
                param[key] = value;
            }
            else
            {
                param.Add(key, value);
            }
        }
    }

    /// <summary>
    /// 字典转参数串
    /// </summary>
    internal class ParamDicToStr
    {
        /// <summary>
        /// 设置代理参数
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>参数串</returns>
        internal static string Transfer(Dictionary<string, object> param)
        {
            if (param == null || param.Count <= 0) return "";
            return string.Join(";", param.Select(i => i.Key + ":" + i.Value.ToString()));
        }
    }
}
