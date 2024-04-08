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
        public override ResStruct Execute()
        {
            var res=base.DoExecute();
            var res_st=new ResStruct();
            res_st.Type = ResType.Normal;
            res_st.JumpTarget = null;
            res_st.Res = null;
            res_st.Name = "";
            if (ResJudgeDic.ContainsKey(res))
            {
                res_st.Type = ResType.Jump;
                res_st.JumpTarget=ResJudgeDic[res];
            }
            return res_st;
        }
    }
}
