using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 抽象处理器
    /// </summary>
    public abstract class AbstractHandler
    {
        /// <summary>
        /// 参数转换器
        /// </summary>
        List<ParamTransfer> transfers = new List<ParamTransfer>();
        internal List<ParamTransfer> Transfers => transfers;

        #region
        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; protected set; } = 0;

        /// <summary>
        /// 委托
        /// </summary>
        public DelegateInfo Delegate { get; protected set; }

        /// <summary>
        /// 方法参数名
        /// </summary>
        public List<Param> InParamInfo { get; set; }
        #endregion

        #region
        /// <summary>
        /// 获取处理器
        /// </summary>
        internal Func<ParamTransfer, AbstractHandler> GetHandler { get; set; }
        /// <summary>
        /// 获取处理参数
        /// </summary>
        internal Func<int, string, Param> GetHandlerParam { get; set; }
        #endregion

        public AbstractHandler()
        {
            InParamInfo = new List<Param>();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal virtual AbstractHandler Register(DelegateInfo method)
        {
            this.Delegate = method;
            var parameters = this.Delegate.Parameters?.Select(parameter => new Param() {
                Name = parameter.Name,
                HasDefaultValue = parameter.HasDefaultValue,
                DefaultValue = parameter.DefaultValue,
            });
            if (parameters != null && parameters.Count() > 0)
            {
                InParamInfo.AddRange(parameters);
            }
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="delegate">委托</param>
        /// <param name="param">参数</param>
        /// <param name="getLowLevelRes">低级参数</param>
        /// <returns>返回值</returns>
        internal AbstractHandler Register(Func<ParamTransfer, AbstractHandler> GetHandler = null,
            Func<int, string, Param> GetHandlerParam=null)
        {
            this.GetHandler = GetHandler;
            this.GetHandlerParam = GetHandlerParam;
            return this;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="level">登基</param>
        /// <param name="executeGroupType">执行组别</param>
        /// <returns>返回值</returns>
        internal AbstractHandler Register(int level = 0)
        {
            Level = level;
            return this;
        }

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
        public AbstractHandler AddParamConfigs(string configStr)
        {
            if (configStr == null || string.IsNullOrWhiteSpace(configStr)) return this;
            var striparr = configStr.Split(new string[] { "\r\n" }, StringSplitOptions.None)?.
                Where(o => !string.IsNullOrWhiteSpace(o));
            if (striparr != null && striparr.Count() > 0)
            {
                foreach (var item in striparr)
                {
                    AddParamConfig(item);
                }
            }
            return this;
        }

        /// <summary>
        /// 增加参数配置 
        /// 格式：--param_name--sourcelevel--methodname--source_param_name--in/out--
        /// 
        /// param_name 本方法参数名
        /// sourcelevel 源方法层级
        /// methodname 源方法名
        /// source_param_name 参数名
        /// in/out in和out里面的一个,代表入参或者出参 可缺损缺损时先取出参出参取不到取入参
        /// </summary>
        /// <param name="configStr">配置串</param>
        public AbstractHandler AddParamConfigs(string[] configsStr)
        {
            if (configsStr == null || configsStr.Length <= 0) return this;
            if (configsStr != null && configsStr.Count() > 0)
            {
                foreach (var item in configsStr)
                {
                    AddParamConfig(item);
                }
            }
            return this;
        }

        /// <summary>
        /// 增加参数配置 
        /// 格式：--param_name--sourcelevel--methodname--source_param_name--in/out--
        /// param_name 本方法参数名
        /// sourcelevel 源方法层级
        /// methodname 源方法名
        /// source_param_name 参数名
        /// in/out in和out里面的一个,代表入参或者出参 可缺损缺损时先取出参出参取不到取入参
        /// </summary>
        /// <param name="configStr">配置串</param>
        public AbstractHandler AddParamConfig(string configStr)
        {
            if (configStr == null || string.IsNullOrWhiteSpace(configStr)) return this;
            var paramTransfer=ParamStrToParamTransfer.Transfer(configStr);
            if (paramTransfer == null) return this;
            Transfers.Add(paramTransfer);
            return this;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        private void SetInputParam()
        {
            #region
            if (InParamInfo == null || InParamInfo.Count <= 0) return;
            for (int i = 0; i < InParamInfo.Count; i++)
            {
                var inparamName=InParamInfo[i].Name;
                if(Delegate.param != null&& Delegate.param.ContainsKey(inparamName))
                {
                    InParamInfo[i].ValueInfo = Delegate.param[inparamName];
                }
                else
                {
                    Param param = null;
                    var transfer=transfers.FirstOrDefault(o => o.ParamName == inparamName);
                    var handler = transfer == null ? null : GetHandler?.Invoke(transfer);
                    if (handler != null)
                    {
                        if (transfer.ParamInOut.ToLower() == "in")
                        {
                            param = handler.InParamInfo?.FirstOrDefault(o => o.Name == transfer.SourceParamName);
                            InParamInfo[i].ValueInfo = param != null ? param.ValueInfo : SetDefaultInputParam(inparamName, InParamInfo[i]);
                        }
                        else if (transfer.ParamInOut.ToLower() == "out" && handler.GetType().IsAssignableFrom(typeof(LogicHandler)))
                        {
                            param = ((LogicHandler)handler).OutParamInfo?.FirstOrDefault(o => o.Name == transfer.SourceParamName);
                            InParamInfo[i].ValueInfo = param != null ? param.ValueInfo : SetDefaultInputParam(inparamName, InParamInfo[i]);
                        }
                        else
                        {
                            if (handler.GetType().IsAssignableFrom(typeof(LogicHandler)))
                            {
                                param = ((LogicHandler)handler).OutParamInfo?.FirstOrDefault(o => o.Name == transfer.SourceParamName);
                                if (param != null)
                                {
                                    InParamInfo[i].ValueInfo = param.ValueInfo;
                                }
                                else
                                {
                                    param = handler.InParamInfo?.FirstOrDefault(o => o.Name == transfer.SourceParamName);
                                    InParamInfo[i].ValueInfo = param != null ? param.ValueInfo : SetDefaultInputParam(inparamName, InParamInfo[i]);
                                }
                            }
                            else
                            {
                                param = handler.InParamInfo?.FirstOrDefault(o => o.Name == transfer.SourceParamName);
                                InParamInfo[i].ValueInfo = param != null ? param.ValueInfo : SetDefaultInputParam(inparamName, InParamInfo[i]);
                            }
                        }
                    }
                    else
                    {
                        InParamInfo[i].ValueInfo= SetDefaultInputParam(inparamName, InParamInfo[i]);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 设置默认参数
        /// </summary>
        /// <param name="inparamName">入参名称</param>
        /// <param name="target_param">目标参数</param>
        /// <returns>返回参数</returns>
        private object SetDefaultInputParam(string inparamName, Param target_param)
        {
            #region
            var param = GetHandlerParam?.Invoke(Level, inparamName);
            if (param != null)
            {
                return param.ValueInfo;
            }
            else if (target_param.HasDefaultValue)
            {
                return target_param.DefaultValue;
            }
            else if (param.HasDefaultValue)
            {
                return param.DefaultValue;
            }
            else
            {
                return null;
            }
            #endregion
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>执行结果</returns>
        public abstract ResStruct Execute();

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">错误信息</exception>
        protected object DoExecute()
        {
            #region
            if (Delegate == null) return null;

            try
            {
                SetInputParam();
                if (InParamInfo != null && InParamInfo.Count > 0)
                {
                    var paraml = InParamInfo.Select(o => o.ValueInfo).ToArray();
                    return Delegate.@delegate.Method?.Invoke(Delegate.@delegate.Target, paraml);
                }
                else
                {
                    return Delegate.@delegate.Method?.Invoke(Delegate.@delegate.Target,null);
                }
            }
            catch (ArgumentException aex)
            {
                string errmsg = "方法" + Delegate.MethodName + "出错:" + aex.Message;
                throw new CXArgumentException(Level, Delegate.MethodName, errmsg, InParamInfo);
            }
            catch (Exception ex)
            {
                string errmsg = "方法" + Delegate.MethodName + "出错:" + ex.Message;
                throw new CXException(Level, Delegate.MethodName, errmsg, InParamInfo);
            }
            #endregion
        }
    }
}
