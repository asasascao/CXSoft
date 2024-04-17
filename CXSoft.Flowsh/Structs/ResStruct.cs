using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 返回结构
    /// </summary>
    public class ResStruct
    {
        public static ResStruct CreateInstance(string Name, object result)
        {
            ResStruct res = new ResStruct();
            res.Type = (int)ResType.Normal;
            res.Name = Name;
            res.Result = result;
            return res;
        }

        public static ResStruct CreateInstance(int type, string Name, object result)
        {
            ResStruct res = new ResStruct();
            res.Type = type;
            res.Name = Name;
            res.Result = result;
            return res;
        }

        internal static ResStruct CreateInstance(JumpTargetStruct jumpTarget)
        {
            ResStruct res_st = new ResStruct();
            res_st.Type = (int)ResType.Normal;
            res_st.JumpTarget = null;
            res_st.Result = null;
            res_st.Name = "";
            if (jumpTarget!=null)
            {
                res_st.Type = (int)ResType.Jump;
                res_st.JumpTarget = jumpTarget;
            }
            return res_st;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 跳转目标
        /// </summary>
        internal JumpTargetStruct JumpTarget { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>结果</returns>
        public T GetResult<T>()
        {
            return Result == null?default(T):(T)System.Convert.ChangeType(Result, typeof(T));
        }
    }
}
