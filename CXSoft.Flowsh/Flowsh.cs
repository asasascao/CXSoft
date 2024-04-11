using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 流程线
    /// </summary>
    public class Flowsh: IFlowsh
    {
        List<LevelList<AbstractHandler>> handlers = 
            new List<LevelList<AbstractHandler>>();//方法线

        #region
        /// <summary>
        /// 索引
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int Type { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Discription { get; set; }
        #endregion

        public Flowsh()
        {
            Key = Guid.NewGuid().ToString();
            Type = (int)FlowshType.Normal;
            Discription = "";
        }

        /// <summary>
        /// 流程
        /// </summary>
        /// <param name="type">类别</param>
        /// <param name="discription">描述</param>
        public Flowsh(int type = (int)FlowshType.Normal, string discription = "")
        {
            Key = Guid.NewGuid().ToString();
            Type = type;
            Discription = discription;
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="level">层级 默认传-1为顺序模式</param>
        /// <param name="steps">步骤 --委托 --层级 默认传-1为顺序模式</param>
        /// <returns>流程</returns>
        public Flowsh AddHandlers(params Tuple<DelegateInfo,int>[] steps)
        {
            #region
            if (steps != null && steps.Count() > 0)
            {
                foreach (var step in steps)
                {
                    AddHandler(step.Item1, step.Item2);
                }
            }
            return this;
            #endregion
        }

        /// <summary>
        /// 添加层级步骤
        /// </summary>
        /// <param name="level">层级 默认传-1为顺序模式</param>
        /// <param name="steps">步骤</param>
        /// <returns>流程</returns>
        public Flowsh AddLevelHandlers(int level = -1, params DelegateInfo[] steps)
        {
            #region
            if (level == -1)
            {
                level = (handlers == null || handlers.Count <= 0) ? 0 : handlers.Max(o => o.Level);
            }
            if (steps != null && steps.Count() > 0)
            {
                foreach (var step in steps)
                {
                    AddHandler(step, level);
                }
            }
            return this;
            #endregion
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="delegate">委托</param>
        /// <param name="level">层级 默认传-1为顺序模式</param>
        public Flowsh AddHandler(DelegateInfo @delegate, int level = -1)
        {
            #region
            if (level == -1)
            {
                level = (handlers == null || handlers.Count <= 0) ? 0 : handlers.Max(o => o.Level)+1;
            }

            AbstractHandler handler = null;
            if (@delegate is LogicDelegateInfo)
            {
                handler = new LogicHandler().Register(@delegate).
                    Register(level).Register(GetHandler, GetHandlerParam).
                    AddParamConfigs(@delegate.ParamConfigs?.ToArray());
            }
            else if (@delegate is JudgeDelegateInfo)
            {
                handler = new JudgeHandler().Register(@delegate).Register(level).
                    AddParamConfigs(@delegate.ParamConfigs?.ToArray());
            }
            else if (@delegate is AsyncDelegateInfo)
            {
                handler = new AsyncHandler().Register(@delegate).
                    Register(level).Register(GetHandler, GetHandlerParam).
                    AddParamConfigs(@delegate.ParamConfigs?.ToArray());
            }
            if (handler == null)
            {
                throw new ArgumentException("请正确使用步骤");
            }

            if (handlers.Exists(o=>o.Level == level))
            {
                var hand=handlers.First(o => o.Level == level);
                hand.Add(handler);
            }
            else
            {
                var ll=new LevelList<AbstractHandler>();
                ll.Add(handler);
                ll.Level = level;
                handlers.Add(ll);
            }
            return this;
            #endregion
        }

        /// <summary>
        /// 获取委托信息列表
        /// </summary>
        /// <param name="level">步骤次序</param>
        /// <param name="methodname">方法名</param>
        /// <returns>委托信息集</returns>
        public IEnumerable<DelegateInfo> GetDelegateByLevelArray(int level,string methodname)
        {
            #region
            var hads = handlers.FirstOrDefault(o => o.Level == level);
            if (hads!=null)
            {
                if (string.IsNullOrWhiteSpace(methodname)) return hads?.Select(o => o.Delegate);
                return hads?.Where(o=>o.Delegate.MethodName== methodname)?.Select(o => o.Delegate);
            }
            return null;
            #endregion
        }

        /// <summary>
        /// 获取委托信息
        /// </summary>
        /// <param name="level">步骤次序</param>
        /// <param name="methodname">方法名</param>
        /// <param name="methodindex">方法顺序</param>
        /// <returns>委托信息</returns>
        public DelegateInfo GetDelegateByLevel(int level, string methodname,int methodindex=0)
        {
            #region
            if (methodindex < 0) return null;
            var deles= GetDelegateByLevelArray(level, methodname)?.ToArray();
            return (deles != null && deles.Length > 0 && methodindex <= deles.Length - 1) ? deles[methodindex] : null;
            #endregion
        }

        /// <summary>
        /// 获取步骤图谱
        /// </summary>
        /// <returns>图谱</returns>
        public string[,] GetHandlers()
        {
            var zx=handlers.Max(o => o.Count);
            var hx = handlers.Count;
            string[,] res = new string[hx, zx];
            for (int i = 0; i < hx; i++)
            {
                for (int j = 0; j < zx; j++)
                {
                    res[i, j] = "";
                    if (j+1 <= handlers[i].Count)
                    {
                        res[i, j] = handlers[i][j].Delegate.MethodName;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 获取处理器
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>处理器</returns>
        private AbstractHandler GetHandler(ParamTransfer param)
        {
            #region
            var handles=handlers.FirstOrDefault(o => o.Level == param.SourceLevel);
            if(handles!=null&& handles.Count()>0)
            {
                var hanls=handles.Where(o => o.Delegate.MethodName == param.MethodName)?.ToArray();
                if(hanls != null && hanls.Length > 0 && param.MethodIndex < hanls.Count())
                {
                    return hanls[param.MethodIndex];
                }
            }
            return null;
            #endregion
        }

        /// <summary>
        /// 获取处理器上一参数
        /// </summary>
        /// <param name="level">最高等级</param>
        /// <param name="param_name">参数名</param>
        /// <returns>参数值</returns>
        private Param GetHandlerParam(int level,string param_name)
        {
            #region
            var handles = handlers?.Where(o => o.Level < level)?.ToArray();
            if (handles != null && handles.Count() > 0)
            {
                for (int i = handles.Length-1; i >=0; i--)
                {
                    for (int j = handles[i].Count-1; j >=0 ; j--)
                    {
                        if (handles[i][j].GetType().IsAssignableFrom(typeof(LogicHandler)))
                        {
                            var paramv = ((LogicHandler)handles[i][j]).OutParamInfo?.FirstOrDefault(o => o.Name == param_name);
                            if(paramv != null)
                            {
                                return paramv;
                            }
                            else
                            {
                                paramv = handles[i][j].InParamInfo?.FirstOrDefault(o => o.Name == param_name);
                                if (paramv != null)
                                {
                                    return paramv;
                                }
                            }
                        }
                        else
                        {
                            var paramv = handles[i][j].InParamInfo?.FirstOrDefault(o => o.Name == param_name);
                            if (paramv != null)
                            {
                                return paramv;
                            }
                        }
                    }
                }
            }
            return null;
            #endregion
        }

        /// <summary>
        /// 执行
        /// </summary>
        public Dictionary<string, object> Execute()
        {
            #region
            if (handlers == null || handlers.Count <= 0) return null;
            object sock_excu = new object();
            Dictionary<string, object> res = new Dictionary<string, object>();
            int curLevel = 0;
            try
            {
                var handler = handlers.OrderBy(o => o.Level);
                int lastLevel = handler.Max(o => o.Level);
                var levels = handler.Select(o => o.Level).ToArray();
                for (int i = 0; i < levels.Length; i++)
                {
                    curLevel = levels[i];
                    var handleritem = handler.FirstOrDefault(o => o.Level == curLevel);
                    if (handleritem == null || handleritem.Count <= 0) continue;
                    if (handleritem.Count == 1)
                    {
                        var resh=handleritem[0].Execute();
                        if(resh.Type == ResType.Normal)
                        {
                            if (lastLevel == handleritem.Level)
                            {
                                res.Add(resh.Name, resh.Res);
                            }
                        }
                        else
                        {
                            var level=resh.JumpTarget.Level;
                            i = Array.IndexOf(levels, level)-1;
                            if (i < 0) i = -1;
                            continue;
                        }
                    }
                    else
                    {
                        var list=handleritem.Where(o => o.GetType().IsAssignableFrom(typeof(LogicHandler)));
                        if(list!=null&& list.Count() == 1)
                        {
                            handleritem[0].Execute();
                            if (lastLevel == handleritem.Level)
                            {
                                lock (sock_excu)
                                {
                                    foreach (LogicHandler o in handleritem)
                                    {
                                        var opi = o.OutParamInfo.FirstOrDefault(s => s.Name == o.DefaultResName);
                                        if (res.ContainsKey(opi.Name))
                                        {
                                            res[o.DefaultResName] = opi.ValueInfo;
                                        }
                                        else
                                        {
                                            res.Add(o.DefaultResName, opi.ValueInfo);
                                        }
                                    }
                                }
                            }
                        }
                        else if (list != null && list.Count() > 1)
                        {
                            list.AsParallel().ForAll(o => o.Execute());
                            if (lastLevel == handleritem.Level)
                            {
                                lock (sock_excu)
                                {
                                    foreach (LogicHandler o in handleritem)
                                    {
                                        var opi = o.OutParamInfo.FirstOrDefault(s => s.Name == o.DefaultResName);
                                        if (res.ContainsKey(opi.Name))
                                        {
                                            res[o.DefaultResName] = opi.ValueInfo;
                                        }
                                        else
                                        {
                                            res.Add(o.DefaultResName, opi.ValueInfo);
                                        }
                                    }
                                }
                            }
                        }
                        list = handleritem.Where(o => o.GetType().IsAssignableFrom(typeof(JudgeHandler)));
                        if (list != null && list.Count() >= 1)
                        {
                            var resh = handleritem[0].Execute();
                            if (resh.Type == ResType.Normal)
                            {
                                if (lastLevel == handleritem.Level)
                                {
                                    res.Add(resh.Name, resh.Res);
                                }
                            }
                            else
                            {
                                var level = resh.JumpTarget.Level;
                                i = Array.IndexOf(levels, level) - 1;
                                if (i < 0) i = -1;
                                continue;
                            }
                        }
                    }
                }
                return res;
            }
            catch (ArgumentException aex)
            {
                throw new CXArgumentException(curLevel,"","流程" + curLevel + "层出现错:" + aex.Message);
            }
            catch (Exception ex)
            {
                throw new CXException(curLevel, "", "流程" + curLevel + "层出现错:" + ex.Message);
            }
            #endregion
        }
    }
}
