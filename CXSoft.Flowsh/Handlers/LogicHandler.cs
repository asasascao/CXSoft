using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 逻辑处理器
    /// </summary>
    public class LogicHandler : AbstractHandler
    {
        /// <summary>
        /// 方法参数名
        /// </summary>
        public List<Param> OutParamInfo { get; private set; }

        /// <summary>
        /// 默认返回名称
        /// </summary>
        public string DefaultResName { get; private set; } = "";

        public LogicHandler() : base()
        {
            OutParamInfo = new List<Param>();
            Delegate = null;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="defaultResName"></param>
        /// <returns></returns>
        internal override AbstractHandler Register(DelegateInfo method)
        {
            base.Register(method);
            this.DefaultResName = ((LogicDelegateInfo)method).ResName;
            OutParamInfo = new List<Param>();
            if (!string.IsNullOrWhiteSpace(DefaultResName))
            {
                OutParamInfo.Add(new Param() { Name = DefaultResName });
            }
            return this;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>返回值</returns>
        public override ResStruct Execute()
        {
            #region
            var res = base.DoExecute();
            if(OutParamInfo!=null&& OutParamInfo.Count()>0)
            {
                OutParamInfo.ForEach((p) => { p.ValueInfo = res; });
            }
            return new ResStruct() { Type=ResType.Normal,Res= res, Name= DefaultResName };
            #endregion
        }
    }
}
