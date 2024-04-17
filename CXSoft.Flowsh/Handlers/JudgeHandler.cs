using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 判断逻辑处理器
    /// </summary>
    public class JudgeHandler : AbstractHandler
    {
        Dictionary<object, JumpTargetStruct> ResJudgeDic = null;//结果判断字典

        public JudgeHandler() : base()
        { }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal override AbstractHandler Register(DelegateInfo method)
        {
            base.Register(method);
            ResJudgeDic = ((JudgeDelegateInfo)method).resJudgeDic;
            return this;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>返回结果</returns>
        public override ResStruct[] Execute()
        {
            var res=base.DoExecute();
            var ju = ResJudgeDic!=null && ResJudgeDic.Count>0 && 
                ResJudgeDic.ContainsKey(res) ? ResJudgeDic[res] : null;
            var resi = ResStruct.CreateInstance(ju);
            return resi.Type==(int)ResType.Normal ? null: new ResStruct[] { resi };
        }
    }
}
