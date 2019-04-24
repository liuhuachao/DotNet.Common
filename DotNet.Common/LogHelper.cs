using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DotNet.Common
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        private ILog _log;

        public LogHelper(string logger)
        {
            _log = LogManager.GetLogger(logger) ;
        }

        public void Info(object msg)
        {
            _log.Info(msg);
        }

        public void Debug(object msg)
        {
            _log.Debug(msg);
        }

        public void Error(object msg)
        {
            _log.Error(msg);
        }

        public void Fatal(object msg)
        {
            _log.Fatal(msg);
        }

        public void Warn(object msg)
        {
            _log.Warn(msg);
        }
    }
}
