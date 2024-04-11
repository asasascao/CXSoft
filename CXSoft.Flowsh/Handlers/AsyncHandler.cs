using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 异步逻辑处理器
    /// </summary>
    public class AsyncHandler : AbstractHandler
    {
        #region
        /// <summary>
        /// 方法参数名
        /// </summary>
        public List<Param> OutParamInfo { get; private set; }

        /// <summary>
        /// 默认返回名称
        /// </summary>
        public string DefaultResName { get; private set; } = "";
        #endregion

        public AsyncHandler() : base()
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
            this.DefaultResName = ((AsyncDelegateInfo)method).ResName;
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
            return ExecuteInfo().Result;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>返回值</returns>
        private async Task<ResStruct> ExecuteInfo()
        {
            #region
            bool isback = false;
            ((AsyncDelegateInfo)this.Delegate).ComplateHandler = (clist) => {
                //异步回调
                if (clist != null && clist.Count > 0)
                {
                    foreach (var item in clist)
                    {
                        var opi = OutParamInfo.FirstOrDefault(o => o.Name == item.Key);
                        if (opi != null)
                        {
                            OutParamInfo.Remove(opi);
                        }
                        OutParamInfo.Add(new Param() { Name = item.Key, ValueInfo = item.Value });
                    }
                }
                isback = true;
            };
            var res = base.DoExecute();
            if (OutParamInfo != null && OutParamInfo.Count() > 0)
            {
                OutParamInfo.ForEach((p) => { if(p.Name == DefaultResName) p.ValueInfo = res; });
            }
            await Task.Run(() => // 创建一个子线程
            {
                while (!isback)
                {
                    Thread.Sleep(1000);
                }
            });
            isback = false;
            return new ResStruct() { Type = ResType.Normal, Res = res, Name = DefaultResName };
            #endregion
        }
    }
}
