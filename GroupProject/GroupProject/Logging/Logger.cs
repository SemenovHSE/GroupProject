using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;

namespace GroupProject.Logging
{
    public class Logger : ILogger
    {
        private ILog log;
        private ILog Log
        {
            get
            {
                if (log == null)
                {
                    log = LogManager.GetLogger("FileLogger");
                }
                return log;
            }
            set
            {
                log = value;
            }
        }


        public void Info(string message)
        {
            Log.Info(message);
        }


        public void Error(string message)
        {
            Log.Error(message);
        }
    }
}