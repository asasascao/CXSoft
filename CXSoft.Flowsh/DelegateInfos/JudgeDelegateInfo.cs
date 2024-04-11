using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 判断结构输入参数
    /// </summary>
    public class JudgeDelegateInfo : DelegateInfo
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns>判断输入参数结构</returns>
        public static JudgeDelegateInfo CreateInstance()
            => new JudgeDelegateInfo();

        #region
        /// <summary>
        /// 跳转目标字典
        /// </summary>
        internal Dictionary<object, JumpTargetStruct> resJudgeDic { get; private set; }
        #endregion

        public JudgeDelegateInfo() : base()
        { }

        #region
        #region 无入参
        /// <summary>
        /// 注册
        /// 返回判断参数串 x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">功能</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<R>(Func<R> func, string resdicStr)
           => Register(func, new Dictionary<string, object>(), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">功能</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<R>(Func<R> func, Dictionary<object, int> resJDic)
           => Register(func, new Dictionary<string, object>(), resJDic);
        #endregion

        #region 一入参
        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T">动作参数类型</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="defaultResName">默认返回名称</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T, R>(Func<T, R> func, string paramStr, string resdicStr)
           => Register(func, ParamStrToDic.Transfer(paramStr), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <typeparam name="T">动作参数类型</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T, R>(Func<T, R> func, string paramStr, Dictionary<object, int> resJDic)
           => Register(func, ParamStrToDic.Transfer(paramStr), resJDic);

        /// <summary>
        /// 注册
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T">动作参数类型</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param">参数</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T, R>(Func<T, R> func, T param, string resdicStr)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 0)
            {
                m_params.Add(parameters[0], param);
            }
            this.param = m_params;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T">动作参数类型</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param">参数</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T, R>(Func<T, R> func, T param, Dictionary<object, int> resJDic)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 0)
            {
                m_params.Add(parameters[0], param);
            }
            this.param = m_params;
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion

        #region 二入参
        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="defaultResName">默认返回名称</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1,T2, R>(Func<T1, T2, R> func, string paramStr, string resdicStr)
           => Register(func, ParamStrToDic.Transfer(paramStr), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, R>(Func<T1, T2, R> func, string paramStr, Dictionary<object, int> resJDic)
           => Register(func, ParamStrToDic.Transfer(paramStr), resJDic);

        /// <summary>
        /// 注册
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, R>(Func<T1, T2, R> func, T1 param1, T2 param2, string resdicStr)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 1)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
            }
            this.param = m_params;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, R>(Func<T1, T2, R> func, T1 param1, T2 param2, Dictionary<object, int> resJDic)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 1)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
            }
            this.param = m_params;
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion

        #region 三入参
        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="defaultResName">默认返回名称</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2,T3, R>(Func<T1, T2,T3, R> func, string paramStr, string resdicStr)
           => Register(func, ParamStrToDic.Transfer(paramStr), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, R>(Func<T1, T2, T3, R> func, string paramStr, Dictionary<object, int> resJDic)
           => Register(func, ParamStrToDic.Transfer(paramStr), resJDic);

        /// <summary>
        /// 注册
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, R>(Func<T1, T2, T3, R> func, T1 param1, T2 param2, T3 param3, string resdicStr)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 2)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
            }
            this.param = m_params;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, R>(Func<T1, T2, T3, R> func, T1 param1, T2 param2, T3 param3, Dictionary<object, int> resJDic)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 2)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
            }
            this.param = m_params;
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion

        #region 四入参
        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="defaultResName">默认返回名称</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3,T4, R>(Func<T1, T2, T3, T4, R> func, string paramStr, string resdicStr)
           => Register(func, ParamStrToDic.Transfer(paramStr), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> func, string paramStr, Dictionary<object, int> resJDic)
           => Register(func, ParamStrToDic.Transfer(paramStr), resJDic);

        /// <summary>
        /// 注册
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> func, T1 param1, T2 param2, T3 param3, T4 param4, string resdicStr)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 3)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
            }
            this.param = m_params;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> func, T1 param1, T2 param2, T3 param3, T4 param4, Dictionary<object, int> resJDic)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 3)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
            }
            this.param = m_params;
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion

        #region 五入参
        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="defaultResName">默认返回名称</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4,T5, R>(Func<T1, T2, T3, T4, T5, R> func, string paramStr, string resdicStr)
           => Register(func, ParamStrToDic.Transfer(paramStr), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, R>(Func<T1, T2, T3, T4, T5, R> func, string paramStr, Dictionary<object, int> resJDic)
           => Register(func, ParamStrToDic.Transfer(paramStr), resJDic);

        /// <summary>
        /// 注册
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="param5">参数5</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, R>(Func<T1, T2, T3, T4, T5, R> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, string resdicStr)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 4)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
                m_params.Add(parameters[4], param5);
            }
            this.param = m_params;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="param5">参数5</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, R>(Func<T1, T2, T3, T4, T5, R> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, Dictionary<object, int> resJDic)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 4)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
                m_params.Add(parameters[4], param5);
            }
            this.param = m_params;
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion

        #region 六入参
        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="defaultResName">默认返回名称</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5,T6, R>(Func<T1, T2, T3, T4, T5, T6, R> func, string paramStr, string resdicStr)
           => Register(func, ParamStrToDic.Transfer(paramStr), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, R>(Func<T1, T2, T3, T4, T5, T6, R> func, string paramStr, Dictionary<object, int> resJDic)
           => Register(func, ParamStrToDic.Transfer(paramStr), resJDic);

        /// <summary>
        /// 注册
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="param5">参数5</param>
        /// <param name="param6">参数6</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, R>(Func<T1, T2, T3, T4, T5, T6, R> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, string resdicStr)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 5)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
                m_params.Add(parameters[4], param5);
                m_params.Add(parameters[5], param6);
            }
            this.param = m_params;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="param5">参数5</param>
        /// <param name="param6">参数6</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, R>(Func<T1, T2, T3, T4, T5, T6, R> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, Dictionary<object, int> resJDic)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 5)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
                m_params.Add(parameters[4], param5);
                m_params.Add(parameters[5], param6);
            }
            this.param = m_params;
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion

        #region 七入参
        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="T7">动作参数类型7</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="defaultResName">默认返回名称</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6,T7, R>(Func<T1, T2, T3, T4, T5, T6, T7, R> func, string paramStr, string resdicStr)
           => Register(func, ParamStrToDic.Transfer(paramStr), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="T7">动作参数类型7</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7, R>(Func<T1, T2, T3, T4, T5, T6, T7, R> func, string paramStr, Dictionary<object, int> resJDic)
           => Register(func, ParamStrToDic.Transfer(paramStr), resJDic);

        /// <summary>
        /// 注册
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="T7">动作参数类型7</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="param5">参数5</param>
        /// <param name="param6">参数6</param>
        /// <param name="param7">参数7</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7, R>(Func<T1, T2, T3, T4, T5, T6, T7, R> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, string resdicStr)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 6)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
                m_params.Add(parameters[4], param5);
                m_params.Add(parameters[5], param6);
                m_params.Add(parameters[6], param7);
            }
            this.param = m_params;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="T7">动作参数类型7</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="param5">参数5</param>
        /// <param name="param6">参数6</param>
        /// <param name="param7">参数7</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7, R>(Func<T1, T2, T3, T4, T5, T6, T7, R> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, Dictionary<object, int> resJDic)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 6)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
                m_params.Add(parameters[4], param5);
                m_params.Add(parameters[5], param6);
                m_params.Add(parameters[6], param7);
            }
            this.param = m_params;
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion

        #region 八入参
        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="T7">动作参数类型7</typeparam>
        /// <typeparam name="T8">动作参数类型8</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="defaultResName">默认返回名称</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7,T8, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, string paramStr, string resdicStr)
           => Register(func, ParamStrToDic.Transfer(paramStr), JudgeStrToDic.Transfer(resdicStr));

        /// <summary>
        /// 注册
        /// 参数串格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="T7">动作参数类型7</typeparam>
        /// <typeparam name="T8">动作参数类型8</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="paramStr">参数串</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7, T8, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, string paramStr, Dictionary<object, int> resJDic)
           => Register(func, ParamStrToDic.Transfer(paramStr), resJDic);

        /// <summary>
        /// 注册
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="T7">动作参数类型7</typeparam>
        /// <typeparam name="T8">动作参数类型8</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="param5">参数5</param>
        /// <param name="param6">参数6</param>
        /// <param name="param7">参数7</param>
        /// <param name="param8">参数8</param>
        /// <param name="resdicStr">返回判断参数串</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7, T8, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, string resdicStr)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 7)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
                m_params.Add(parameters[4], param5);
                m_params.Add(parameters[5], param6);
                m_params.Add(parameters[6], param7);
                m_params.Add(parameters[7], param8);
            }
            this.param = m_params;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T1">动作参数类型1</typeparam>
        /// <typeparam name="T2">动作参数类型2</typeparam>
        /// <typeparam name="T3">动作参数类型3</typeparam>
        /// <typeparam name="T4">动作参数类型4</typeparam>
        /// <typeparam name="T5">动作参数类型5</typeparam>
        /// <typeparam name="T6">动作参数类型6</typeparam>
        /// <typeparam name="T7">动作参数类型7</typeparam>
        /// <typeparam name="T8">动作参数类型8</typeparam>
        /// <typeparam name="R">返回类型</typeparam>
        /// <param name="func">动作</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <param name="param5">参数5</param>
        /// <param name="param6">参数6</param>
        /// <param name="param7">参数7</param>
        /// <param name="param8">参数8</param>
        /// <param name="resJDic">返回判断参数</param>
        /// <returns>委托信息</returns>
        public DelegateInfo Register<T1, T2, T3, T4, T5, T6, T7, T8, R>(Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, Dictionary<object, int> resJDic)
        {
            this.@delegate = func;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            Dictionary<string, object> m_params = new Dictionary<string, object>();
            var parameters = func?.Method?.GetParameters()?.Select(parameter => parameter.Name)?.ToArray();
            if (parameters != null && parameters.Count() > 7)
            {
                m_params.Add(parameters[0], param1);
                m_params.Add(parameters[1], param2);
                m_params.Add(parameters[2], param3);
                m_params.Add(parameters[3], param4);
                m_params.Add(parameters[4], param5);
                m_params.Add(parameters[5], param6);
                m_params.Add(parameters[6], param7);
                m_params.Add(parameters[7], param8);
            }
            this.param = m_params;
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion

        #region 无关参数数量
        /// <summary>
        /// 方法动作
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="param">参数</param>
        /// <param name="resJDic">返回判断参数</param>
        public DelegateInfo Register(Delegate @delegate, Dictionary<string, object> param, Dictionary<object, int> resJDic)
        {
            this.@delegate = @delegate;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            this.param = param;
            this.resJudgeDic=resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 方法动作
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="param">参数</param>
        /// <param name="resdicStr">返回判断参数</param>
        public DelegateInfo Register(Delegate @delegate, Dictionary<string, object> param, string resdicStr)
        {
            this.@delegate = @delegate;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            this.param = param;
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 方法动作
        /// 参数格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="paramStr">参数</param>
        /// <param name="resJDic">返回判断参数</param>
        public DelegateInfo Register(Delegate @delegate, string paramStr, Dictionary<object, int> resJDic)
        {
            this.@delegate = @delegate;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            this.param = ParamStrToDic.Transfer(paramStr);
            this.resJudgeDic = resJDic?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }

        /// <summary>
        /// 方法动作
        /// 参数格式 xxx:vvv;xxx:vvv;xxx:vvv
        /// 返回判断参数  x-level;y-level;z-level
        /// </summary>
        /// <param name="delegate">方法</param>
        /// <param name="paramStr">参数</param>
        /// <param name="resdicStr">返回判断参数</param>
        public DelegateInfo Register(Delegate @delegate, string paramStr, string resdicStr)
        {
            this.@delegate = @delegate;
            this.Parameters = this.@delegate?.Method?.GetParameters();
            this.MethodName = this.@delegate?.Method == null ? "" : this.@delegate.Method.Name;
            this.param = ParamStrToDic.Transfer(paramStr);
            this.resJudgeDic = JudgeStrToDic.Transfer(resdicStr)?.Select(item => new KeyValuePair<object, JumpTargetStruct>(item.Key,
                    new JumpTargetStruct() { Level = item.Value, ExecuteGroupType = -1 }))?.
                    ToDictionary(o => o.Key, v => v.Value);
            if (this.resJudgeDic == null)
            {
                this.resJudgeDic = new Dictionary<object, JumpTargetStruct>();
            }
            return this;
        }
        #endregion
        #endregion
    }
}
