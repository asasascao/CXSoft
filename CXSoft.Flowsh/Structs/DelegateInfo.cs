using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 输入参数
    /// </summary>
    public abstract class DelegateInfo
    {
        #region
        /// <summary>
        /// 委托信息
        /// </summary>
        public Delegate @delegate { get; protected set; }

        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> param { get; protected set; }

        /// <summary>
        /// 设置参数配置
        /// </summary>
        public List<string> ParamConfigs { get; protected set; }
        #endregion

        /// <summary>
        /// 增加参数配置 
        /// 格式：
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// --param_name--sourcelevel--methodname--source_param_name--in/out--
        /// 
        /// param_name 本方法参数名
        /// sourcelevel 源方法层级
        /// methodname 源方法名
        /// source_param_name 参数名
        /// in/out in和out里面的一个,代表入参或者出参 可缺损缺损时先取出参出参取不到取入参
        /// </summary>
        /// <param name="configStr">配置串</param>
        public DelegateInfo AddParamConfigs(string paramConfigs)
        {
            if (ParamConfigs == null)
            {
                ParamConfigs = new List<string>();
            }
            var striparr = paramConfigs.Split(new string[] { "\r\n" }, StringSplitOptions.None)?.
              Where(o => !string.IsNullOrWhiteSpace(o));
            if (striparr != null && striparr.Count() > 0)
            {
                this.ParamConfigs.AddRange(striparr);
            }
            return this;
        }
    }

    /// <summary>
    /// 逻辑结构输入参数
    /// </summary>
    public class LogicDelegateInfo: DelegateInfo
    {
        public static LogicDelegateInfo CreateInstance()
        {
            return new LogicDelegateInfo();
        }

        #region
        /// <summary>
        /// 返回字段
        /// </summary>
        public string ResName { get; private set; }
        #endregion

        public LogicDelegateInfo():base()
        {
        }

        public DelegateInfo Register(Action action)
            => Register(action, "", "");

        public DelegateInfo Register<T>(Action<T> action, string paramStr)
            => Register(action, paramStr, "");

        public DelegateInfo Register<T1,T2>(Action<T1,T2> action, string paramStr)
            => Register(action, paramStr, "");

        public DelegateInfo Register<T1, T2,T3>(Action<T1, T2,T3> action, string paramStr)
            => Register(action, paramStr, "");

        public DelegateInfo Register<T1, T2, T3,T4>(Action<T1, T2, T3, T4> action, string paramStr)
           => Register(action, paramStr, "");

        public DelegateInfo Register<T1, T2, T3, T4,T5>(Action<T1, T2, T3, T4, T5> action, string paramStr)
           => Register(action, paramStr, "");

        public DelegateInfo Register<T1, T2, T3, T4, T5,T6>(Action<T1, T2, T3, T4, T5,T6> action, string paramStr)
          => Register(action, paramStr, "");

        public DelegateInfo Register<T1, T2, T3, T4, T5, T6,T7>(Action<T1, T2, T3, T4, T5, T6,T7> action, string paramStr)
          => Register(action, paramStr, "");

        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7,T8>(Action<T1, T2, T3, T4, T5, T6, T7,T8> action, string paramStr)
          => Register(action, paramStr, "");

        public DelegateInfo Register<R>(Func<R> func, string defaultResName)
           => Register((Delegate)func, "", defaultResName);

        public DelegateInfo Register<T,R>(Func<T,R> func, string paramStr, string defaultResName)
            => Register((Delegate)func, paramStr, defaultResName);

        public DelegateInfo Register<T1,T2, R>(Func<T1,T2, R> func, string paramStr, string defaultResName)
           => Register((Delegate)func, paramStr, defaultResName);

        public DelegateInfo Register<T1, T2,T3, R>(Func<T1, T2,T3, R> func, string paramStr, string defaultResName)
          => Register((Delegate)func, paramStr, defaultResName);

        public DelegateInfo Register<T1, T2, T3,T4, R>(Func<T1, T2, T3,T4, R> func, string paramStr, string defaultResName)
          => Register((Delegate)func, paramStr, defaultResName);

        public DelegateInfo Register<T1, T2, T3, T4,T5, R>(Func<T1, T2, T3, T4,T5, R> func, string paramStr, string defaultResName)
         => Register((Delegate)func, paramStr, defaultResName);

        public DelegateInfo Register<T1, T2, T3, T4, T5,T6, R>(Func<T1, T2, T3, T4, T5,T6, R> func, string paramStr, string defaultResName)
         => Register((Delegate)func, paramStr, defaultResName);

        public DelegateInfo Register<T1, T2, T3, T4, T5, T6,T7, R>(Func<T1, T2, T3, T4, T5, T6,T7, R> func, string paramStr, string defaultResName)
        => Register((Delegate)func, paramStr, defaultResName);

        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7,T8, R>(Func<T1, T2, T3, T4, T5, T6, T7,T8, R> func, string paramStr, string defaultResName)
        => Register((Delegate)func, paramStr, defaultResName);

        /// <summary>
        /// 注册
        /// 参数格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="paramStr">参数</param>
        /// <param name="defaultResName">返回值名称 默认void为空</param>
        private DelegateInfo Register(Delegate @delegate, string paramStr, string defaultResName="")
            => Register(@delegate, SetDelegateParam(paramStr), defaultResName);

        /// <summary>
        /// 方法动作
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="paramStr">参数</param>
        /// <param name="defaultResName">返回值名称 默认void为空</param>
        public DelegateInfo Register(Delegate @delegate, Dictionary<string, object> param, string defaultResName="")
        {
            this.@delegate = @delegate;
            this.param = param;
            this.ResName = defaultResName;
            return this;
        }

        private Dictionary<string, object> SetDelegateParam(string paramStr)
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
                    if (paraminfo.Length >= 2)
                    {
                        var key = paraminfo[0].ToString();
                        var value = paraminfo[1].ToString();
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
                }
                catch { }
                #endregion
            }

            return param;
        }

        private void AddParam(Dictionary<string, object> param, string key, object value)
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
    /// 判断结构输入参数
    /// </summary>
    public class JudgeDelegateInfo : DelegateInfo
    {
        public static JudgeDelegateInfo CreateInstance()
        {
            return new JudgeDelegateInfo();
        }

        #region
        /// <summary>
        /// 跳转目标字典
        /// </summary>
        internal Dictionary<object, JumpTargetStruct> resJudgeDic { get; private set; }
        #endregion

        public JudgeDelegateInfo() : base()
        {
        }

        public DelegateInfo Register<R>(Func<R> func, string resdicStr)
           => Register(func, "", resdicStr);

        public DelegateInfo Register<T, R>(Func<T, R> func, string paramStr, string resdicStr)
           => Register(func, paramStr, resdicStr);

        public DelegateInfo Register<T1, T2, R>(Func<T1, T2, R> func, string paramStr, string resdicStr)
           => Register(func, paramStr, resdicStr);

        public DelegateInfo Register<T1, T2, T3, R>(Func<T1, T2, T3, R> func, string paramStr, string resdicStr)
          => Register(func, paramStr, resdicStr);

        public DelegateInfo Register<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> func, string paramStr, string resdicStr)
          => Register(func, paramStr, resdicStr);

        public DelegateInfo Register<T1, T2, T3, T4, T5, R>(Func<T1, T2, T3, T4, T5, R> func, string paramStr, string resdicStr)
         => Register(func, paramStr, resdicStr);

        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, R>(Func<T1, T2, T3, T4, T5, T6, R> func, string paramStr, string resdicStr)
         => Register(func, paramStr, resdicStr);

        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7, R>(Func<T1, T2, T3, T4, T5, T6, T7, R> func, string paramStr, string resdicStr)
        => Register(func, paramStr, resdicStr);

        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7, T8, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, string paramStr, string resdicStr)
        => Register(func, paramStr, resdicStr);

        /// <summary>
        /// 方法动作
        /// 参数格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="paramStr">参数</param>
        /// <param name="resdicStr">返回判断参数</param>
        public DelegateInfo Register(Delegate @delegate, string paramStr, string resdicStr)
            => Register(@delegate, SetDelegateParam(paramStr), SetResJudgeParam(resdicStr));

        /// <summary>
        /// 方法动作
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="param">参数</param>
        /// <param name="resdicStr">返回判断参数</param>
        public DelegateInfo Register(Delegate @delegate, Dictionary<string, object> param, string resdicStr)
            => Register(@delegate, param, SetResJudgeParam(resdicStr));

        /// <summary>
        /// 方法动作
        /// 参数格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="paramStr">参数</param>
        /// <param name="resJDic">返回判断参数</param>
        public DelegateInfo Register(Delegate @delegate, string paramStr, Dictionary<object, Tuple<int, int>> resJDic)
            => Register(@delegate, SetDelegateParam(paramStr), resJDic);

        /// <summary>
        /// 方法动作
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="param">参数</param>
        /// <param name="resJDic">返回判断参数</param>
        public DelegateInfo Register(Delegate @delegate, Dictionary<string, object> param, Dictionary<object, Tuple<int, int>> resJDic)
        {
            this.@delegate = @delegate;
            this.param = param;
            this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            if (resJDic != null && resJDic.Count() > 0)
            {
                foreach (var item in resJDic)
                {
                    this.resJudgeDic.Add(item.Key, new JumpTargetStruct()
                    { Level = item.Value.Item1, ExecuteGroupType = item.Value.Item2 });
                }
            }

            return this;
        }

        private Dictionary<string, object> SetDelegateParam(string paramStr)
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
                    if (paraminfo.Length >= 2)
                    {
                        var key = paraminfo[0].ToString();
                        var value = paraminfo[1].ToString();
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
                }
                catch { }
                #endregion
            }

            return param;
        }

        private Dictionary<object, Tuple<int, int>> SetResJudgeParam(string paramStr)
        {
            Dictionary<object, Tuple<int, int>> param = new Dictionary<object, Tuple<int, int>>();
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
                    if (spl != null && spl.Count() >= 2)
                    {
                        string v = spl.Last();
                        if (int.TryParse(v, out int level))
                        {
                            var key = value.Replace("-" + level, "");
                            if (param.ContainsKey(key))
                            {
                                param[key] = new Tuple<int, int>(level, -1);
                            }
                            else
                            {
                                param.Add(key, new Tuple<int, int>(level, -1));
                            }
                        }
                    }
                }
                catch { }
                #endregion
            }

            return param;
        }

        private void AddParam(Dictionary<string, object> param, string key, object value)
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
}
