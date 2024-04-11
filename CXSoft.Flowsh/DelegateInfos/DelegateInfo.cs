using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 异步方法完成委托
    /// </summary>
    /// <param name="data">结果参数</param>
    public delegate void ComplateAsyncDelegateInfo(Dictionary<string, object> data);

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
        /// 方法名称
        /// </summary>
        public string MethodName { get; protected set; } = "";

        /// <summary>
        /// 参数列表
        /// </summary>
        public ParameterInfo[] Parameters { get; protected set; } = null;

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
            if (ParamConfigs == null) ParamConfigs = new List<string>();
            var striparr = paramConfigs.Split(new string[] { "\r\n" }, StringSplitOptions.None)?.
              Where(o => !string.IsNullOrWhiteSpace(o));
            if (striparr != null && striparr.Count() > 0)
            {
                this.ParamConfigs.AddRange(striparr);
            }
            return this;
        }
    }
}
