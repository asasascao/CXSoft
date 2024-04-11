using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CXSoft.Flowsh
{
    /// <summary>
    /// 异常信息
    /// </summary>
    public class CXException:Exception
    {
        #region
        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// 入参信息
        /// </summary>
        public string InParamInfo { get; private set; }
        #endregion

        public CXException():base()
        { }

        public CXException(int level,string methodName,string message) :
            base(message)
        {
            this.Level = level;
            this.MethodName = methodName;
        }

        public CXException(int level,string methodName,string message, List<Param> inParamInfo) :
            base(message)
        {
            this.Level = level;
            this.MethodName = methodName;
            this.InParamInfo = inParamInfo == null || inParamInfo.Count <= 0 ? "" : 
                string.Join(";", inParamInfo.Select(o => o.ToString()));
        }

        public CXException(int level, string methodName, string message, Exception innerException) :
            base(message, innerException)
        {
            this.Level = level;
            this.MethodName = methodName;
        }

        public CXException(int level, string methodName, string message, Exception innerException, List<Param> inParamInfo) :
           base(message, innerException)
        {
            this.Level = level;
            this.MethodName = methodName;
            this.InParamInfo = inParamInfo == null || inParamInfo.Count <= 0 ? "" :
                string.Join(";", inParamInfo.Select(o => o.ToString()));
        }
    }
}
