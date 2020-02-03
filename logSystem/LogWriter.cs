using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace logSystem
{
  public class LogWriter
    {
        static private object locker;
        static private Logger instance=null;

        static public Logger getLog()
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                init();
                return instance;

            }
            
        }
        
        static private void init()
        {
            Console.WriteLine("initializing log system");

            locker = new object();
            
            instance = LogManager.GetCurrentClassLogger();
            instance.Info($"logSystem started at {DateTime.Now}");
            
        }

    }
}
